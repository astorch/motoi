using motoi.platform.ui.shells;

namespace motoi.platform.ui.factories {
    /// <summary>
    /// Provides methods to control the platform specific application lifecycle.
    /// </summary>
    public interface IApplicationController {
        /// <summary>
        /// Tells the instance to start the message dispatching for the given main window..
        /// </summary>
        /// <param name="mainWindow">Main window of the application</param>
        void RunApplication(IMainWindow mainWindow);

        /// <summary>
        /// Tells the instance to shutdown the current running application.
        /// </summary>
        void ShutdownApplication();
    }
}