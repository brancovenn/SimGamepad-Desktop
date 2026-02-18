using System.Diagnostics;

namespace SimGamepad.Services
{
    public class UsbReceiver
    {
        public void Start()
        {
            try
            {
                Process.Start("adb", "devices");
            }
            catch { }
        }

        public void Stop() { }
    }
}
