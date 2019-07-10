using System;
using motoi.platform.application;
using motoi.platform.ui.actions;
using motoi.workbench.events;
using motoi.workbench.model;

namespace motoi.workbench.stub.menu {
    /// <summary> Provides an action handler for the menu item 'Save as'. </summary>
    public class FileMenuSaveAsHandler : AbstractActionHandler, IWorkbenchListener, IPerspectiveListener {
        /// <inheritdoc />
        public FileMenuSaveAsHandler() {
            PlatformUI.Instance.Workbench.AddWorkbenchListener(this);
            IsEnabled = false;
        }

        /// <inheritdoc />
        public override void Run() {
            try {
                PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor.ExecuteSaveAs();
            } catch (Exception ex) {
                Platform.Instance.PlatformLog.Error("Error on performing ExecuteSaveAs() on active editor.", ex);
            }
        }

        /// <inheritdoc />
        public void OnWorkbenchPartOpened(IWorkbenchPart workbenchPart) {
            IsEnabled = PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor != null;
        }


        /// <inheritdoc />
        public void OnWorkbenchPartClosed(IWorkbenchPart workbenchPart) {
            IsEnabled = PlatformUI.Instance.Workbench.ActivePerspective.ActiveEditor != null;
        }

        /// <inheritdoc />
        public void OnWorkbenchPartActivated(IWorkbenchPart workbenchPart) {
            // Nothing to do
        }

        /// <inheritdoc />
        public void OnPerspectiveChanged(IPerspective oldPerspective, IPerspective newPerspective) {
            oldPerspective?.RemovePerspectiveListener(this);
            newPerspective?.AddPerspectiveListener(this);
        }
    }
}