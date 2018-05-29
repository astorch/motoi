using motoi.platform.ui.shells;

namespace motoi.platform.application {
    /// <summary>
    /// Defines methods for an application to hookup within the application lifecycle.
    /// </summary>
    public interface IMotoiApplication {
        /// <summary>
        /// Return true if the application wants to use a splash screen.
        /// </summary>
        bool ShowSplashscreen { get; }

        /// <summary>
        /// Return TRUE if the application shall run headless. In this case, no main window 
        /// is being initialized.
        /// </summary>
        bool IsHeadless { get; }

        /// <summary>
        /// Tells the application that the framework has been initialized and loaded, so that 
        /// the application does now start.
        /// </summary>
        void OnStartup();

        /// <summary>
        /// Will be invoked just before the main window is initialized.
        /// </summary>
        void OnPreInitializeMainWindow();

        /// <summary>
        /// Will be invoked just after the main window has been initialized.
        /// </summary>
        /// <param name="mainWindow">Currently used main window</param>
        void OnPostInitializeMainWindow(IMainWindow mainWindow);

        /// <summary>
        /// Will be invoked when the UI thread starts and application will run now.
        /// </summary>
        void OnApplicationRun();

        /// <summary>
        /// Will be invoked when the UI Thread ends and the application will shutdown now.
        /// </summary>
        void OnApplicationShutdown();

        /// <summary>
        /// Tells the application that it has been shutdown.
        /// </summary>
        void OnShutdown();
    }
}