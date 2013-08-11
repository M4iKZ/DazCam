using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using DazCamUI.Controller;
using System.IO;

namespace DazCamUI.GCode
{
    public class GCodeFunctions
    {
        #region Declarations

        public enum FunctionTemplates
        {
            End,
            ZTop,
            ZBottom,
            ZLoad,
            Exec
        }

        #endregion

        #region Public Static Methods

        public static string GetFunctionTemplate(FunctionTemplates functionType)
        {
            switch (functionType)
            {
                case FunctionTemplates.End:
                    return "!End";

                case FunctionTemplates.ZTop:
                    return "!Z-Top";

                case FunctionTemplates.ZBottom:
                    return "!Z-Bottom";

                case FunctionTemplates.ZLoad:
                    return "!Z-Load";

                case FunctionTemplates.Exec:
                    return "!Exec: Count, Filename";
            }

            return "";
        }

        /// <summary>
        /// Expands a G02/G03 arc into gcode for straight line segments to create the same result. The resulting arc is a flat 2D arc or a
        /// helix arc if start.Z and finish.Z are different, and so center.Z is ignored.
        /// </summary>
        public static List<string> GenerateArc(Coordinate start, Coordinate finish, Coordinate center, bool isClockwise)
        {
            var functionCode = new List<string>();

            // The mathematics in this function were originally written for a Left Handed coordinate system. If the coordinate origin is in a corner that
            // causes the coordinate system to be Right Handed, then we need to treat the clockwise/counter-clockwise direction of the arc as the opposite.
            if (Machine.GetMachine().Settings.BedIsRightHandedCoordinateSystem()) isClockwise = !isClockwise;

            functionCode.Add(string.Format("(Arc To {0:0.0000},{1:0.0000}", finish.X, finish.Y));

            double radius = getHypotenuseOfTriangle(start.X - center.X, start.Y - center.Y);
            double radianStart = Math.Atan2(start.Y - center.Y, start.X - center.X);
            double radianFinish = Math.Atan2(finish.Y - center.Y, finish.X - center.X);

            if (isClockwise && radianFinish <= radianStart) radianFinish += (2 * Math.PI);
            if (!isClockwise && radianFinish >= radianStart) radianFinish -= (2 * Math.PI);

            double segmentLength = Machine.GetMachine().Settings.ArcLineSegmentLength;
            double radianArc = Math.Abs(radianFinish - radianStart);
            double arcLength = (2 * radius * Math.PI) * (radianArc / (2 * Math.PI));
            double radianIncrement = radianArc / (arcLength / segmentLength);
            int segmentCount = (int)Math.Ceiling( radianArc / radianIncrement);  

            if (!isClockwise) radianIncrement = -radianIncrement;

            double zIncrement = 0;
            string gcodeLine = "g1 X{0:0.0000} Y{1:0.0000}";

            if (start.Z != finish.Z)
            {
                gcodeLine += " Z{2:0.0000}";
                zIncrement = (finish.Z - start.Z) / segmentCount;
            }

            double radians = radianStart;
            double x;
            double y;
            double z = start.Z + zIncrement;

            while (true)
            {
                radians += radianIncrement;

                if (isClockwise && radians > radianFinish) radians = radianFinish;
                if (!isClockwise && radians < radianFinish) radians = radianFinish;

                x = Math.Cos(radians) * radius + center.X;
                y = Math.Sin(radians) * radius + center.Y;
                functionCode.Add(string.Format(gcodeLine, x, y, z));

                z += zIncrement;
                if (zIncrement > 0 && z > finish.Z) z = finish.Z;
                if (zIncrement < 0 && z < finish.Z) z = finish.Z;

                if (isClockwise && radians >= radianFinish) break;
                if (!isClockwise && radians <= radianFinish) break;
            }

            // In case we're close but not quite there due to rounding error
            if (x != finish.X || y != finish.Y || z!=finish.Z) functionCode.Add(string.Format(gcodeLine, finish.X, finish.Y, finish.Z));

            return functionCode;
        }

        /// <summary>
        /// Creates standard g-code for a pecking drill movement at the supplied location (where Z=target depth). The drilling
        /// will advance by increment (if 0, then the full depth will be drilled) with a dwell at the bottom of each peck, then
        /// returning to the Z where the drilling started
        /// </summary>
        public static List<string> GeneratePeckingDrillCycle(Coordinate targetLocation, double increment, double dwell, double workSurfaceZ)
        {
            var functionCode = new List<string>();

            //TODO: To make this support G91 mode, we may need to add code to set to G90 then back to G91 if we were in G91

            double startZ = Machine.GetMachine().GetWorkingLocation().Z;
            functionCode.Add(string.Format("(Peck Drill at: {0:0.0000},{1:0.0000}", targetLocation.X, targetLocation.Y));
            functionCode.Add(string.Format("g0 x{0} y{1}", targetLocation.X, targetLocation.Y));

            if (increment == 0) increment = workSurfaceZ - targetLocation.Z;

            increment = Math.Abs(increment);
            if (startZ > targetLocation.Z) increment = -increment;
            double z = workSurfaceZ;

            while (z != targetLocation.Z)
            {
                z += increment;

                if ((increment < 0 && z < targetLocation.Z) || (increment > 0 && z > targetLocation.Z)) z = targetLocation.Z;

                functionCode.Add(string.Format("g1 z{0}", z));
                functionCode.Add(string.Format("g4 P{0}", dwell));
                functionCode.Add(string.Format("g0 z{0}", z - increment));
            }

            functionCode.Add(string.Format("g0 z{0}", startZ));

            return functionCode;
        }

        /// <summary>
        /// Executes the gcode in another file. The file 
        /// </summary>
        /// <param name="parameters"></param>
        public static List<string> ExecuteGCodeFile(string parameters)
        {
            var eachParam = parameters.Split(',');

            string fileToExecute = "";
            int executionCount = 1;

            if (eachParam.Length == 1)
            {
                fileToExecute = eachParam[0];
            }

            else if (eachParam.Length == 2)
            {
                if (!int.TryParse(eachParam[0], out executionCount)) executionCount = 1;
                fileToExecute = eachParam[1];
            }

            else
            {
                throw new Exception("Invalid number of parameters");
            }

            fileToExecute = fileToExecute.Trim();
            if (fileToExecute.Length == 0) throw new Exception("Invalid Filename");

            // If the specified file does not contain a folder, then search the folders specified in settings
            if (!fileToExecute.Contains(@"\"))
            {
                foreach (string folder in Machine.GetMachine().Settings.GCodeExecSearchPath.Split(';'))
                {
                    string trimmedFolder = folder.Trim();
                    if (trimmedFolder.Length == 0) continue;
                    if (!trimmedFolder.EndsWith(@"\")) trimmedFolder += @"\";
                    if (File.Exists(trimmedFolder + fileToExecute))
                    {
                        fileToExecute = trimmedFolder + fileToExecute;
                        break;
                    }
                }
            }

            if (!File.Exists(fileToExecute)) throw new Exception("File not found");

            var codeToExec = new List<string>();
            var allLines = File.ReadAllLines(fileToExecute);

            codeToExec.Add(string.Format("(EXECUTING '{0}'", fileToExecute));

            for (int i = 1; i <= executionCount; i++)
            {
                codeToExec.Add(string.Format("(BEGIN {0} of {1})", i, executionCount));
                codeToExec.AddRange(allLines);
                codeToExec.Add(string.Format("(END {0} of {1})", i, executionCount));
            }

            return codeToExec;
        }

        #endregion

        #region Private Static Methods

        private static double getHypotenuseOfTriangle(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }

        #endregion
    }
}
