using System;
using System.Text;

namespace buildlight
{
    public class Color : IColor
    {
        public static IColor Green = new Color(Delcom.GREENLED);
        public static IColor Red = new Color(Delcom.REDLED);
        public static IColor Blue = new Color(Delcom.BLUELED);
        public static IColor Pink = new CompositeColor(Red, Blue);
        public static IColor Yellow = new CompositeColor(Red, Green);
        public static IColor BlueGreen = new CompositeColor(Green, Blue);
        public static IColor AllColors = new CompositeColor(Red, Green, Blue);

        private readonly int color;

        public Color(int color)
        {
            this.color = color;
        }

        public virtual void Off()
        {
            SetLed(Delcom.LEDOFF);
        }

        public virtual void On()
        {
            SetLed(Delcom.LEDON);
        }

        public virtual void Flash()
        {
            SetLed(Delcom.LEDFLASH);
        }

        private void SetLed(int action)
        {
            var deviceName = new StringBuilder(Delcom.MAXDEVICENAMELEN);
            var result = Delcom.DelcomGetNthDevice(Delcom.USBDELVI, 0, deviceName);

            if (result == 0)
                throw new ApplicationException("Device not found!");

            var deviceHandle = Delcom.DelcomOpenDevice(deviceName, 0);
            Delcom.DelcomLEDControl(deviceHandle, color, action);
            Delcom.DelcomCloseDevice(deviceHandle);
        }
    }
}
