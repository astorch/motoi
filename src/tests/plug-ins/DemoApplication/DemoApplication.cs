using motoi.platform.application.model;
using motoi.platform.ui.shells;

namespace DemoApplication {
    /// <summary>
    /// Provides a demo implementation of <see cref="IMotoiApplication"/>.
    /// </summary>
    public class DemoApplication : IMotoiApplication {
        /// <summary>
        /// Return true if the application wants to usea splash screen.
        /// </summary>
        public bool ShowSplashscreen { get { return false; } }

        /// <summary>
        /// Return TRUE if the application shall run headless. In this case, no main window 
        /// is being initialized.
        /// </summary>
        public bool IsHeadless { get { return false; } }

        /// <summary>
        /// Tells the application that the framework has been initialized and loaded, so that 
        /// the application does now start.
        /// </summary>
        public void OnStartup() { }

        /// <summary>
        /// Will be invoked just before the main window is initialized.
        /// </summary>
        public void OnPreInitializeMainWindow() { }

        /// <summary>
        /// Will be invoked just after the main window has been initialized.
        /// </summary>
        /// <param name="mainWindow">Currently used main window</param>
        public void OnPostInitializeMainWindow(IMainWindow mainWindow) {
            mainWindow.WindowTitle = "Motoi - Demo Application";
            mainWindow.Width = 800;
            mainWindow.Height = 600;
        }

        /// <summary>
        /// Will be invoked when the UI thread starts and application will run now.
        /// </summary>
        public void OnApplicationRun() { }

        /// <summary>
        /// Will be invoked when the UI Thread ends and the application will shutdown now.
        /// </summary>
        public void OnApplicationShutdown() { }

        /// <summary>
        /// Tells the application that it has been shutdown.
        /// </summary>
        public void OnShutdown() { }
    }
}
