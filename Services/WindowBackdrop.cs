using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SimGamepad.Services
{
    public static class WindowBackdrop
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;
        private const int DWMWA_MICA_EFFECT = 1029; // Old undocumented Mica
        
        // Backdrop types for DWMWA_SYSTEMBACKDROP_TYPE
        private const int DWMSBT_AUTO = 0;
        private const int DWMSBT_NONE = 1;         
        private const int DWMSBT_MAINWINDOW = 2;   // Mica
        private const int DWMSBT_TRANSIENTWINDOW = 3; // Acrylic
        private const int DWMSBT_TABBEDWINDOW = 4; // Mica Alt

        public static void ApplyBackdrop(Window window, BackdropType type = BackdropType.Mica)
        {
            if (window == null) return;

            window.Loaded += (s, e) =>
            {
                IntPtr handle = new WindowInteropHelper(window).Handle;
                
                // 1. Enable Dark Mode Titlebar (immersive)
                int darkMode = 1;
                DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));

                // 2. Try SystemBackdrop (Win11 22H2+)
                if (IsWindows11_22H2_OrGreater())
                {
                    int backdropValue = (int)type;
                    DwmSetWindowAttribute(handle, DWMWA_SYSTEMBACKDROP_TYPE, ref backdropValue, sizeof(int));
                }
                else
                {
                    // Fallback or older Win11/10 handling could go here
                    // For now we focus on the "Ultra Modern" Win11 path or simple transparency for older
                }
            };
        }

        public enum BackdropType
        {
            Auto = 0,
            None = 1,
            Mica = 2,
            Acrylic = 3,
            Tabbed = 4
        }

        private static bool IsWindows11_22H2_OrGreater()
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= 22621;
        }
    }
}
