using motoi.platform.ui.actions;

namespace motoi.workbench.stub.menu {
    /// <summary>
    /// Implements the main menu action 'Close'.
    /// </summary>
    public class FileMenuExitHandler : AbstractActionHandler {
        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public override void Run() {
            PlatformUI.Instance.Workbench.MainWindow.Close();
        }
    }
}