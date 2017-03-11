using motoi.platform.ui.actions;
using motoi.workbench;

namespace DemoApplication {
    /// <summary>
    /// Provides an action which closes the application.
    /// </summary>
    public class FileMenuCloseHandler : AbstractActionHandler {
        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public override void Run() {
            PlatformUI.Instance.Workbench.MainWindow.Close();
        }
    }
}