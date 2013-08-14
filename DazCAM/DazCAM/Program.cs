using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.IO;
using System;

namespace DazCAM
{
    public class Program
    {
        #region Declarations

        private static Thread _jogModeThread;
        private static bool _errorFlashActive;

        #endregion

        #region Public Member Variables
        public static bool MotionInterrupt = false;
        public static bool EStopCondition = false;
        public static bool OvertravelCondition = false;
        public static bool Paused = false;
        public static bool IsHoming = false;

        public static Axis X;
        public static Axis Y;
        public static Axis Z;
        public static MotionController Controller;
        public static Devices ExternalDevices;

        public static OutputPort Led = new OutputPort(Pins.ONBOARD_LED, false);
        public static OutputPort ExternalLed = new OutputPort(Pins.GPIO_PIN_A0, false);   // Was D10

        public static int Resolution;
        public static OutputPort Ms1 = new OutputPort(Pins.GPIO_PIN_D0, false);
        public static OutputPort Ms2 = new OutputPort(Pins.GPIO_PIN_D1, false);                     // Stepper Driver Resolution pins

        public static OutputPort StepperEnable = new OutputPort(Pins.GPIO_PIN_D2, true);            // Low is enabled, High is disabled

        public static Servo ServoZTemp = new Servo(Pins.GPIO_PIN_D5);                               // Using a servo for a temporary Z axis to lift and lower a sharpie pen

        public static InterruptPort EStopButton = new InterruptPort(Pins.GPIO_PIN_A5, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);

        public static Queue CommandList = new Queue();

        public static Tacho FanTacho = new Tacho();

        #endregion

        #region Static Main

        public static void Main()
        {
            //FanTacho.Intialize(Cpu.Pin.GPIO_Pin13);//not working dont use

            X = new Axis("X", Pins.GPIO_PIN_D11, Pins.GPIO_PIN_D4, Pins.GPIO_PIN_D3);
            Y = new Axis("Y", Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D8, Pins.GPIO_PIN_A1);
            Z = new Axis("Z", Pins.GPIO_PIN_D7, Pins.GPIO_PIN_D6, Pins.GPIO_PIN_A2);

            ExternalDevices = new Devices(Pins.GPIO_PIN_A4, Pins.GPIO_PIN_A3, Pins.GPIO_NONE);

            X.OnAxisLimitReached += OnAxisLimitReached;
            Y.OnAxisLimitReached += OnAxisLimitReached;
            Z.OnAxisLimitReached += OnAxisLimitReached;

            EStopButton.OnInterrupt += EStopButton_OnPress;

            EnableSteppers(false);              // don't drive the stepper coils while idle
            SetStepResolution("2");             // half stepping by default
            ServoZTemp.Angle = 10;              // raise the sharpie on reboot

            Controller = new MotionController(X, Y, Z, ExternalDevices);

            var socketServer = new SocketServer();
            socketServer.OnRequest += SocketServer_OnRequest;

            BlinkLED(3, false);       // Visual "Ready" Cue

            // Boot up in E-Stop mode to require an initializing communication from the controller UI
            SetEStop();

            socketServer.ListenForRequest();
        }

        #endregion

        #region Private Static Methods

        private static string ExecuteCommand(string command, out bool error)
        {
            string requestCommand;
            string requestParams;

            SplitCommandAndParam(command, out requestCommand, out requestParams);
            requestCommand = requestCommand.Trim().ToUpper();

            error = false;

            if (EStopCondition)
            {
                if (requestCommand != "?" && requestCommand != "CLEARESTOP" && requestCommand != "SCAN")
                {
                    error = true;
                    return "***E-STOP: No commands allowed while E-Stop is active";
                }
            }

            switch (requestCommand)
            {
                case "PING": return ExecutePing(true);
                case "SCAN": return ExecuteScan(true);
                case "?": return ReportStatus();

                case "STEPRESOLUTION": return SetStepResolution(requestParams);
                case "AXISINVERT": return InvertAxis(requestParams, out error);
                case "STEPENABLE": return EnableSteppers(requestParams == "1");

                case "PATH": return ExecutePathLinear(requestParams, out error);
                case "GOTO": return ExecuteGotoNonLinear(requestParams, out error);

                case "ZSERVO": return SetZServoPosition(requestParams);
                case "ZSERVOTRANSITIONTO": return ZServoTransitionTo(requestParams);

                case "ADD": return AddToCommandList(requestParams);
                case "RUN": return RunCommandList(out error);
                case "CLEAR": return ClearCommandList();

                case "FILEADD": return AddToCommandFile(requestParams, out error);
                case "FILERUN": return RunCommandFile(out error);
                case "FILECLEAR": return ClearCommandFile();

                case "CLEARESTOP": return ClearEStop();
                case "FINDHOME": return FindHome(requestParams, out error);
                case "FINDEDGE": return FindEdge(requestParams, out error);

                case "JOG": return EnableJogMode(requestParams, out error);
                case "DWELL": return ExecuteDwell(requestParams);

                case "ZERO": return ZeroCurrentLocation();

                case "IGNORELIMITS": return SetIgnoreLimits(requestParams);

                default:
                    error = true;
                    return "Invalid Command";
            }
        }

        private static string SetIgnoreLimits(string requestParams)
        {
            bool ignoreLimits = requestParams.Trim() == "1";
            Controller.IgnoreLimitSwitches = ignoreLimits;

            return "Setting Ignore Limits to " + ignoreLimits.ToString();
        }

        private static string ExecuteDwell(string param)
        {
            int dwell = int.Parse(param);
            Thread.Sleep(dwell);
            return "Dwell complete";
        }

        private static string ZeroCurrentLocation()
        {
            X.ResetLocation();
            Y.ResetLocation();
            Z.ResetLocation();

            return "Current location set to zero \r\nLocation: 0, 0, 0";
        }

        private static string FindHome(string param, out bool error)
        {
            IsHoming = true;
            error = false;

            int speedRapid;
            int speedSlow;
            bool findX;
            bool findY;
            bool findZ;

            var parameters = param.Split(',');

            speedRapid = int.Parse(parameters[0]);
            speedSlow = int.Parse(parameters[1]);
            findX = parameters[2] == "1";
            findY = parameters[3] == "1";
            findZ = parameters[4] == "1";

            Controller.FindHome(speedRapid, speedSlow, findX, findY, findZ);

            IsHoming = false;

            if (EStopCondition)
            {
                error = true;
                return "***E-STOP Activated";
            }

            string xErrorFactor = "n/a";
            if (findX)
            {
                xErrorFactor = (-X.Location).ToString();
                X.ResetLocation();
            }

            string yErrorFactor = "n/a";
            if (findY)
            {
                yErrorFactor = (-Y.Location).ToString();
                Y.ResetLocation();
            }

            string zErrorFactor = "n/a";
            if (findZ)
            {
                zErrorFactor = (-Z.Location).ToString();
                Z.ResetLocation();
            }

            string errorFactor = xErrorFactor + ", " + yErrorFactor + ", " + zErrorFactor;

            return "Requested Axes set to home. Adjusted for the following Error Factor: " + errorFactor;
        }

        private static string FindEdge(string param, out bool error)
        {
            IsHoming = true;
            error = false;

            var parameters = param.Split(',');

            Axis axisToFindEdgeFor = GetAxis(parameters[0]);
            int speedSlow = int.Parse(parameters[1]);
            int backoffCount = int.Parse(parameters[2]);
            int backoffSpeed = int.Parse(parameters[3]);

            string returnValue = Controller.FindEdge(speedSlow, backoffCount, backoffSpeed, axisToFindEdgeFor);

            IsHoming = false;

            if (EStopCondition)
            {
                error = true;
                return "***E-STOP Activated";
            }

            string message = "Edge found for " + axisToFindEdgeFor.ID + "\r\n";
            message += returnValue + "\r\n";
            message += "Location: " + X.Location.ToString() + ", " + Y.Location.ToString() + ", " + Z.Location.ToString();

            return message;
        }

        private static void SetEStop(bool forOvertravel = false)
        {
            if (!EStopCondition)
            {
                EnableSteppers(false);
                MotionInterrupt = true;
                EStopCondition = true;
                if (forOvertravel) OvertravelCondition = true;

                ExternalDevices.ShutdownAll();
                CommandList.Clear();

                EnableErrorFlash(true);
            }
        }

        private static void EnableErrorFlash(bool enable)
        {
            if (!_errorFlashActive)
            {
                var threadStart = new ThreadStart(ErrorFlash);
                var flashThread = new Thread(threadStart);
                _errorFlashActive = true;
                flashThread.Start();
            }

            else
            {
                _errorFlashActive = false;
            }
        }

        private static void ErrorFlash()
        {
            while (_errorFlashActive)
            {
                BlinkLED(5, false, true);
                Thread.Sleep(500);
            }
        }

        private static string ClearEStop()
        {
            if (EStopCondition)
            {
                X.ResetLimitReached();
                Y.ResetLimitReached();
                Z.ResetLimitReached();

                if (X.IsAtLimit() || Y.IsAtLimit() || Z.IsAtLimit())
                {
                    //TODO: This should set an error condition to return with
                    //      AND should also only work for unknown limits, but we can't support that until we have
                    //      code to auto step the axis off the limit (which may never happen)

                    return "Cannot clear E-Stop while an axis is over-travelled";
                }

                MotionInterrupt = false;
                EStopCondition = false;
                OvertravelCondition = false;
                EnableErrorFlash(false);
                return "E-Stop Cleared";
            }

            return "E-Stop condition is not active";
        }

        private static string ClearCommandList()
        {
            CommandList.Clear();
            return "Cleared Command List";
        }

        private static void SplitCommandAndParam(string requestText, out string requestCommand, out string requestParam)
        {
            int separator = requestText.IndexOf(':');

            if (separator == -1)
            {
                requestCommand = requestText;
                requestParam = "";
            }
            else
            {
                requestCommand = requestText.Substring(0, separator);
                requestParam = requestText.Substring(separator + 1);
            }
        }

        private static string RunCommandList(out bool error)
        {
            string responseMessage = "";

            error = false;

            while (CommandList.Count > 0)
            {
                string cmd = (string)CommandList.Dequeue();
                responseMessage = ExecuteCommand(cmd, out error);

                if (error) return responseMessage;
            }

            return "Command List Finished";
        }

        private static string AddToCommandList(string param)
        {
            int count = 0;
            string toAdd;
            foreach (string cmd in param.Split('/'))
            {
                toAdd = cmd.Trim();
                if (toAdd.Length > 0)
                {
                    CommandList.Enqueue(toAdd);
                    count++;
                }
            }

            string msg = "";
            msg += "Command(s) added: " + CommandList.Count.ToString() + "\r\n";
            msg += "Total in Queue: " + CommandList.Count.ToString();
            return msg;
        }

        private static string ClearCommandFile()
        {
            try
            {
                File.Delete(@"\SD\Commands.txt");
            }
            catch { }
            return "Command file cleared";
        }

        private static string AddToCommandFile(string param, out bool error)
        {
            try
            {
                using (var file = new StreamWriter(@"\SD\Commands.txt", true))
                {
                    file.WriteLine(param);
                }
            }

            catch (Exception ex)
            {
                error = true;
                return ex.Message;
            }

            error = false;
            return "Commands added to file";
        }

        private static string RunCommandFile(out bool error)
        {
            using (var reader = new StreamReader(@"\SD\Commands.txt"))
            {
                while (true)
                {
                    string commandBlock = "";
                    try
                    {
                        commandBlock = reader.ReadLine();
                        if (commandBlock.Length == 0) break;
                    }
                    catch {
 
                    }

                    AddToCommandList(commandBlock);
                    string message = RunCommandList(out error);
                    if (error) return message;
                }
            }

            ClearCommandFile();

            error = false;
            return "Command File Finished";
        }

        private static string SetZServoPosition(string param)
        {
            uint angle = uint.Parse(param);
            ServoZTemp.Angle = angle;
            return "Setting Z Servo to " + angle.ToString();
        }

        /// <summary>
        /// Example Usage: ZServoTransitionTo:110,2   - adjusts the angle to 110, waiting 2ms between each degree
        /// </summary>
        private static string ZServoTransitionTo(string param)
        {
            uint newAngle;
            uint currentAngle = ServoZTemp.Angle;
            int mSecStepDelay;

            var parsed = param.Split(',');
            newAngle = uint.Parse(parsed[0]);
            mSecStepDelay = int.Parse(parsed[1]);

            while (currentAngle != newAngle)
            {
                if (currentAngle < newAngle) currentAngle++;
                else currentAngle--;

                ServoZTemp.Angle = currentAngle;

                Thread.Sleep(mSecStepDelay);
            }

            return "Moved Z Servo to " + newAngle.ToString();
        }

        /// <summary>
        /// Blinks the LED 3 times
        /// </summary>
        private static string ExecutePing(bool useExternalLED = false)
        {
            BlinkLED(3, useExternalLED);
            return "Pong!";
        }

        /// <summary>
        /// Blinks the LED 3 times
        /// </summary>
        private static string ExecuteScan(bool useExternalLED = false)
        {
            BlinkLED(3, useExternalLED);
            return "IP-" + Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress;
        }

        private static void BlinkLED(int blinkCount, bool useExternalLED = false, bool fast = false)
        {
            var pin = useExternalLED ? ExternalLed : Led;
            int delay = fast ? 50 : 250;

            for (int i = 0; i < blinkCount; i++)
            {
                pin.Write(true);
                Thread.Sleep(delay);
                pin.Write(false);
                Thread.Sleep(delay);
            }
        }

        private static string ReportStatus()
        {
            string cr = "\r\n";
            string status = "Status" + cr;

            status += "Location: " + X.Location.ToString() + ", " + Y.Location.ToString() + ", " + Z.Location.ToString() + cr;
            status += "Commands in Queue: " + CommandList.Count.ToString() + cr;
            status += "Free Memory: " + Debug.GC(true) + cr;
            status += cr;

            status += "Resolution: " + Resolution.ToString() + cr;
            status += "X-Axis Inversion: " + X.InvertDirectionPinOutput.ToString() + cr;
            status += "Y-Axis Inversion: " + Y.InvertDirectionPinOutput.ToString() + cr;
            status += "Z-Axis Inversion: " + Z.InvertDirectionPinOutput.ToString() + cr;
            status += cr;

            status += "E-Stop: " + EStopCondition.ToString() + cr;
            status += "Overtravel: " + OvertravelCondition.ToString() + cr;
            status += "Motion Interrupt: " + MotionInterrupt.ToString() + cr;
            status += "Jog Mode: " + Controller.JogMode.ToString() + cr;
            status += "Ignoring Limit Switches: " + Controller.IgnoreLimitSwitches.ToString() + cr;
            status += cr;

            string whichSwitch;
            status += "X-Limit Switch Tripped: " + X.IsAtLimit(out whichSwitch) + " - " + whichSwitch + cr;
            status += "Y-Limit Switch Tripped: " + Y.IsAtLimit(out whichSwitch) + " - " + whichSwitch + cr;
            status += "Z-Limit Switch Tripped: " + Z.IsAtLimit(out whichSwitch) + " - " + whichSwitch + cr;
            status += cr;

            status += "Fan RPM: " + FanTacho.GetRpm() + cr;

            if (EStopCondition)
            {
                status += "***E-STOP: Active" + cr;
            }

            return status;
        }

        /// <summary>
        /// Sets the stepper resolution using a param value of 1:Full 2:Half 4:Quarter 8:Eigth
        /// </summary>
        private static string SetStepResolution(string param)
        {
            string message = "";

            if (param == "8")
            {
                SetStepperResolutionBits(1, 1);
                Resolution = 8;
                message = "Eighth Step (1,1)";
            }

            else if (param == "4")
            {
                SetStepperResolutionBits(0, 1);
                Resolution = 4;
                message = "Quarter Step (0,1)";
            }

            else if (param == "2")
            {
                SetStepperResolutionBits(1, 0);
                Resolution = 2;
                message = "Half Step (1,0)";
            }

            else
            {
                SetStepperResolutionBits(0, 0);
                Resolution = 1;
                message = "Full Step (0,0)";
            }

            // Location is always based on microstepping at one/eigth resolution, so that we can always report our current location 
            // regardless of resolution changes.
            X.LocationUnitsPerStep = 8 / Resolution;
            Y.LocationUnitsPerStep = 8 / Resolution;
            Z.LocationUnitsPerStep = 8 / Resolution;

            return "Setting resolution to " + message;
        }

        private static void SetStepperResolutionBits(int ms1, int ms2)
        {
            Ms1.Write(ms1 == 1);
            Ms2.Write(ms2 == 1);
        }

        /// <summary>
        /// Enables or Disables the output of the A4988 Stepper Drivers. When they are enabled, the coils will be energized with current flowing, holding the steppers
        /// from turning. When they are disabled there will be no current flowing through the coils, and they can be manually rotated. 
        /// </summary>
        private static string EnableSteppers(bool enable)
        {
            // The A4988 Enable pin is inverted. 1=Disabled, 0=Enabled
            StepperEnable.Write(!enable);
            return (enable ? "Enabling" : "Disabling") + " stepper output";
        }

        private static string InvertAxis(string param, out bool error)
        {
            if (param.Length != 2)
            {
                error = true;
                return "Invalid param, must supply as [X|Y|Z][0|1] - eg: AxisInvert:X1 ";
            }

            var axis = GetAxis(param.Substring(0, 1));

            if (axis == null)
            {
                error = true;
                return "Invalid Axis. Valid values = X, Y or Z";
            }

            axis.InvertDirectionPinOutput = param.Substring(1, 1) == "1" ? true : false;

            // Done intentionally to bypass the optimization that supresses a false change in direction.
            axis.StepDirection = !axis.StepDirection;
            axis.StepDirection = !axis.StepDirection;

            error = false;
            return "Setting axis inversion: " + param;
        }

        private static Axis GetAxis(string axis)
        {
            switch (axis.ToUpper())
            {
                case "X": return X;
                case "Y": return Y;
                case "Z": return Z;
                default: return null;
            }
        }

        private static string EnableJogMode(string param, out bool error)
        {
            error = false;
            MotionController.JogModeType modeToSet;

            switch (param)
            {
                case "1": modeToSet = MotionController.JogModeType.XY; break;
                case "2": modeToSet = MotionController.JogModeType.XZ; break;
                case "3": modeToSet = MotionController.JogModeType.YZ; break;
                default: modeToSet = MotionController.JogModeType.None; break;
            }

            // Start Jog Mode
            if (Controller.JogMode == MotionController.JogModeType.None && modeToSet != MotionController.JogModeType.None)
            {
                Controller.JogMode = modeToSet;

                EnableSteppers(true);

                _jogModeThread = new Thread(new ThreadStart(Controller.StartJogMode));
                _jogModeThread.Start();

                return "Jog mode Enabled: " + modeToSet.ToString();
            }

            // Changing Jog Mode
            else if (Controller.JogMode != MotionController.JogModeType.None && modeToSet != MotionController.JogModeType.None)
            {
                Controller.JogMode = modeToSet;
                return "Jog mode changed: " + modeToSet.ToString();
            }

            // End Jog Mode
            else if (Controller.JogMode != MotionController.JogModeType.None && modeToSet == MotionController.JogModeType.None)
            {
                Controller.JogMode = MotionController.JogModeType.None;
                _jogModeThread.Join();
                _jogModeThread = null;

                // Return new location
                string msg = "Jog mode Disabled\r\n";
                msg += "Location: " + X.Location.ToString() + ", " + Y.Location.ToString() + ", " + Z.Location.ToString();

                EnableSteppers(false);

                return msg;
            }

            // Attempt to disable jog mode when its already disabled
            else
            {
                error = true;
                return "Jog Mode is not currently active";
            }
        }

        /// <summary>
        /// Drives the Axes in a non-coordinated movement using the supplied number of relative steps. The resulting
        /// path will not be a straight line, all axes are driven until they reach their destination independantly at the specified speed (using a delay param).
        /// This would generally be used as a rapid traverse as it is faster for all steppers to run at full speed than
        /// to run at full speed along a calculated line. The param format is x,y,z,d where x,y,z are relative steps and s is the delay in milliseconds between steps
        /// </summary>
        private static string ExecuteGotoNonLinear(string param, out bool error)
        {
            var values = param.Split(',');

            int x = int.Parse(values[0]);
            int y = int.Parse(values[1]);
            int z = int.Parse(values[2]);
            int delay = int.Parse(values[3]);

            string errorMessage;
            Controller.GotoNonLinear(x, y, z, delay, out errorMessage);

            if (errorMessage.Length > 0)
            {
                error = true;
                return errorMessage;
            }

            error = false;
            return "Goto complete";
        }

        /// <summary>
        /// Drives the Axes in a linear path, interpolating X, Y and Z to form a continuous straight line. The param
        /// format is x,y,z,d where x,y,z are relative steps and d is the delay in milliseconds between steps
        /// </summary>
        private static string ExecutePathLinear(string param, out bool error)
        {
            var values = param.Split(',');

            int x = int.Parse(values[0]);
            int y = int.Parse(values[1]);
            int z = int.Parse(values[2]);
            int delay = int.Parse(values[3]);

            string errorMessage;
            Controller.PathToLinear(x, y, z, delay, out errorMessage);

            if (errorMessage.Length > 0)
            {
                error = true;
                return errorMessage;
            }

            error = false;
            return "Path complete";
        }

        #endregion

        #region Events

        private static void SocketServer_OnRequest(object sender, SocketServer.RequestHandlerEventArgs e)
        {
            bool responseError = false;

            e.ResponseMessage = ExecuteCommand(e.RequestCommand, out responseError);
            e.ResponseError = responseError;
        }

        private static void OnAxisLimitReached(Axis sender)
        {
            if (!IsHoming) SetEStop(true);
        }

        static void EStopButton_OnPress(uint data1, uint data2, System.DateTime time)
        {
            if (data2 == 0) SetEStop();
        }

        #endregion
    }
}
