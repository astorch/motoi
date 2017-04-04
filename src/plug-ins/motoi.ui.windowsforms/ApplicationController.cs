using System;
using System.Windows.Forms;
using motoi.platform.ui.factories;
using motoi.platform.ui.shells;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Implements <see cref="IApplicationController"/> for the windows forms UI platform.
    /// </summary>
    public class ApplicationController : IApplicationController {
        /// <inheritdoc />
        public void RunApplication(IMainWindow mainWindow) {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationContext applicationContext = new ApplicationContext(mainWindow as Form);
            Application.Run(applicationContext);
        }

        /// <inheritdoc />
        public void ShutdownApplication() {
            Application.Exit();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}