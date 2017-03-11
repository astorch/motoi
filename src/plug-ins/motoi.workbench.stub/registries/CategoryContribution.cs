using System.Collections.Generic;
using Xcite.Collections;

namespace motoi.workbench.stub.registries {
    /// <summary>
    /// Defines the properties of a category contribution.
    /// </summary>
    class CategoryContribution : Contribution {

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public CategoryContribution() {
            Wizards = new LinearList<WizardContribution>();
        }

        /// <summary>
        /// Returns the associated wizards.
        /// </summary>
        public ICollection<WizardContribution> Wizards { get; private set; }
    }
}