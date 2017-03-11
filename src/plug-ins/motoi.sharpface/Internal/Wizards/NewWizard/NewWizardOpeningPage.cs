using System;
using System.Collections.Generic;
using System.Linq;
using Motoi.Extensions;
using Motoi.Extensions.Core;
using Motoi.Plugins.Core;
using Motoi.SharpFace.Wizards;
using Motoi.SharpFace.Wizards.Pages;
using Motoi.UI;
using Motoi.UI.Data;
using Motoi.UI.Widgets;

namespace Motoi.SharpFace.Internal.Wizards.NewWizard
{
    /// <summary>
    /// Implements <see cref="IWizardPage"/> to provide an Opening Page 
    /// for the New Wizard.
    /// </summary>
    public class NewWizardOpeningPage : AbstractWizardPage {

        /// <summary>
        /// Returns the currently selected wizard.
        /// </summary>
        public IWizard SelectedWizard { get; private set; }

        /// <summary>
        /// Tells the page to initialize its content.
        /// </summary>
        /// <param name="gridComposite">Element container</param>
        public override void Initialize(IGridComposite gridComposite) {
            Title = "Select a category";
            Description = "Select the type you want to create";

            ITreeViewer treeViewer = UIFactory.NewWidget<ITreeViewer>(gridComposite);
            gridComposite.AddWidget(treeViewer);

            treeViewer.ContentProvider = new NewWizardTreeContentProvider();
            treeViewer.LabelProvider = new NewWizardLabelProvider();
            treeViewer.Input = new object();
            treeViewer.Update();
            treeViewer.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionEventArgs selectionEventArgs) {
            WizardContribution wizard = selectionEventArgs.Selection as WizardContribution;
            bool canLeave = wizard != null;
            CanLeave = canLeave;
            SelectedWizard = null;
            if (wizard != null) {
                SelectedWizard = wizard.Wizard;
            }
        }

        /// <summary>
        /// Tells subclass that dispose has been invoked.
        /// </summary>
        protected override void OnDispose() {
        }

        /// <summary>
        /// Implements <see cref="ITreeContentProvider"/> to provide the content of the 
        /// Tree Viewer.
        /// </summary>
        class NewWizardTreeContentProvider : ITreeContentProvider
        {
            /// <summary>
            /// Extension point id.
            /// </summary>
            private const string ExtensionPointId = "org.motoi.sharpface.wizards.newWizard";

            /// <summary>
            /// Collection of resolved contributions.
            /// </summary>
            private readonly IList<Contribution> iContributions = new List<Contribution>(20);

            /// <summary>
            /// Creates a new instance.
            /// </summary>
            public NewWizardTreeContentProvider() {
                IConfigurationElement[] configurationElements = ExtensionService.Instance
                    .GetConfigurationElements(ExtensionPointId);

                IConfigurationElement[] categories = configurationElements.Where(x => x.Prefix == "category").ToArray();
                IConfigurationElement[] wizards = configurationElements.Where(x => x.Prefix == "wizard").ToArray();
                IDictionary<string,CategoryContribution> idToCategoryMap = new Dictionary<string, CategoryContribution>(categories.Length);

                // Processing categories
                for (int i = -1; ++i < categories.Length;) {
                    IConfigurationElement category = categories[i];
                    string id = category["id"];
                    string label = category["label"];
                    CategoryContribution contribution = new CategoryContribution {Id = id, Label = label};
                    idToCategoryMap.Add(id, contribution);
                    iContributions.Add(contribution);
                }

                // Processing wizards
                for (int i = -1; ++i < wizards.Length;) {
                    IConfigurationElement wizard = wizards[i];
                    string id = wizard["id"];
                    string category = wizard["category"];
                    string label = wizard["label"];
                    string className = wizard["class"];
                    
                    if(string.IsNullOrEmpty(className))
                        continue;

                    IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(wizard);
                    Type wizardType = TypeLoader.TypeForName(providingBundle, className);
                    IWizard wizardImpl = Activator.CreateInstance(wizardType) as IWizard;

                    WizardContribution contribution = new WizardContribution { Id = id, Label = label, Category = category, Wizard = wizardImpl };
                    // Is it a categorized item?
                    if (!string.IsNullOrEmpty(category)) {
                        CategoryContribution categoryContr;
                        if (idToCategoryMap.TryGetValue(category, out categoryContr)) {
                            categoryContr.Wizards.Add(contribution);
                            continue;
                        }
                    }
                    iContributions.Add(contribution);
                }
            }

            /// <summary>
            /// Return all elements that derive from the given input element.
            /// </summary>
            /// <param name="input">Input element</param>
            /// <returns></returns>
            public object[] GetElements(object input) {
                return iContributions.ToArray();
            }

            /// <summary>
            /// Return true if the item has child elements.
            /// </summary>
            /// <param name="item">Item to check</param>
            /// <returns>True if it has child elements</returns>
            public bool HasChildren(object item) {
                CategoryContribution category = item as CategoryContribution;
                if (category == null)
                    return false;

                return category.Wizards.Count != 0;
            }

            /// <summary>
            /// Return all children of the given item.
            /// </summary>
            /// <param name="item">Item</param>
            /// <returns>Children of the item</returns>
            public object[] GetChildren(object item) {
                CategoryContribution category = item as CategoryContribution;
                if (category == null)
                    return new object[0];
                return category.Wizards.ToArray();
            }
        }

        /// <summary>
        /// Implements <see cref="ILabelProvider"/> used by the Tree Viewer.
        /// </summary>
        class NewWizardLabelProvider : ILabelProvider
        {
            /// <summary>
            /// Return the text for the item.
            /// </summary>
            /// <param name="item">Item</param>
            /// <returns>Text of the item</returns>
            public string GetText(object item) {
                Contribution contribution = item as Contribution;
                if (contribution == null)
                    return item.ToString();
                return contribution.Label;
            }

            /// <summary>
            /// Return true if the item shall be enabled.
            /// </summary>
            /// <param name="item">Item</param>
            /// <returns>True if the item shall be enabled</returns>
            public bool IsEnabled(object item) {
                return true;
            }
        }

        /// <summary>
        /// Defines the properties of a category contribution.
        /// </summary>
        class CategoryContribution : Contribution {

            /// <summary>
            /// Creates a new instance.
            /// </summary>
            public CategoryContribution() {
                Wizards = new List<WizardContribution>(5);
            }

            /// <summary>
            /// Returns the associated wizards.
            /// </summary>
            public IList<WizardContribution> Wizards { get; private set; }
        }

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
        }

        /// <summary>
        /// Defines common properties of a contribution.
        /// </summary>
        class Contribution {
            /// <summary>
            /// Returns the id or does set it.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Returns the label or does set it.
            /// </summary>
            public string Label { get; set; }   
        }
    }
}