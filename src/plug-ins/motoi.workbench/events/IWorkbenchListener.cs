using motoi.workbench.model;

namespace motoi.workbench.events {
    /// <summary>
    /// Defines a listener of workbench events.
    /// </summary>
    public interface IWorkbenchListener {
        /// <summary>
        /// Tells the instance that the active perspective of the workbench has been change.
        /// </summary>
        /// <param name="oldPerspective">Previous perspective</param>
        /// <param name="newPerspective">New perspective</param>
        void OnPerspectiveChanged(IPerspective oldPerspective, IPerspective newPerspective);
    }
}