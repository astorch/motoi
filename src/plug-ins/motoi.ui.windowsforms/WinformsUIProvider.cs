using motoi.platform.ui.factories;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Implements <see cref="IUIProvider"/> for the windows forms UI platform.
    /// </summary>
    public class WinformsUIProvider : IUIProvider {
        private IApplicationController iApplicationController;
        private IUIServiceFactory iUIServiceFactory;
        private IShellFactory iShellFactory;
        private IWidgetFactory iWidgetFactory;

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