using System;
using System.Runtime.InteropServices;

namespace motoi.moose.applicationstarter {
    /// <summary>
    /// Stellt Methoden bereit, um ein natives Konsolenfenster anzuzeigen.
    /// </summary>
    public static class ConsoleManager {
        // http://stackoverflow.com/questions/472282/show-console-in-windows-application
        // http://stackoverflow.com/questions/15604014/no-console-output-when-using-allocconsole-and-target-architecture-x86

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        /// <summary>
        /// Shows the console window that is assigned to the current process. If there isn't any window yet, 
        /// a new one will be allocated. Otherwise the existing one will be brought to front.
        /// </summary>
        public static void ShowConsoleWindow() {
            IntPtr handle = GetConsoleWindow();

            if (handle == IntPtr.Zero) {
                AllocConsole();
            } else {
                ShowWindow(handle, SW_SHOW);
            }
        }

        /// <summary>
        /// Hides the console window that is assigned to the current process. If there isn't any window, 
        /// nothing will happen.
        /// </summary>
        public static void HideConsoleWindow() {
            IntPtr handle = GetConsoleWindow();
            if (handle == IntPtr.Zero) return;

            ShowWindow(handle, SW_HIDE);
            FreeConsole();
        }
    }
}