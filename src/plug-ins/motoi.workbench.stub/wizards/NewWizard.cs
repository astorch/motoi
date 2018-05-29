using System;
using motoi.platform.application;
using motoi.workbench.runtime;
using motoi.workbench.stub.wizards.pages;

namespace motoi.workbench.stub.wizards {
    /// <summary>
    /// Provides an implementation of a new file wizard.
    /// </summary>
    public class NewWizard : AbstractWizard {
        private readonly NewWizardOpeningPage iOpeningPage = new NewWizardOpeningPage();

        /// <summary>
        /// Tells the subclass that initialize has been invoked.
        /// </summary>
        protected override void OnInitialize() {
            AddWizardPage(iOpeningPage);
            Title = Messages.NewWizard_Title;
        }

        /// <summary>
        /// Tells the subclass that cancel has been invoked.
        /// </summary>
        protected override void OnCancel() {
            // Currently nothing to do here
        }

        /// <summary>
        /// Tells the subclass that finish has been invoked.
        /// </summary>
        protected override void OnFinish() {
            try {
                if (iOpeningPage.SelectedWizard != null)
                    iOpeningPage.SelectedWizard.Open();
            } catch (Exception ex) {
                Platform.Instance.PlatformLog.Error(ex, "Error on performing OnFinish().");
            }
        }

        /// <summary>
        /// Tells the subclass that dispose has been invoked.
        /// </summary>
        protected override void OnDispose() {
            iOpeningPage.Dispose();
        }
    }
}