using motoi.platform.ui.images;
using motoi.workbench.model;

namespace motoi.workbench.stub.registries {
    /// <summary>
    /// Defines the properties of a wizard contribution.
    /// </summary>
    class WizardContribution : Contribution {
        /// <summary>
        /// Returns the associated category or does set it.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Returns the wizard or does set it.
        /// </summary>
        public IWizard Wizard { get; set; }

        /// <summary>
        /// Returns the image of the wizard or does set it.
        /// </summary>
        public ImageDescriptor Image { get; set; }
    }
}