using System.Device.Gpio;
using System.Device.I2c;
using System.Text;

namespace WebSocketServer
{
    public delegate void KeypadNotify();

    public class Keypad
    {
        public event KeypadNotify? OnKeypadButtonPress;

        private I2cDevice keypad;
        private readonly GpioController gpio;
        private const int I2C_BUS = 1;
        private const int I2C_ADDRESS = 0x2a;
        private const int INTERRUPT_PIN = 17;
        private const int RESET_PIN = 18;
        
        public Keypad(GpioController gpio)
        {
            this.gpio = gpio;
            
            keypad = I2cDevice.Create(new I2cConnectionSettings(I2C_BUS, I2C_ADDRESS));
            gpio.OpenPin(INTERRUPT_PIN, PinMode.Input);
            gpio.OpenPin(RESET_PIN, PinMode.Output);

            Reset();

            gpio.RegisterCallbackForPinValueChangedEvent(INTERRUPT_PIN, PinEventTypes.Rising, OnInterruptEvent);
            
        }

        private void Reset()
        {
            Console.WriteLine("Resetting keypad controller...");
            gpio.Write(RESET_PIN, 1);
            Thread.Sleep(200);
            gpio.Write(RESET_PIN, 0);
        }

        private void OnInterruptEvent(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            var read = keypad.ReadByte();
            byte[] bytes = new byte[1];
            bytes[0] = read;
            Console.WriteLine("Keypad key pressed:  " + ASCIIEncoding.UTF8.GetString(bytes));
            OnKeypadButtonPress?.Invoke();
        }

    }
}
