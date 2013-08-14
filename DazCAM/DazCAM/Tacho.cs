using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace DazCAM
{
    public class Tacho
    {
        private static double _lastPulse = 0;
        private static double _lastDifference;
        private const int MilliSecondsPerMinute = 60000;

        public Tacho()
        {
           
        }

        public void Intialize(Cpu.Pin tPin = Cpu.Pin.GPIO_Pin13)
        {
             var input = new InterruptPort(tPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh);
            input.OnInterrupt += OnFanRotation;
        }

        private static void OnFanRotation(uint data1, uint data2, DateTime time)
        {

            var current = Utility.GetMachineTime().Ticks / TimeSpan.TicksPerMillisecond;
            if (_lastPulse > 0)
                _lastDifference = current - _lastPulse;

            _lastPulse = current;
        }

        public double GetRpm()
        {
            if (_lastDifference > 0)
            {
                var milliSecondsPerRotation = _lastDifference * 2; //2 Pulses per rotation
                var rpm = MilliSecondsPerMinute / milliSecondsPerRotation;
                return rpm;
            }
            return 0;
        }
    }
}