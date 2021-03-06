﻿using System.Linq;
using motoi.platform.ui.data;
using motoi.platform.ui.factories;
using motoi.platform.ui.images;
using motoi.platform.ui.widgets;
using motoi.workbench.model;
using motoi.workbench.runtime;
using motoi.workbench.stub.registries;

namespace motoi.workbench.stub.wizards.pages {
    /// <summary>
    /// Implements <see cref="IWizardPage"/> to provide an opening page 
    /// for the New Wizard.
    /// </summary>
    public class NewWizardOpeningPage : AbstractWizardPage {
        /// <summary> Returns the currently selected wizard. </summary>
        public IWizard SelectedWizard { get; private set; }
        
        /// <inheritdoc />
        public override void Initialize(IGridPanel gridComposite, IWidgetFactory widgetFactory) {
            Title = Messages.NewWizardOpeningPage_Title;
            Description = Messages.NewWizardOpeningPage_Description;

            ITreeViewer treeViewer = UIFactory.NewWidget<ITreeViewer>(gridComposite);
            gridComposite.AddWidget(treeViewer);

            treeViewer.ContentProvider = new NewWizardTreeContentProvider();
            treeViewer.LabelProvider = new NewWizardTreeLabelProvider();
            treeViewer.Input = NewWizardRegistry.Instance.Contributions;
            treeViewer.Update();
            treeViewer.SelectionChanged += OnSelectionChanged;
        }

        /// <summary> Is invoked when a selection has been made. </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="selectionEventArgs">Event arguments</param>
        private void OnSelectionChanged(object sender, SelectionEventArgs selectionEventArgs) {
            WizardContribution wizardContribution = selectionEventArgs.Selection as WizardContribution;
            bool canLeave = wizardContribution != null;
            CanLeave = canLeave;
            SelectedWizard = null;

            if (wizardContribution == null) return;
            
            SelectedWizard = wizardContribution.Wizard;
        }
        
        /// <inheritdoc />
        public override void Dispose() {
            // Currently nothing to do here
        }

        /// <summary> Provides an implementation of <see cref="ITreeContentProvider"/>. </summary>
        class NewWizardTreeContentProvider : ITreeContentProvider {
            /// <inheritdoc />
            public object[] GetElements(object input) {
                Contribution[] contributions = (Contribution[]) input;
                return contributions;
            }

            /// <inheritdoc />
            public bool HasChildren(object item) {
                CategoryContribution category = item as CategoryContribution;
                if (category == null) return false;

                return category.Wizards.Count != 0;
            }

            /// <inheritdoc />
            public object[] GetChildren(object item) {
                CategoryContribution category = item as CategoryContribution;
                if (category == null) return new object[0];
                return category.Wizards.ToArray();
            }
        }

        /// <summary> Provides an implementation of <see cref="ITreeLabelProvider"/>. </summary>
        class NewWizardTreeLabelProvider : ITreeLabelProvider {

            private static readonly ImageDescriptor FolderImageDescriptor = ImageDescriptor.Create("image.folder", "resources/images/Folder-32.png");

            /// <inheritdoc />
            public string GetText(object item) {
                Contribution contribution = item as Contribution;
                if (contribution == null) return item.ToString();
                return contribution.Label;
            }

            /// <inheritdoc />
            public ImageDescriptor GetImage(object item) {
                if (item is CategoryContribution) return FolderImageDescriptor;
                if (item is WizardContribution wizardContribution) return wizardContribution.Image;
                return null;
            }

            /// <inheritdoc />
            public bool IsEnabled(object item) {
                return true;
            }

            /// <inheritdoc />
            public string GetToolTipText(object item) {
                return null;
            }
        }
    }
}