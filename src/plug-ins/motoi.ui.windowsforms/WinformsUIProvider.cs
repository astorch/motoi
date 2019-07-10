using System;
using System.Windows.Forms;
using motoi.platform.ui.factories;

namespace motoi.ui.windowsforms {
    /// <summary> Implements <see cref="IUIProvider"/> for the windows forms UI platform. </summary>
    public class WinformsUIProvider : IUIProvider {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private IApplicationController _applicationController;
        private IUIServiceFactory _uiServiceFactory;
        private IShellFactory _shellFactory;
        private IWidgetFactory _widgetFactory;

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
            return _applicationController ?? (_applicationController = new ApplicationController());
        }

        /// <inheritdoc />
        public IShellFactory GetShellFactory() {
            return _shellFactory ?? (_shellFactory = new ShellFactory());
        }

        /// <inheritdoc />
        public IWidgetFactory GetWidgetFactory() {
            return _widgetFactory ?? (_widgetFactory = new WidgetFactory());
        }

        /// <inheritdoc />
        public IUIServiceFactory GetUIServiceFactory() {
            return _uiServiceFactory ?? (_uiServiceFactory = new ServiceFactory());
        }
    }
}