using System;
using motoi.platform.application;
using motoi.platform.ui.actions;
using motoi.workbench.events;
using motoi.workbench.model;

namespace motoi.workbench.stub.menu {
    /// <summary>
    /// Provides an action handler for the menu item 'Save'.
    /// </summary>
    public class FileMenuSaveHandler : AbstractActionHandler, IWorkbenchListener, IPerspectiveListener {

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public FileMenuSaveHandler() {
            PlatformUI.Instance.Workbench.AddWorkbenchListener(this);
            IsEnabled = false;
        }

        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public override void Run() {
            try {
                PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor.ExecuteSave(null);
            } catch (Exception ex) {
                Platform.Instance.PlatformLog.ErrorFormat("Error on performing ExecuteSave() on active editor. Reason: {0}", ex);
            }
        }

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been opened.
        /// </summary>
        /// <param name="workbenchPart">Opened workbench part</param>
        public void OnWorkbenchPartOpened(IWorkbenchPart workbenchPart) {
            IEditor editor = workbenchPart as IEditor;
            if (editor == null) return;
            IsEnabled = false;
            editor.DirtyChanged += OnEditorIsDirtyChanged;
        }

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been closed.
        /// </summary>
        /// <param name="workbenchPart">Closed workbench part</param>
        public void OnWorkbenchPartClosed(IWorkbenchPart workbenchPart) {
            IEditor editor = workbenchPart as IEditor;
            if (editor == null) return;
            IsEnabled = false;
            editor.DirtyChanged -= OnEditorIsDirtyChanged;
        }

        /// <summary>
        /// Is invoked when the dirty state of the currently observed editor changed.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="eventArgs">Event arguments</param>
        private void OnEditorIsDirtyChanged(object sender, EventArgs eventArgs) {
            IEditor editor = (IEditor)sender;
            IsEnabled = editor.IsDirty;
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