using System.Collections.Generic;

namespace motoi.workbench.stub.registries {
    /// <summary> Defines the properties of a category contribution. </summary>
    class CategoryContribution : Contribution {

        /// <summary> Returns the associated wizards. </summary>
        public ICollection<WizardContribution> Wizards { get; } = new List<WizardContribution>(30);
    }
}