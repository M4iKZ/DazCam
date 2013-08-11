using System.Threading;

namespace DazCAM
{
    /// <summary>
    /// 3 Axis (X, Y, Z) Stepper Axis synchronizer class for abstracting circuitry required to drive 3 stepper 
    /// motors from a NetDuino 
    /// </summary>
    public class MotionController
    {
        #region Declarations

        public enum JogModeType { None, XY, XZ, YZ }

        private bool _ignoreLimitSwitches = false;

        #endregion
        
        #region Properties

        public Axis AxisX { get; private set; }
        public Axis AxisY { get; private set; }
        public Axis AxisZ { get; private set; }
        public Devices ExternalDevices { get; private set; }

        public JogModeType JogMode { get; set; }

        /// <summary>
        /// Set this property to True while performing moves if the limit switches should be ignored. This was created because on my machine, a voltage spike was
        /// causing a false hit on the limit switches and interrupting the program when I would turn on or off the router or dust vacuum for example.
        /// </summary>
        public bool IgnoreLimitSwitches
        {
            get
            {
                return _ignoreLimitSwitches;
            }
            set
            {
                _ignoreLimitSwitches = value;
                AxisX.IgnoreLimit = _ignoreLimitSwitches;
                AxisY.IgnoreLimit = _ignoreLimitSwitches;
                AxisZ.IgnoreLimit = _ignoreLimitSwitches;
            }
        }

        #endregion

        #region Constructors

        public MotionController(Axis axisX, Axis axisY, Axis axisZ, Devices externalDevices)
        {
            //TODO: Eliminate all references to Program.x - make sure all dependancies are injected
            AxisX = axisX;
            AxisY = axisY;
            AxisZ = axisZ;
            ExternalDevices = externalDevices;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Finds the minimum limit switches for all 3 Axes
        /// </summary>
        public void FindHome(int delayRapid, int delaySlow, bool findX, bool findY, bool findZ)
        {
            bool wasIgnoringLimits = IgnoreLimitSwitches;
            IgnoreLimitSwitches = false;

            AxisX.StepDirection = false;
            AxisY.StepDirection = false;
            AxisZ.StepDirection = false;

            bool xSeeking = findX;
            bool ySeeking = findY;
            bool zSeeking = findZ;

            // --- STEP 1 - Rapid to home (All minimum limit switches)
            while (xSeeking || ySeeking || zSeeking)
            {
                if (Program.EStopCondition) return;

                xSeeking = xSeeking && AxisX.LimitReached != Axis.LimitSwitch.Min && AxisX.LimitReached != Axis.LimitSwitch.Unknown;
                ySeeking = ySeeking && AxisY.LimitReached != Axis.LimitSwitch.Min && AxisY.LimitReached != Axis.LimitSwitch.Unknown;
                zSeeking = zSeeking && AxisZ.LimitReached != Axis.LimitSwitch.Min && AxisZ.LimitReached != Axis.LimitSwitch.Unknown;

                if (xSeeking) AxisX.Step();
                if (ySeeking) AxisY.Step();
                if (zSeeking) AxisZ.Step();

                DelayBy(delayRapid);
            }

            // --- STEP 2 - After a pause, slowly move each axis back off until the switches are opened
            AxisX.StepDirection = true;
            AxisY.StepDirection = true;
            AxisZ.StepDirection = true;

            xSeeking = findX;
            ySeeking = findY;
            zSeeking = findZ;

            Thread.Sleep(500);

            while (xSeeking || ySeeking || zSeeking)
            {
                if (Program.EStopCondition) return;

                xSeeking = xSeeking && !AxisX.ResetLimitReached();
                ySeeking = ySeeking && !AxisY.ResetLimitReached();
                zSeeking = zSeeking && !AxisZ.ResetLimitReached();

                if (xSeeking) AxisX.Step();
                if (ySeeking) AxisY.Step();
                if (zSeeking) AxisZ.Step();

                DelayBy(delaySlow);
            }

            IgnoreLimitSwitches = wasIgnoringLimits;
        }

        public string FindEdge(int delaySlow, int backoffCount, int backoffDelay, Axis axisToFindEdgeFor)
        {
            bool wasIgnoringLimits = IgnoreLimitSwitches;
            IgnoreLimitSwitches = false;

            // X and Y move towards home to find their edges, Z moves away from home (towards bed)
            axisToFindEdgeFor.StepDirection = axisToFindEdgeFor.ID == "Z" ? true : false;

            while (true)
            {
                if (Program.EStopCondition) return "";

                // Use Z Limit pin regardless of axis we're finding the edge for 
                if (AxisZ.IsAtLimit()) break;
                axisToFindEdgeFor.Step();

                DelayBy(delaySlow);
            }

            int edgeFoundAt = axisToFindEdgeFor.Location;
            axisToFindEdgeFor.StepDirection = !axisToFindEdgeFor.StepDirection;

            for (int i = 0; i < backoffCount; i++)
            {
                axisToFindEdgeFor.Step();
                DelayBy(backoffDelay);
            }

            AxisZ.ResetLimitReached();

            IgnoreLimitSwitches = wasIgnoringLimits;
            return "Edge touched at: " + edgeFoundAt.ToString();
        }

        /// <summary>
        /// Destination coordinates are relative microsteps. delay is supplied as milliseconds to delay between steps.
        /// </summary>
        public void GotoNonLinear(int destinationX, int destinationY, int destinationZ, int delay, out string errorMessage)
        {
            errorMessage = "";

            int x = 0;
            int y = 0;
            int z = 0;

            int stepX;
            int stepY;
            int stepZ;

            // Set X Direction
            if (destinationX > 0)
            {
                stepX = 1;
                AxisX.StepDirection = true;
            }
            else
            {
                stepX = -1;
                AxisX.StepDirection = false;
            }

            // Set Y Direction
            if (destinationY > 0)
            {
                stepY = 1;
                AxisY.StepDirection = true;
            }
            else
            {
                stepY = -1;
                AxisY.StepDirection = false;
            }

            // Set Z Direction
            if (destinationZ > 0)
            {
                stepZ = 1;
                AxisZ.StepDirection = true;
            }
            else
            {
                stepZ = -1;
                AxisZ.StepDirection = false;
            }

            while (x != destinationX || y != destinationY || z != destinationZ)
            {
                if (Program.MotionInterrupt)
                {
                    if (!Program.Paused)
                    {
                        errorMessage = GetEStopErrorMessage();
                        return;
                    }
                }

                // Step X?
                if (x != destinationX)
                {
                    x += stepX;
                    AxisX.Step();
                }

                // Step Y?
                if (y != destinationY)
                {
                    y += stepY;
                    AxisY.Step();
                }

                // Step Z?
                if (z != destinationZ)
                {
                    z += stepZ;
                    AxisZ.Step();
                }
                DelayBy(delay);
            }
        }

        private void DelayBy(int delay)
        {
            if (delay > 0)
            {
                int delayCountdown = delay;
                while (delayCountdown > 0) delayCountdown--;
            }
        }

        /// <summary>
        /// Uses the Bresenham's Algorithm to coordinate 3 axis of stepper movements to create a straight 3D line segment, using
        /// the supplied Destination coordinates relative to a start vector of <0,0,0>. delay is the number of milliseconds between step pulses.
        /// </summary>
        public void PathToLinear(int destinationX, int destinationY, int destinationZ, int delay, out string errorMessage)
        {
            errorMessage = "";

            int x = 0;
            int y = 0;
            int z = 0;

            int deltaX = System.Math.Abs(destinationX);
            int deltaY = System.Math.Abs(destinationY);
            int deltaZ = System.Math.Abs(destinationZ);

            int stepX;
            int stepY;
            int stepZ;

            // Set X Direction
            if (destinationX > 0)
            {
                stepX = 1;
                AxisX.StepDirection = true;
            }
            else
            {
                stepX = -1;
                AxisX.StepDirection = false;
            }

            // Set Y Direction
            if (destinationY > 0)
            {
                stepY = 1;
                AxisY.StepDirection = true;
            }
            else
            {
                stepY = -1;
                AxisY.StepDirection = false;
            }

            // Set Z Direction
            if (destinationZ > 0)
            {
                stepZ = 1;
                AxisZ.StepDirection = true;
            }
            else
            {
                stepZ = -1;
                AxisZ.StepDirection = false;
            }

            // Keep track of the progressive error that accumulates with each step on the major axis to adjust along the way
            int errorXY = deltaX - deltaY;
            int errorXZ = deltaX - deltaZ;
            int errorYZ = deltaY - deltaZ;

            while (x != destinationX || y != destinationY || z != destinationZ)
            {
                if (Program.MotionInterrupt)
                {
                    if (!Program.Paused)
                    {
                        errorMessage = GetEStopErrorMessage();
                        return;
                    }
                }

                int offsetXY = errorXY * 2;
                int offsetXZ = errorXZ * 2;
                int offsetYZ = errorYZ * 2;

                // Step X?
                if (offsetXY > -deltaY && offsetXZ > -deltaZ)
                {
                    errorXY -= deltaY;
                    errorXZ -= deltaZ;
                    x += stepX;
                    AxisX.Step();
                }

                // Step Y?
                if (offsetXY < deltaX && offsetYZ > -deltaZ)
                {
                    errorXY += deltaX;
                    errorYZ -= deltaZ;
                    y += stepY;
                    AxisY.Step();
                }

                // Step Z?
                if (offsetXZ < deltaX && offsetYZ < deltaY)
                {
                    errorXZ += deltaX;
                    errorYZ += deltaY;
                    z += stepZ;
                    AxisZ.Step();
                }

                DelayBy(delay);
            }
        }

        public void StartJogMode()
        {
            double jogX;
            double jogXAmount;
            double jogXDelays = 0;

            double jogY;
            double jogYAmount;
            double jogYDelays = 0;

            while (this.JogMode != JogModeType.None)
            {
                if (jogXDelays == 0)
                {
                    jogX = ExternalDevices.JogXPort.Read();

                    jogXAmount = System.Math.Abs(jogX);
                    if (jogXAmount < 8) jogXAmount = 0;
                    if (jogXAmount > 40) jogXAmount = 50;
                    jogXDelays = 51 - jogXAmount;

                    if (jogXAmount > 0)
                    {
                        Axis axis = AxisX;
                        if (JogMode == JogModeType.YZ) axis = AxisZ;            // Z moves instead of X
                        
                        axis.StepDirection = jogX > 0;
                        axis.Step();
                     }
                }

                jogXDelays--;

                if (jogYDelays == 0)
                {
                    jogY = -ExternalDevices.JogYPort.Read();

                    jogYAmount = System.Math.Abs(jogY);
                    if (jogYAmount < 8) jogYAmount = 0;
                    if (jogYAmount > 40) jogYAmount = 50;
                    jogYDelays = 51 - jogYAmount;

                    if (jogYAmount > 0)
                    {
                        Axis axis = AxisY;
                        if (JogMode == JogModeType.XZ) axis = AxisZ;            // Z moves instead of Y

                        axis.StepDirection = jogY > 0;
                        axis.Step();
                    }
                }

                jogYDelays--;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Evaluates the stop condition, checking for EStop or a limit switch. Turns off all devices and sets an error message to be returned.
        /// </summary>
        private string GetEStopErrorMessage()
        {
            string limitSwitch;

            if (Program.OvertravelCondition)
            {
                if (AxisX.IsAtLimit(out limitSwitch)) return "***E-STOP Activated: X Axis (" + limitSwitch + ") Limit reached";
                if (AxisY.IsAtLimit(out limitSwitch)) return "***E-STOP Activated: Y Axis (" + limitSwitch + ") Limit reached";
                if (AxisZ.IsAtLimit(out limitSwitch)) return "***E-STOP Activated: Z Axis (" + limitSwitch + ") Limit reached";
            }

            if (Program.EStopCondition)
            {
                return "***E-STOP Activated";
            }

            return "Unknown reason for motion stop condition";
        }

        #endregion
    }
}
