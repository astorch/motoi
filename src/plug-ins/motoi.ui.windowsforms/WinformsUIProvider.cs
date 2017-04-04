using System;
using System.Windows.Forms;
using motoi.platform.ui.factories;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Implements <see cref="IUIProvider"/> for the windows forms UI platform.
    /// </summary>
    public class WinformsUIProvider : IUIProvider {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private IApplicationController iApplicationController;
        private IUIServiceFactory iUIServiceFactory;
        private IShellFactory iShellFactory;
        private IWidgetFactory iWidgetFactory;

        /// <inheritdoc />
        public WinformsUIProvider() {
            // The settings must been set before any control has been created
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        /// <inheritdoc />
        public IApplicationController GetApplicationController() {
            return iApplicationController ?? (iApplicationController = new ApplicationController());
        }

        /// <inheritdoc />
        public IShellFactory GetShellFactory() {
            return iShellFactory ?? (iShellFactory = new ShellFactory());
        }

        /// <inheritdoc />
        public IWidgetFactory GetWidgetFactory() {
            return iWidgetFactory ?? (iWidgetFactory = new WidgetFactory());
        }

        /// <inheritdoc />
        public IUIServiceFactory GetUIServiceFactory() {
            return iUIServiceFactory ?? (iUIServiceFactory = new ServiceFactory());
        }
    }
}