using System;
using motoi.platform.application;
using motoi.platform.ui.actions;
using motoi.workbench.events;
using motoi.workbench.model;

namespace motoi.workbench.stub.menu {
    /// <summary>
    /// Provides an action handler for the menu item 'Save as'.
    /// </summary>
    public class FileMenuSaveAsHandler : AbstractActionHandler, IWorkbenchListener, IPerspectiveListener {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public FileMenuSaveAsHandler() {
            PlatformUI.Instance.Workbench.AddWorkbenchListener(this);
            IsEnabled = false;
        }

        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public override void Run() {
            try {
                PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor.ExecuteSaveAs();
            } catch (Exception ex) {
                Platform.Instance.PlatformLog.ErrorFormat("Error on performing ExecuteSaveAs() on active editor. Reason: {0}", ex);
            }
        }

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been opened.
        /// </summary>
        /// <param name="workbenchPart">Opened workbench part</param>
        public void OnWorkbenchPartOpened(IWorkbenchPart workbenchPart) {
            IsEnabled = PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor != null;
        }

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been closed.
        /// </summary>
        /// <param name="workbenchPart">Closed workbench part</param>
        public void OnWorkbenchPartClosed(IWorkbenchPart workbenchPart) {
            IsEnabled = PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor != null;
        }

        /// <summary>
        /// Tells the instance that the active perspective of the workbench has been change.
        /// </summary>
        /// <param name="oldPerspective">Previous perspective</param>
        /// <param name="newPerspective">New perspective</param>
        public void OnPerspectiveChanged(IPerspective oldPerspective, IPerspective newPerspective) {
            if (oldPerspective != null)
                oldPerspective.RemovePerspectiveListener(this);

            if (newPerspective != null)
                newPerspective.AddPerspectiveListener(this);
        }
    }
}