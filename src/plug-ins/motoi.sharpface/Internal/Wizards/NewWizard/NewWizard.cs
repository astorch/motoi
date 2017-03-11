using Motoi.SharpFace.Wizards;

namespace Motoi.SharpFace.Internal.Wizards.NewWizard
{
    /// <summary>
    /// Implements a common New Wizard.
    /// </summary>
    public class NewWizard : AbstractWizard {
        
        private readonly NewWizardOpeningPage iOpeningPage = new NewWizardOpeningPage();

        /// <summary>
        /// Tells the subclass that initialize has been invoked.
        /// </summary>
        protected override void OnInitialize() {
            AddWizardPage(iOpeningPage);
            Title = "New";
        }

        /// <summary>
        /// Tells the subclass that cancel has been invoked.
        /// </summary>
        protected override void OnCancel() {
        }

        /// <summary>
        /// Tells the subclass that finish has been invoked.
        /// </summary>
        protected override void OnFinish() {
            if(iOpeningPage.SelectedWizard != null)
                iOpeningPage.SelectedWizard.Open();
        }

        /// <summary>
        /// Tells the subclass that dispose has been invoked.
        /// </summary>
        protected override void OnDispose() {
        }
    }
}