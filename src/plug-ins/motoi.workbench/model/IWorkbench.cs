using motoi.platform.ui.shells;
using motoi.workbench.events;

namespace motoi.workbench.model {
    /// <summary> Describes the workbench. </summary>
    public interface IWorkbench {
        /// <summary> Returns the current main window. </summary>
        IMainWindow MainWindow { get; }

        /// <summary> Returns the current active perspective. </summary>
        IPerspective ActivePerspective { get; }

        /// <summary> Opens a perspective with the given perspective id. </summary>
        /// <param name="perspectiveId">Id of the perspective</param>
        /// <returns>Instance of perspective or null</returns>
        /// <exception cref="WorkbenchPartInitializationException">If an error during the process occurs</exception>
        IPerspective OpenPerspective(string perspectiveId);

        /// <summary> Subscribes the given <paramref name="listener"/> to workbench events. </summary>
        /// <param name="listener">Listener to subscribe</param>
        void AddWorkbenchListener(IWorkbenchListener listener);

        /// <summary> Unsubscribes the given <paramref name="listener"/> from workbench events. </summary>
        /// <param name="listener">Listener to unsubscribe</param>
        void RemoveWorkbenchListener(IWorkbenchListener listener);
    }
}