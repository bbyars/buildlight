using System;
using System.Text;

namespace buildlight
{
    public class Color : IColor
    {
        public static IColor Green = new Color(Delcom.GREENLED);
        public static IColor Red = new Color(Delcom.REDLED);
        public static IColor Orange = new Color(Delcom.BLUELED); // mislabeled in their code
        public static IColor Yellow = new CompositeColor(Red, Orange);
        public static IColor Gold = new CompositeColor(Red, Green);
        public static IColor DarkOrange = new CompositeColor(Green, Orange);
        public static IColor AllColors = new CompositeColor(Red, Green, Orange);

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
