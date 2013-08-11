using System.Collections.Generic;
using DazCamUI.Controller;
using System;

namespace DazCamUI.GCode
{
    public class GCodeParser
    {
        #region Event Declarations

        public delegate void OnProgressNotifyEventHandler(string progressMessage, out bool cancel);
        public event OnProgressNotifyEventHandler OnProgressNotify;

        #endregion

        #region Declarations

        private Machine _machine;
        private double _drillDepthIncrement = 0;
        private double _drillSurfaceStart = 0;
        private int _dwellDurationMS = 250;

        private bool _oneTimeMachineCoordinateSystem;
        private Offset _machineOffsetHold;

        #endregion

        #region Properties

        public bool AbsoluteMode { get; set; }
        public int G1FeedRate { get; set; }
        public bool ProgramFinished { get; set; }

        public OffsetType.GCodeIdentifiers WorkspaceOffset { get; set; }
        public OffsetType.GCodeIdentifiers ToolOffset { get; set; }
        public bool ToolLengthCompensation { get; set; }

        #endregion  

        #region Constructors

        public GCodeParser()
        {
            _machine = Machine.GetMachine();

            AbsoluteMode = true;   //Default to G90 mode (all coordinates as absolutes)
            G1FeedRate = _machine.Settings.FeedRateCuttingDefault;
            ProgramFinished = false;
        }

        #endregion

        #region Public Methods

        public void ExecuteCode(string gCode)
        {
            var gCodeLines = new List<string>();

            gCodeLines.AddRange(gCode.Split(new[] { "\r\n" }, StringSplitOptions.None));
            ExecuteCode(gCodeLines);
        }

        public void ExecuteCode(List<string> gCode, bool nestedCall = false)
        {
            int linesParsed = 0;
            int linesToParse = gCode.Count;
            bool cancel = false;

            SetMachineWorkingOffset();
            
            // Header File
            if (!nestedCall && _machine.Settings.ExecHeaderFile.Length > 0)
                ExecuteCode(GCodeFunctions.ExecuteGCodeFile(_machine.Settings.ExecHeaderFile), true);

            foreach (string line in gCode)
            {
                if (ProgramFinished) break;

                string normalizedLine = line.Split('/')[0];             // The '/' signifies a remark to the end of the line
                normalizedLine = normalizedLine.Split('(')[0];          // The '(' signifies an inline/multiline remark in standard Gcode, but we're only treating it as a line comment or end of line comment
                normalizedLine = normalizedLine.Trim();

                // Raise event to notify caller of progress
                linesParsed++;
                if (!nestedCall && OnProgressNotify != null) 
                    OnProgressNotify(string.Format("Parsing G-Code: {0} of {1}", linesParsed, linesToParse), out cancel);

                if (cancel) break;
                if (normalizedLine.Length == 0) continue;

                // --- Extended (non-gcode) functions begin with a '!'
                if (normalizedLine.StartsWith("!"))
                {
                    ExecuteExtendedFunction(normalizedLine);
                }

                // --- Everything else is gcode
                else
                {
                    var blocks = GCodeBlock.Parse(normalizedLine);

                    foreach (var block in blocks)
                    {
                        if (block.G.HasValue || block.T.HasValue) ExecuteGBlock(block);
                        if (block.M.HasValue) ExecuteMBlock(block);
                    }
                }
            }

            // Footer File
            if (!nestedCall && _machine.Settings.ExecFooterFile.Length > 0)
                ExecuteCode(GCodeFunctions.ExecuteGCodeFile(_machine.Settings.ExecFooterFile), true);

        }

        #endregion

        #region Private Methods

        private void ExecuteMBlock(GCodeBlock block)
        {

        }

        private void ExecuteGBlock(GCodeBlock block)
        {
            // Remember sticky words for subsequent operations
            if (block.F.HasValue) G1FeedRate = (int)Math.Floor((double)block.F);
            if (block.Q.HasValue) _drillDepthIncrement = (double)block.Q;
            if (block.R.HasValue) _drillSurfaceStart = (double)block.R;
            if (block.P.HasValue) _dwellDurationMS = (int)Math.Abs((Math.Floor((double)block.P)));

            // Set Absolute coordinate mode
            if (block.G == 90)
            {
                AbsoluteMode = true;
            }

            // Set Incremental coordinate mode
            if (block.G == 91)
            {
                AbsoluteMode = false;
            }

            // Causes next G0, G1, G2, or G3 to use Machine coordinate system - only applies once
            if (block.G == 53)
            {
                _oneTimeMachineCoordinateSystem = true;
            }

            // G49 cancels G43 
            if (block.G == 49)
            {
                ToolLengthCompensation = false;
                SetMachineWorkingOffset();
            }

            // G43 - Sets tool length compensation
            if (block.G == 43)
            {
                ToolLengthCompensation = true;
                SetMachineWorkingOffset();
            }

            // G53 - G59 -- Working Coordinate System change
            if (block.G >= 54 && block.G <= 59)
            {
                SetWorkspaceOffset((double)block.G);
            }

            if (block.T >= 1 && block.T <= 9)
            {
                SetToolOffset((double)block.T);
            }

            // G00 - Rapid Traverse
            if (block.G == 0)
            {
                BeforeMoveSetMCS();
                _machine.MoveTo(GetMoveTargetCoordinate(block), !AbsoluteMode, false, _machine.Settings.FeedRateMaximum);
                AfterMoveRevertMCS();
            }

            // G01 - Line to
            if (block.G == 1)
            {
                BeforeMoveSetMCS();
                _machine.MoveTo(GetMoveTargetCoordinate(block), !AbsoluteMode, true, G1FeedRate);
                AfterMoveRevertMCS();
            }

            // G02/G03 - Arc clockwise/counter-clockwise to a point (Does not currently support Incremental mode)
            if (block.G == 2 || block.G == 3)
            {
                bool clockwise = block.G == 2;
                if (!block.I.HasValue) block.I = 0;
                if (!block.J.HasValue) block.J = 0;

                if (block.I == 0 && block.J == 0) throw new Exception("I and J may not both be zero for a g02/g03");

                var machineLocation = _machine.GetWorkingLocation();
                var finish = GetMoveTargetCoordinate(block);
                var center = new Coordinate(machineLocation.X + (double)block.I, machineLocation.Y + (double)block.J, machineLocation.Z);

                //TODO: make sure this supports incremental mode (which I don't think it does now)
                BeforeMoveSetMCS();
                ExecuteCode(GCodeFunctions.GenerateArc(machineLocation, finish, center, clockwise), true);
                AfterMoveRevertMCS();
            }

            // G4 - Pause in milliseconds. EG: G04 P250 -- P is sticky
            if (block.G == 4)
            {
                _machine.SendDwell(_dwellDurationMS);
            }

            // G83 - Drilling cycle with pecks: Accepts X, Y, Z, P (Depth increment), Q (Dwell time in ms) and R (Work surface Z) values
            //       P, Q and R are sticky
            if (block.G == 83 || block.G == 82 || block.G == 81)
            {
                _oneTimeMachineCoordinateSystem = false;        // Not supported for G83
                var location = GetMoveTargetCoordinate(block);
                ExecuteCode(GCodeFunctions.GeneratePeckingDrillCycle(location, _drillDepthIncrement, _dwellDurationMS, _drillSurfaceStart), true);
            }
        }

        private void ExecuteExtendedFunction(string line)
        {
            var lineParts = line.Split(':');
            string functionName = lineParts[0].ToUpper();
            string parameters = lineParts.Length > 1 ? lineParts[1] : "";

            switch (functionName)
            {
                case "!Z-TOP":
                    _machine.MoveZServo(_machine.Settings.ZServoTopPosition);
                    _machine.SendDwell(500);        
                    break;

                case "!Z-BOTTOM":
                    _machine.MoveZServo(_machine.Settings.ZServoBottomPosition, true);
                    break;

                case "!Z-LOAD":
                    _machine.MoveZServo(_machine.Settings.ZServoLoadPosition);
                    _machine.SendDwell(500);        
                    break;

                case "!END":
                    ProgramFinished = true;
                    break;

                case "!EXEC":
                    ExecuteCode(GCodeFunctions.ExecuteGCodeFile(parameters), true);
                    break;

                default:
                    throw new Exception("Invalid extended function");
            }
        }

        /// <summary>
        /// Converts block X, Y and Z params into a Coordinate, substituting current machine locations for those that don't 
        /// have values supplied in the block
        /// </summary>
        private Coordinate GetMoveTargetCoordinate(GCodeBlock block)
        {
            var coordinate = new Coordinate();
            Coordinate machineLocation = _machine.GetWorkingLocation();

            if (block.X.HasValue)
            {
                coordinate.X = (double)block.X;
            }
            else
            {
                if (AbsoluteMode) coordinate.X = machineLocation.X;
                else coordinate.X = 0;
            }

            if (block.Y.HasValue)
            {
                coordinate.Y = (double)block.Y;
            }
            else
            {
                if (AbsoluteMode) coordinate.Y = machineLocation.Y;
                else coordinate.Y = 0;
            }

            if (block.Z.HasValue)
            {
                coordinate.Z = (double)block.Z;
            }
            else
            {
                if (AbsoluteMode) coordinate.Z = machineLocation.Z;
                else coordinate.Z = 0;
            }

            return coordinate;
        }

        /// <summary>
        /// Pass a 54 to set G54 etc
        /// </summary>
        private void SetWorkspaceOffset(double gCodeIdentifier)
        {
            if (gCodeIdentifier == 54) WorkspaceOffset = OffsetType.GCodeIdentifiers.G54;
            if (gCodeIdentifier == 55) WorkspaceOffset = OffsetType.GCodeIdentifiers.G55;
            if (gCodeIdentifier == 56) WorkspaceOffset = OffsetType.GCodeIdentifiers.G56;
            if (gCodeIdentifier == 57) WorkspaceOffset = OffsetType.GCodeIdentifiers.G57;
            if (gCodeIdentifier == 58) WorkspaceOffset = OffsetType.GCodeIdentifiers.G58;
            if (gCodeIdentifier == 59) WorkspaceOffset = OffsetType.GCodeIdentifiers.G59;
            SetMachineWorkingOffset();
        }

        /// <summary>
        /// Pass a 1 to set T1 etc
        /// </summary>
        /// <param name="gCodeIdentifier"></param>
        private void SetToolOffset(double gCodeIdentifier)
        {
            if (gCodeIdentifier == 1) ToolOffset = OffsetType.GCodeIdentifiers.T1;
            if (gCodeIdentifier == 2) ToolOffset = OffsetType.GCodeIdentifiers.T2;
            if (gCodeIdentifier == 3) ToolOffset = OffsetType.GCodeIdentifiers.T3;
            if (gCodeIdentifier == 4) ToolOffset = OffsetType.GCodeIdentifiers.T4;
            if (gCodeIdentifier == 5) ToolOffset = OffsetType.GCodeIdentifiers.T5;
            if (gCodeIdentifier == 6) ToolOffset = OffsetType.GCodeIdentifiers.T6;
            if (gCodeIdentifier == 7) ToolOffset = OffsetType.GCodeIdentifiers.T7;
            if (gCodeIdentifier == 8) ToolOffset = OffsetType.GCodeIdentifiers.T8;
            if (gCodeIdentifier == 9) ToolOffset = OffsetType.GCodeIdentifiers.T9;
            SetMachineWorkingOffset();
        }

        private void SetMachineWorkingOffset()
        {
            var offset = new Offset();

            offset += _machine.Settings.CombinedWorkspaceOffset(WorkspaceOffset);
            offset += _machine.Settings.CombinedWorkspaceOffset(ToolOffset);
            if (ToolLengthCompensation) offset += _machine.Settings.CombinedWorkspaceOffset(OffsetType.GCodeIdentifiers.G43);

            _machine.WorkingOffset = offset;
        }

        /// <summary>
        /// Temporarily disables any working offsets if we are in G53 mode
        /// </summary>
        private void BeforeMoveSetMCS()
        {
            if (!_oneTimeMachineCoordinateSystem) return;
            
            _machineOffsetHold = _machine.WorkingOffset;
            _machine.WorkingOffset = null;
        }

        /// <summary>
        /// Restores working offsets if we just moved in G53 mode
        /// </summary>
        private void AfterMoveRevertMCS()
        {
            if (!_oneTimeMachineCoordinateSystem) return;

            _machine.WorkingOffset = _machineOffsetHold;
            _machineOffsetHold = null;
            _oneTimeMachineCoordinateSystem = false;
        }

        #endregion
    }
}
