using Motoi.SharpFace.Internal.Wizards.NewWizard;
using Motoi.UI.Actions;

namespace Motoi.SharpFace.Menus.Handlers
{
    /// <summary>
    /// Implements an Action Handler which opens the New Wizard.
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