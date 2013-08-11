using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DazCamUI.Helpers;

namespace DazCamUI.Controller
{
    public class ControllerSettings
    {
        #region Declarations

        private string _fileName;

        public enum BedCorner { BottomLeft, BottomRight, TopLeft, TopRight };

        public struct SectionLabel
        {
            public string Name;
            public bool Active;
        }

        #endregion

        #region Properties

        public string MachineIPAddress;

        public AxisSettings XAxis { get; set; }
        public AxisSettings YAxis { get; set; }
        public AxisSettings ZAxis { get; set; }

        public int FeedRateMaximum { get; set; }            // Used for Rapid traverse etc
        public int FeedRateManual { get; set; }             // Used for manual moves and cuts
        public int FeedRateCuttingDefault { get; set; }     // Used if g-code doesn't specify an F parameter

        // Angles from 0 to 180
        public int ZServoLoadPosition { get; set; }
        public int ZServoTopPosition { get; set; }
        public int ZServoBottomPosition { get; set; }

        public BedCorner HomeCorner { get; set; }           // Determines the Left/Right Handedness of the coordinate system for actions such as g02/g03

        public List<WorkspaceOffset> WorkspaceOffsets { get; set; }

        public OffsetType.GCodeIdentifiers ActiveWorkingCoordinateSystem { get; set; }
        public OffsetType.GCodeIdentifiers ActiveToolOffset { get; set; }
        public bool ToolLengthOffsetActive { get; set; }
        public double ArcLineSegmentLength { get; set; }
        
        public string GCodeExecSearchPath { get; set; }     // multiple search paths can be stored, separated by ';'
        public string ExecHeaderFile { get; set; }
        public string ExecFooterFile { get; set; }

        public bool IgnoreLimitsDuringExecution { get; set; }       // ignores limit switches while g-code is executing

        public List<SectionLabel> SectionLabels { get; set; }

        #endregion

        #region Constructors

        public ControllerSettings()
        {
            MachineIPAddress = "192.168.0.100";

            XAxis = new AxisSettings();
            YAxis = new AxisSettings();
            ZAxis = new AxisSettings();

            // Default settings, which will be changed in a machine settings window
            FeedRateMaximum = 50;
            FeedRateManual = 20;
            FeedRateCuttingDefault = 10;

            XAxis.InvertDirection = true;
            XAxis.StepsPerTurn = 200;
            XAxis.TurnsPerInch = 2;

            YAxis.StepsPerTurn = 200;
            YAxis.TurnsPerInch = 2;

            ZAxis.InvertDirection = true;
            ZAxis.StepsPerTurn = 200;
            ZAxis.TurnsPerInch = 5;

            ZServoLoadPosition = 0;
            ZServoTopPosition = 30;
            ZServoBottomPosition = 45;

            HomeCorner = BedCorner.BottomRight;
            WorkspaceOffsets = new List<WorkspaceOffset>();

            ActiveWorkingCoordinateSystem = OffsetType.GCodeIdentifiers.G54;
            ActiveToolOffset = OffsetType.GCodeIdentifiers.T1;
            ToolLengthOffsetActive = false;

            ArcLineSegmentLength = .05;

            GCodeExecSearchPath = @"C:\CreativeDepths\DazCamUI\G-Code programs";

            SectionLabels = new List<SectionLabel>();
        }

        #endregion

        #region Public Methods

        public static ControllerSettings Load(string fileName)
        {
            ControllerSettings settings;

            if (!File.Exists(fileName))
            {
                settings = new ControllerSettings();
            }
            else
            {
                string xml = File.ReadAllText(fileName);
                settings = XMLSerializer.DeserializeObject(xml, typeof(ControllerSettings)) as ControllerSettings;
            }

            settings._fileName = fileName;
            return settings;
        }

        public void SaveAs(string fileName)
        {
            _fileName = fileName;
            Save();
        }

        public void Save()
        {
            string xml = XMLSerializer.SerializeObject(this);
            File.WriteAllText(_fileName, xml);
        }

        /// <summary>
        /// Converts the supplied targetFeedRate to a millisecond delay value to be used between step pulses, and a step resolution
        /// </summary>
        /// <param name="targetFeedRate">Desired Feed Rate in IPM</param>
        /// <param name="resolution">Resulting feed rate to be used</param>
        /// <returns>millisecond step pulse delay value to pass to the machine</returns>
        public int CalculateStepDelayFromFeedRate(int targetFeedRate, out FeedRate.StepResolutions resolution)
        {
            //TODO: These need to be stored as settings with the ability to measure them and determine their values.. right now everything
            //      is hard coded from the charting of my manual trial and error

            if (targetFeedRate > FeedRateMaximum) targetFeedRate = FeedRateMaximum;
            if (targetFeedRate < 5) targetFeedRate = 5;

            resolution = FeedRate.StepResolutions.Half;     //TODO: We've calculated these all on half steps which seem to work great for the full range
            // (Really making me wonder why I support changing resolutions)

            if (targetFeedRate > 50) return LinearInterpolation(50, 17, targetFeedRate, 60, 9);          // 51 to 60
            if (targetFeedRate > 40) return LinearInterpolation(40, 28, targetFeedRate, 50, 17);          // 41 to 50
            if (targetFeedRate > 30) return LinearInterpolation(30, 42, targetFeedRate, 40, 28);          // 31 to 40
            if (targetFeedRate > 20) return LinearInterpolation(20, 80, targetFeedRate, 30, 42);          // 21 to 30
            if (targetFeedRate > 10) return LinearInterpolation(10, 170, targetFeedRate, 20, 80);          // 11 to 20
            if (targetFeedRate >= 5) return LinearInterpolation(5, 350, targetFeedRate, 10, 170);          // 5 to 10

            return 40;      //Not used
        }

        /// <summary>
        ///  Returns True if the natural coordinate system used by the macine is Right handed. G02/G03 and other functions need to know this 
        ///  when calculating clockwise/counter-clockwise arcs. 
        ///  See ***(FILL URL)*** for explanation of LH vs RH coordinates
        /// </summary>
        /// <returns></returns>
        public bool BedIsRightHandedCoordinateSystem()
        {
            return (HomeCorner == BedCorner.BottomLeft || HomeCorner == BedCorner.TopRight);
        }

        public AxisSettings GetAxisSettings(string axis)
        {
            switch (axis.Trim().ToUpper())
            {
                case "X": return XAxis;
                case "Y": return YAxis;
                case "Z": return ZAxis;
                default: throw new Exception("Invalid Axis Specified");
            }
        }

        public Offset CombinedWorkspaceOffset(OffsetType.GCodeIdentifiers gCodeIdentifier)
        {
            var offset = new Offset();

            var workspaceOffsets = from o in WorkspaceOffsets
                                   where o.Type.GCodeIdentifier == gCodeIdentifier && o.Active == true
                                   select o;

            foreach (var workspaceOffset in workspaceOffsets) offset += workspaceOffset;

            return offset;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Solves for y2 where a line x1,y1 - x3,y3 is known and the mid point is found on x2.
        /// Source: http://www.ajdesigner.com/phpinterpolation/linear_interpolation_equation.php
        /// </summary>
        private int LinearInterpolation(int x1, int y1, int x2, int x3, int y3)
        {
            var y2 = (((x2 - x1) * (y3 - y1)) / (x3 - x1)) + y1;
            return y2;
        }

        #endregion
    }
}
