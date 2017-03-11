using motoi.platform.ui.actions;
using motoi.workbench.stub.wizards;

namespace motoi.workbench.stub.menu {
    /// <summary>
    /// Implements the main menu action 'New ...' which opens a standard new wizard.
    /// </summary>
    public class FileMenuNewHandler : AbstractActionHandler {
        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public override void Run() {
            using (NewWizard newWizard = new NewWizard()) {
                newWizard.Open();
            }
        }
    }
}