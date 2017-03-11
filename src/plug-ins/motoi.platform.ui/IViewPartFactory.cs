using motoi.platform.ui.shells;

namespace motoi.platform.ui {
    /// <summary>
    /// Defines the interface of a View Part Factory.
    /// </summary>
    public interface IViewPartFactory {
        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TViewPart">Type of the view part to create</typeparam>
        /// <returns>Newly created instance of the given type</returns>
        TViewPart CreateInstance<TViewPart>() where TViewPart : class, IViewPart;

        /// <summary>
        /// Tells the factory to start the message dispatching for the given main window.
        /// </summary>
        /// <param name="mainWindow">Main window of the application</param>
        void RunApplication(IMainWindow mainWindow);
    }
}