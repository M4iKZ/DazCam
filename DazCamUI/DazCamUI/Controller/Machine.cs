using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace DazCamUI.Controller
{
    public class Machine
    {
        #region Declarations

        private static Machine _instance;           // Our singleton instance
        private Queue<string> _commandQueue;

        private FeedRate.StepResolutions _resolution = FeedRate.StepResolutions.Full;
        private AxisSettings _axisFindingEdgeFor = null;

        public enum JogModeTypes { None, XY, XZ, YZ };

        #endregion

        #region Properties

        public bool EStopActive { get; private set; }

        public ControllerSettings Settings { get; private set; }
        public Coordinate Location { get; private set; }

        public int ZServoAngle { get; private set; }

        public bool EnqueueCommands { get; set; }

        public Offset WorkingOffset { get; set; }

        #endregion

        #region Event Declarations

        public delegate void OnCommandSendingEventHandler(string commandToBeSent);
        public event OnCommandSendingEventHandler OnCommandSending;

        public delegate void OnCommandResponseEventHandler(bool isError, string responseText);
        public event OnCommandResponseEventHandler OnCommandResponse;

        public delegate void OnLocationChangedEventHandler(Coordinate machineLocation, int zServoAngle);
        public event OnLocationChangedEventHandler OnLocationChanged;

        public delegate void OnEStopNotificationEventHandler(bool eStopActive);
        public event OnEStopNotificationEventHandler OnEStopNotification;

        public delegate void OnEdgeTouchedEventHandler(double touchedAt);
        public event OnEdgeTouchedEventHandler OnEdgeTouched;

        #endregion

        #region Constructors

        private Machine(string pathToSettings)
        {
            Settings = ControllerSettings.Load(pathToSettings);
            _commandQueue = new Queue<string>();

            Location = new Coordinate(0, 0, 0);
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Instantiates a singelton instance of the Machine Class, loading the saved settings from the file specified. This must be the first call to this class.
        /// Subsequent calls can be then made on the instance returned from the GetMachine() static method.
        /// </summary>
        /// <param name="pathToSettings"></param>
        /// <returns></returns>
        public static Machine Initialize(string pathToSettings)
        {
            if (_instance != null) throw new Exception("Machine Instance has already been initialized");

            _instance = new Machine(pathToSettings);
            return _instance;
        }

        /// <summary>
        /// Returns a singelton instance of the Machine Class. A call to the Initialize() method must first be made to load the settings. 
        /// </summary>
        /// <returns></returns>
        public static Machine GetMachine()
        {
            if (_instance == null) throw new Exception("Machine Instance has not yet been initialized with a call to .Initialize()");
            return _instance;
        }

        #endregion

        #region Public Methods

        public Coordinate GetWorkingLocation()
        {
            var machineLocation = Location;
            if (WorkingOffset != null) machineLocation = WorkingOffset.ConvertCoordinateToWorking(machineLocation);
            return machineLocation;
        }

        public void Ping()
        {
            SendCommand("Ping", 3);
        }

        public string Scan(string ipToScan, int start = 0, int end = 255)
        {
            //for (int i = start; i < end; i++)
            //{
            //    var ipAddress = SendCommand("Scan", 1, ipToScan + i);
            //    if (ipAddress != "")
            //        return ipAddress;
            //}
            return "0.0.0.0";
        }

        public void RequestStatus()
        {
            SendCommand("?", 3);
        }

        public void ZeroCurrentPosition()
        {
            SendCommand("ZERO", 3);
        }

        public void FindEdge(string axis)
        {
            //NOTE: As we are obviously hard coding the resolution, the speed calculations wouldn't work if we ever changed
            //      resolution or if the speed calculations returned a different resolution. I don't forsee doing this, but just be careful in case.
            SendStepResolution(FeedRate.StepResolutions.Half);

            _axisFindingEdgeFor = Settings.GetAxisSettings(axis);

            FeedRate.StepResolutions resolution;
            int slowSpeed = Settings.CalculateStepDelayFromFeedRate(20, out resolution);

            // Back off of the finder by 1/4 inch (maybe this should be a setting too?)
            int backoffSpeed = Settings.CalculateStepDelayFromFeedRate(30, out resolution);
            int backoffSteps = _axisFindingEdgeFor.FullStepsPerInch() * (int)_resolution / 4;

            EnableStepperDrivers(true);
            SendCommand(string.Format("FINDEDGE:{0},{1},{2},{3}", axis, slowSpeed, backoffSteps, backoffSpeed));
            EnableStepperDrivers(false);
        }

        public void FindHome(bool xAxis = true, bool yAxis = true, bool zAxis = true)
        {
            //NOTE: As we are obviously hard coding the resolution, the speed calculations wouldn't work if we ever changed
            //      resolution or if the speed calculations returned a different resolution. I don't forsee doing this, but just be careful in case.
            SendStepResolution(FeedRate.StepResolutions.Half);

            FeedRate.StepResolutions resolution;
            int stepDelayRapid = Settings.CalculateStepDelayFromFeedRate(Settings.FeedRateMaximum, out resolution);
            int stepDelaySlow = Settings.CalculateStepDelayFromFeedRate(10, out resolution);
            
            string axisFlags = string.Format("{0},{1},{2}", xAxis ? 1 : 0, yAxis ? 1 : 0, zAxis ? 1 : 0);
            string command = string.Format("FindHome:{0},{1},{2}", stepDelayRapid, stepDelaySlow, axisFlags);

            EnableStepperDrivers(true);
            SendCommand(command);
            EnableStepperDrivers(false);

            ZeroCurrentPosition();
        }

        /// <summary>
        /// Not only clears any E-Stop condition but also initializes the machine
        /// </summary>
        public void ClearEStop()
        {
            // Assume that this works. If it doesn't, the machine will tell us and the state will revert
            ActivateEStop(false);

            SendCommand("ClearEStop");
            InitializeHardware();
        }

        public void EnableStepperDrivers(bool enable)
        {
            SendCommand("StepEnable:" + (enable ? "1" : "0"));
        }

        public void IgnoreLimitSwitches(bool ignore)
        {
            SendCommand("IgnoreLimits:" + (ignore? "1" : "0"));
        }

        public void RunQueuedCommands()
        {
            if (_commandQueue == null) return;

            int countInPacket = 0;
            var sendBuffer = new Queue<string>();
            string toSend = "";

            SendCommand("FileClear");

            // Phase 1: Build up a Queue of packets of commands
            while (_commandQueue.Count > 0)
            {
                if (toSend.Length > 0) toSend += "/";
                toSend += _commandQueue.Dequeue();
                countInPacket++;

                // Don't overfeed the Netduino's memory capacity, only send packets with a limitied qty of commands
                // TODO: This should be a setting somewhere - although serial / asynch Coms may eliminate need for this
                if (countInPacket == 80 || _commandQueue.Count == 0)
                {
                    sendBuffer.Enqueue("FileAdd:" + toSend);
                    toSend = "";
                    countInPacket = 0;
                }
            }

            // Phase 2: Send each packet of commands and allow Netduino to run them, one at a time
            while (sendBuffer.Count > 0)
            {
                if (EStopActive) break;
                SendCommand(sendBuffer.Dequeue());
            }

            SendCommand("FileRun");
        }

        /// <summary>
        /// Sends commands to move the Axes to the specified coordinates
        /// </summary>
        /// <param name="destinationX">Absolute or Relative destination coordinate in Inches</param>
        /// <param name="destinationY">Absolute or Relative destination coordinate in Inches</param>
        /// <param name="destinationZ">Absolute or Relative destination coordinate in Inches</param>
        /// <param name="relative">If True, the move is relative to the current position, otherwise it's the absolute location on the bed</param>
        /// <param name="linear">If true, a straight line is used, otherwise the Axes will be driven until they individually reach the destination</param>
        /// <param name="feedRate">Speed to travel in Inches per Minute (IPM) - if exceeding the defined minimum or maximums, the value will be modified appropriately</param>
        public void MoveTo(double destinationX, double destinationY, double destinationZ, bool relative = true, bool linear = true, int feedRate = 5000)
        {
            var destination = new Coordinate(destinationX, destinationY, destinationZ);
            MoveTo(destination, relative, linear, feedRate);
        }

        /// <summary>
        /// Sends commands to move the Axes to the specified coordinates
        /// </summary>
        /// <param name="destination">Absolute or Relative destination coordinates in Inches</param>
        /// <param name="relative">If True, the move is relative to the current position, otherwise it's the absolute location on the bed</param>
        /// <param name="linear">If true, a straight line is used, otherwise the Axes will be driven until they individually reach the destination</param>
        /// <param name="feedRate">Speed to travel in Inches per Minute (IPM) - if exceeding the defined minimum or maximums, the value will be modified appropriately</param>
        public void MoveTo(Coordinate destination, bool relative = true, bool linear = true, int feedRate = 5000)
        {
            Coordinate relativeDestination = destination.Copy();

            // If absolute, convert to relative using current known location
            if (!relative)
            {
                if (WorkingOffset != null) relativeDestination = WorkingOffset.ConvertToMachineCoordinate(relativeDestination);
                relativeDestination.X -= Location.X;
                relativeDestination.Y -= Location.Y;
                relativeDestination.Z -= Location.Z;
            }

            //TODO: Need to vet against table min/max values here and throw exception if move would force an overtravel
            //      (or not)

            if (relativeDestination.X == 0 && relativeDestination.Y == 0 && relativeDestination.Z == 0) return;

            // Speed calculations
            FeedRate.StepResolutions resolution;
            int stepDelay;

            if (feedRate < 0)
            {
                // For calculating IPM, we can send in a negative feedrate to manually bypass the IPM system, and measure the speed of a
                // given Resolution and StepDelay
                int res = (int)Math.Floor(Math.Abs(feedRate) / 1000f);
                stepDelay = Math.Abs(feedRate) - (res * 1000);

                resolution = (FeedRate.StepResolutions)res;
            }
            else
            {
                stepDelay = Settings.CalculateStepDelayFromFeedRate(feedRate, out resolution);
            }

            if (resolution != _resolution) SendStepResolution(resolution);

            int stepsX = (int)Math.Round(relativeDestination.X * (double)Settings.XAxis.FullStepsPerInch() * (int)resolution);
            int stepsY = (int)Math.Round(relativeDestination.Y * (double)Settings.YAxis.FullStepsPerInch() * (int)resolution);
            int stepsZ = (int)Math.Round(relativeDestination.Z * (double)Settings.ZAxis.FullStepsPerInch() * (int)resolution);

            //TODO: Consolidate these into single command with linear/non-linear param
            string pathCmd = "path:{0},{1},{2},{3}";
            if (!linear) pathCmd = "goto:{0},{1},{2},{3}";

            pathCmd = string.Format(pathCmd, stepsX, stepsY, stepsZ, stepDelay);

            SendCommand(pathCmd);

            Location.X += relativeDestination.X;
            Location.Y += relativeDestination.Y;
            Location.Z += relativeDestination.Z;

            RaiseOnLocationChangedEvent();
        }

        /// <summary>
        /// Changes the Z-Servo position to the specified angle.
        /// </summary>
        /// <param name="angle">The angle to move the servo to, between 0 and 180</param>
        /// <param name="slowMove">If true, the servo will move slow, otherwise it will move at full speed to the target angle</param>
        public void MoveZServo(int angle, bool slowMove = false)
        {
            if (angle < 0 || angle > 180) return;

            string command = "ZServo:{0}";
            if (slowMove) command = "ZServoTransitionTo:{0},10";

            command = string.Format(command, angle);
            SendCommand(command);

            ZServoAngle = angle;

            RaiseOnLocationChangedEvent();
        }

        public void EnableJog(JogModeTypes jogModeType)
        {
            if (jogModeType != JogModeTypes.None) SendStepResolution(FeedRate.StepResolutions.Half);          

            // Stepper Enabling is performed by the firmware for this task
            SendCommand("Jog:" + (int)jogModeType);
        }

        public void SendDwell(int milliseconds)
        {
            SendCommand("Dwell:" + milliseconds.ToString());
        }

        /// <summary>
        /// Establishes first time communication with machine and sends all initializing commands
        /// </summary>
        public void InitializeHardware()
        {
            EnableStepperDrivers(false);
            SendStepResolution(FeedRate.StepResolutions.Half);

            SendAxisInversionStatus("X", Settings.XAxis.InvertDirection);
            SendAxisInversionStatus("Y", Settings.YAxis.InvertDirection);
            SendAxisInversionStatus("Z", Settings.ZAxis.InvertDirection);

            MoveZServo(Settings.ZServoLoadPosition);

            RequestStatus();

            RaiseOnLocationChangedEvent();
        }

        /// <summary>
        /// Sends the specified command to the device. If timeoutSeconds = 0, there is no timeout, otherwise the command will expect an acknowledgement within
        /// the specified interval.
        /// </summary>
        public void SendCommand(string command, int timeoutSeconds = 0, string address = "")
        {
            if (address == "")
                address = Settings.MachineIPAddress;

            if (EnqueueCommands)
            {
                _commandQueue.Enqueue(command);
                return ;
            }

            if (OnCommandSending != null && !command.Contains("Scan")) OnCommandSending(command);

            var responseText = SocketConnection.SendCommand(command, address, timeoutSeconds);
            if (responseText.Contains("Timeout!"))
                RaiseOnCommandResponseEvent(true, responseText);

            if (responseText.Contains("SE!"))
                RaiseOnCommandResponseEvent(true, responseText.Substring(3));

            RaiseOnCommandResponseEvent(false, responseText);

            // Parse each returned line for specific values and messages
            foreach (string line in responseText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string responseLine = line.ToUpper();
                if (responseLine.Contains("LOCATION:")) UpdateLocationFromResponse(responseLine);
                if (responseLine.Contains("***E-STOP")) ActivateEStop(true);
                if (responseLine.Contains("EDGE TOUCHED AT:")) RaiseTouchedAtEvent(responseLine);
                if (responseLine.Contains("ERROR FACTOR:")) ConvertErrorFactor(responseLine);
            }
        }

        #endregion

        #region Private Methods

        private void ConvertErrorFactor(string responseText)
        {
            var coordinates = responseText.Split(':');
            if (coordinates.Length < 3) return;

            coordinates = coordinates[2].Split(',');
            if (coordinates.Length < 3) return;

            if (coordinates[0].Trim() == "N/A") coordinates[0] = "0";
            if (coordinates[1].Trim() == "N/A") coordinates[1] = "0";
            if (coordinates[2].Trim() == "N/A") coordinates[2] = "0";

            // Machine stores all coordinates as 8 microsteps per full step regardless of the current resolution
            double x = (Convert.ToInt32(coordinates[0]) / 8) / (double)Settings.XAxis.FullStepsPerInch();
            double y = (Convert.ToInt32(coordinates[1]) / 8) / (double)Settings.YAxis.FullStepsPerInch();
            double z = (Convert.ToInt32(coordinates[2]) / 8) / (double)Settings.ZAxis.FullStepsPerInch();

            string message = string.Format("Axes were off by: {0}, {1}, {2}", x, y, z);

            RaiseOnCommandResponseEvent(false, message);
        }

        private void UpdateLocationFromResponse(string responseText)
        {
            var coordinates = responseText.Split(':');
            if (coordinates.Length < 2) return;

            coordinates = coordinates[1].Split(',');
            if (coordinates.Length < 3) return;

            // Machine stores all coordinates as 8 microsteps per full step regardless of the current resolution
            Location.X = (Convert.ToInt32(coordinates[0]) / 8) / (double)Settings.XAxis.FullStepsPerInch();
            Location.Y = (Convert.ToInt32(coordinates[1]) / 8) / (double)Settings.YAxis.FullStepsPerInch();
            Location.Z = (Convert.ToInt32(coordinates[2]) / 8) / (double)Settings.ZAxis.FullStepsPerInch();

            RaiseOnLocationChangedEvent();
        }

        /// <summary>
        /// Raises an event when the edge finder is being used to report the coordinate where the specified axis touched the finder.
        /// This event passes the number of inches from the machine origin where the finder was touched.
        /// </summary>
        private void RaiseTouchedAtEvent(string responseText)
        {
            if (OnEdgeTouched == null) return;

            var coordinate = responseText.Split(':');
            if (coordinate.Length < 2) return;

            // Machine stores all coordinates as 8 microsteps per full step regardless of the current resolution
            double touchedAt = (Convert.ToInt32(coordinate[1]) / 8) / (double)_axisFindingEdgeFor.FullStepsPerInch();
            
            OnEdgeTouched(touchedAt);
        }

        private void SendAxisInversionStatus(string axisID, bool inverted)
        {
            string command = string.Format("AxisInvert:{0}{1}", axisID, inverted ? 1 : 0);
            SendCommand(command);
        }

        private void ActivateEStop(bool active)
        {
            if (active == EStopActive) return;

            EStopActive = active;
            if (OnEStopNotification != null) OnEStopNotification(active);
        }

        private void RaiseOnCommandResponseEvent(bool isError, string responseText)
        {
            if (OnCommandResponse != null) OnCommandResponse(isError, responseText);
        }

        private void RaiseOnLocationChangedEvent()
        {
            if (OnLocationChanged != null) OnLocationChanged(Location, ZServoAngle);
        }

        private void SendStepResolution(FeedRate.StepResolutions resolution)
        {
            _resolution = resolution;
            string resolutionCmd = "StepResolution:" + ((int)_resolution).ToString();
            SendCommand(resolutionCmd);
        }

        #endregion
    }
}
