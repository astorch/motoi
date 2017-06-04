using motoi.workbench.model;

namespace motoi.workbench.events {
    /// <summary>  Defines a listener of perspective events. </summary>
    public interface IPerspectiveListener {
        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been opened.
        /// </summary>
        /// <param name="workbenchPart">Opened workbench part</param>
        void OnWorkbenchPartOpened(IWorkbenchPart workbenchPart);

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been activated.
        /// </summary>
        /// <param name="workbenchPart">Activated workbench part</param>
        void OnWorkbenchPartActivated(IWorkbenchPart workbenchPart);

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been closed.
        /// </summary>
        /// <param name="workbenchPart">Closed workbench part</param>
        void OnWorkbenchPartClosed(IWorkbenchPart workbenchPart);
    }
}