using System;
using motoi.platform.application;
using motoi.workbench.runtime;
using motoi.workbench.stub.wizards.pages;

namespace motoi.workbench.stub.wizards {
    /// <summary> Provides an implementation of a new file wizard. </summary>
    public class NewWizard : AbstractWizard {
        private readonly NewWizardOpeningPage _openingPage = new NewWizardOpeningPage();
        
        /// <inheritdoc />
        protected override void OnInitialize() {
            AddWizardPage(_openingPage);
            Title = Messages.NewWizard_Title;
        }
        
        /// <inheritdoc />
        protected override void OnCancel() {
            // Currently nothing to do here
        }
        
        /// <inheritdoc />
        protected override void OnFinish() {
            try {
                _openingPage.SelectedWizard?.Open();
            } catch (Exception ex) {
                Platform.Instance.PlatformLog.Error("Error on performing OnFinish().", ex);
            }
        }
        
        /// <inheritdoc />
        protected override void OnDispose() {
            _openingPage.Dispose();
        }
    }
}