using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DazCAM
{
    public class Axis
    {
        #region Declarations

        public delegate void AxisLimitReachedEventHandler(Axis sender);

        // True = Positive direction (pin output of 1)
        private bool _stepDirection;

        public enum LimitSwitch { None, Min, Max, Unknown };

        #endregion

        #region Properties

        public string ID { get; private set; }
        public LimitSwitch LimitReached { get; private set; }

        public OutputPort StepPort { get; private set; }
        public OutputPort DirectionPort { get; private set; }
        public InterruptPort LimitSwitchPort { get; private set; }

        public event AxisLimitReachedEventHandler OnAxisLimitReached;

        // Inverts the final output of the Direction Pin, allowing us to use positive coordinates regardless of the wiring of the axis
        public bool InvertDirectionPinOutput { get; set; }

        public bool IgnoreLimit { get; set; }

        public bool StepDirection
        {
            get { return _stepDirection; }

            set
            {
                // NOTE: This line optimizes a little bit of CPU time for long complicated paths - no need to keep setting the direction port
                //       for each line segment. However it is very important that the constructor first sets this property to True to ensure
                //       that the correct output exists on the pin based on the value of InvertDirectionPinOutput
                if (_stepDirection == value) return;

                _stepDirection = value;
                DirectionPort.Write(_stepDirection ^ InvertDirectionPinOutput);		// XOR against InvertDirectionPinOutput for complete hardware abstraction of the step direction
            }
        }

        public int LocationUnitsPerStep { get; set; }       // In Full step mode, this would be 8 - a Location Unit is 8 per Full step
        public int Location { get; private set; }           // Keeps track of the axis location in multiples of 8. EG: Half step would move in increments of 4.

        #endregion

        #region Constructors

        public Axis(string axisID, Cpu.Pin stepPin, Cpu.Pin directionPin, Cpu.Pin limitSwitchPin, bool invertDirectionPinOutput = false)
        {
            this.ID = axisID;
            StepPort = new OutputPort(stepPin, false);
            DirectionPort = new OutputPort(directionPin, false);
            LimitReached = LimitSwitch.None;

            if (limitSwitchPin != Cpu.Pin.GPIO_NONE)
            {
                LimitSwitchPort = new InterruptPort(limitSwitchPin, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLevelLow);
                LimitSwitchPort.OnInterrupt += new NativeEventHandler(LimitSwitchPin_OnInterrupt);

                // Since we're sharing a port for the Min/Max limit switches on each axis, we can only determine which switch is tripped 
                // while there is motion (based on direction). If we initialize the axis and find that the switch is tripped, then we don't 
                // know which end it is at and the user will need to move the axis off of the switch before the machine can be homed.
                if (LimitSwitchPort.Read() == false) LimitReached = LimitSwitch.Unknown;
            }

            InvertDirectionPinOutput = invertDirectionPinOutput;

            Location = 0;
            LocationUnitsPerStep = 4;       //Should be overwritten when machine sets the resolution
            StepDirection = true;
        }

        #endregion

        #region Public Methods

        public void ResetLocation()
        {
            Location = 0;
        }

        /// <summary>
        /// Steps the axis 1 pulse in the current direction and updates the current location. 
        /// </summary>
        public void Step()
        {
            this.StepPort.Write(true);

            if (_stepDirection) Location += LocationUnitsPerStep;
            else Location -= LocationUnitsPerStep;

            this.StepPort.Write(false);

            return;
        }

        public bool IsAtLimit(out string whichLimitSwitch)
        {
            switch (this.LimitReached)
            {
                case LimitSwitch.None:
                    whichLimitSwitch = "None";
                    return false;
                case LimitSwitch.Min:
                    whichLimitSwitch = "Minimum";
                    return true;
                case LimitSwitch.Max:
                    whichLimitSwitch = "Maximum";
                    return true;
                case LimitSwitch.Unknown:
                    whichLimitSwitch = "Unknown";
                    return true;
            }
            whichLimitSwitch = "";
            return false;
        }

        public bool IsAtLimit()
        {
            return (this.LimitReached != LimitSwitch.None);
        }

        /// <summary>
        /// Attempts to reset the LimitReached to None by first checking the Limit Switch port. Returns true if the limit switch is no longer
        /// closed (grounded). 
        /// </summary>
        /// <returns></returns>
        public bool ResetLimitReached()
        {
            if (LimitSwitchPort == null) return true;                   // No limit switch for this axis
            if (this.LimitReached == LimitSwitch.None) return true;     // Nothing to do, already reset
            if (LimitSwitchPort.Read() == false) return false;          // Switch is still closed (grounded=false)

            // Go ahead and reset the limit switch condition and interrupt
            LimitReached = LimitSwitch.None;
            LimitSwitchPort.ClearInterrupt();

            return true;
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is raised only when the limit switch is tripped. Call Axis.ResetLimitReached() to attempt to set it to none.
        /// </summary>
        private void LimitSwitchPin_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (IgnoreLimit) return;

            if (this._stepDirection) this.LimitReached = LimitSwitch.Max;
            else this.LimitReached = LimitSwitch.Min;

            if (OnAxisLimitReached != null) OnAxisLimitReached(this);
        }

        #endregion
    }
}
