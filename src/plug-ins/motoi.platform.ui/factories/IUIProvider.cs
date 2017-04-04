namespace motoi.platform.ui.factories {
    /// <summary>
    /// Provides methods to get access to the UI platform specific factories.
    /// </summary>
    /// <seealso cref="IApplicationController"/>
    /// <seealso cref="IShellFactory"/>
    /// <seealso cref="IWidgetFactory"/>
    /// <seealso cref="IUIServiceFactory"/>
    public interface IUIProvider {
        /// <summary>
        /// Returns the instance of <see cref="IApplicationController"/> for the underlying 
        /// UI platform.
        /// </summary>
        /// <returns>Instance of <see cref="IApplicationController"/></returns>
        IApplicationController GetApplicationController();

        /// <summary>
        /// Returns the instance of <see cref="IShellFactory"/> for the underlying 
        /// UI platform.
        /// </summary>
        /// <returns>Instance of <see cref="IShellFactory"/></returns>
        IShellFactory GetShellFactory();

        /// <summary>
        /// Returns the instance of <see cref="IWidgetFactory"/> for the underlying 
        /// UI platform.
        /// </summary>
        /// <returns>Instance of <see cref="IWidgetFactory"/></returns>
        IWidgetFactory GetWidgetFactory();

        /// <summary>
        /// Returns the instance of <see cref="IUIServiceFactory"/> for the underlying 
        /// UI platform.
        /// </summary>
        /// <returns>Instance of <see cref="IUIServiceFactory"/></returns>
        IUIServiceFactory GetUIServiceFactory();
    }
}