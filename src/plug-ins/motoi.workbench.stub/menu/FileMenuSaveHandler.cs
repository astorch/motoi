using System;
using motoi.platform.application;
using motoi.platform.ui.actions;
using motoi.workbench.events;
using motoi.workbench.model;

namespace motoi.workbench.stub.menu {
    /// <summary> Provides an action handler for the menu item 'Save'. </summary>
    public class FileMenuSaveHandler : AbstractActionHandler, IWorkbenchListener, IPerspectiveListener {

        /// <summary> Creates a new instance. </summary>
        public FileMenuSaveHandler() {
            PlatformUI.Instance.Workbench.AddWorkbenchListener(this);
            IsEnabled = false;
        }

        /// <inheritdoc />
        public override void Run() {
            try {
                PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor.ExecuteSave(null);
            } catch (Exception ex) {
                Platform.Instance.PlatformLog.ErrorFormat("Error on performing ExecuteSave() on active editor. Reason: {0}", ex);
            }
        }

        /// <inheritdoc />
        public void OnWorkbenchPartOpened(IWorkbenchPart workbenchPart) {
            IEditor editor = workbenchPart as IEditor;
            if (editor == null) return;
            IsEnabled = false;
            editor.DirtyChanged += OnEditorIsDirtyChanged;
        }

        /// <inheritdoc />
        public void OnWorkbenchPartClosed(IWorkbenchPart workbenchPart) {
            IEditor editor = workbenchPart as IEditor;
            if (editor == null) return;
            IsEnabled = false;
            editor.DirtyChanged -= OnEditorIsDirtyChanged;
        }

        /// <inheritdoc />
        public void OnWorkbenchPartActivated(IWorkbenchPart workbenchPart) {
            // Nothing to do
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

        /// <inheritdoc />
        public void OnPerspectiveChanged(IPerspective oldPerspective, IPerspective newPerspective) {
            oldPerspective?.RemovePerspectiveListener(this);
            newPerspective?.AddPerspectiveListener(this);
        }
    }
}