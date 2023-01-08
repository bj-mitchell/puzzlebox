using System.Device.Gpio;
using System.Device.I2c;

namespace WebSocketServer
{
    public class Keypad
    {
        private readonly I2cDevice? keypad;
        private readonly GpioController gpio;
        private const int I2C_BUS = 1;
        private const int I2C_ADDRESS = 0x2a;
        private const int INTERRUPT_PIN = 17;
        private const int RESET_PIN = 18;

        public Keypad(GpioController gpio)
        {
            this.gpio = gpio;
            Setup();
            Reset();
        }

        private void Reset()
        {
            Console.WriteLine("Resetting keypad...");
            gpio.Write(RESET_PIN, 1);
            Thread.Sleep(200);
            gpio.Write(RESET_PIN, 0);
        }

        private void Setup()
        {
            I2cDevice keypad = I2cDevice.Create(new I2cConnectionSettings(I2C_BUS, I2C_ADDRESS));
            gpio.OpenPin(INTERRUPT_PIN, System.Device.Gpio.PinMode.Input);
            gpio.OpenPin(RESET_PIN, System.Device.Gpio.PinMode.Output);
        }
    }
}
