using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;

namespace DazCAM
{
    public class Servo
    {
        readonly PWM _servo;

        public Servo(Cpu.Pin pin)
        {
            _servo = new PWM(pin);
            _servo.SetDutyCycle(dutyCycle: 0);
        }

        private uint _angle;

        public uint Angle
        {
            get
            {
                return _angle;
            }

            set
            {
                const uint degreeMin = 0;
                const uint degreeMax = 180;
                const uint durationMin = 500;  // 480  
                const uint durationMax = 2300;  // 2450  
                _angle = value;
                if (_angle < degreeMin) _angle = degreeMin;
                if (_angle > degreeMax) _angle = degreeMax;
                uint dur = (_angle - degreeMin) * (durationMax - durationMin) / (degreeMax - degreeMin) + durationMin;
                _servo.SetPulse(20000, dur);
            }
        }
    }
}