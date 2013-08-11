using Microsoft.SPOT.Hardware;

namespace DazCAM
{
    public class Devices
    {
        #region Public Properties

        public SecretLabs.NETMF.Hardware.AnalogInput JogXPort { get; private set; }
        public SecretLabs.NETMF.Hardware.AnalogInput JogYPort { get; private set; }
        public SecretLabs.NETMF.Hardware.AnalogInput JogZPort { get; private set; }

        #endregion

        #region Constructors

        public Devices(Cpu.Pin jogXPin, Cpu.Pin jogYPin, Cpu.Pin jogZPin)
        {
            if (jogXPin != Cpu.Pin.GPIO_NONE)
            {
                JogXPort = new SecretLabs.NETMF.Hardware.AnalogInput(jogXPin);
                JogXPort.SetRange(-50, 50);
            }
            if (jogYPin != Cpu.Pin.GPIO_NONE)
            {
                JogYPort = new SecretLabs.NETMF.Hardware.AnalogInput(jogYPin);
                JogYPort.SetRange(-50, 50);
            }
            if (jogZPin != Cpu.Pin.GPIO_NONE)
            {
                JogZPort = new SecretLabs.NETMF.Hardware.AnalogInput(jogZPin);
                JogZPort.SetRange(-50, 50);
            }
        }

        #endregion

        #region Public Methods

        public void ShutdownAll()
        {
            // Used by E-Stop etc - will eventually turn off spindle and dust extraction
        }

        #endregion
    }
}
