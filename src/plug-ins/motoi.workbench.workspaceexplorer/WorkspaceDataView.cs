using System;
using System.IO;
using motoi.platform.resources;
using motoi.platform.resources.model;
using motoi.platform.resources.runtime.editors;
using motoi.platform.ui.data;
using motoi.platform.ui.factories;
using motoi.platform.ui.images;
using motoi.platform.ui.widgets;
using motoi.workbench.model;
using motoi.workbench.runtime;
using Xcite.Collections;

namespace motoi.workbench.workspaceexplorer {
    /// <summary>
    /// Provides a view of the current workspace content.
    /// </summary>
    public class WorkspaceDataView : AbstractWorkbenchPart, IDataView {
        /// <summary>
        /// Data view id of this class.
        /// </summary>
        public const string Id = "motoi.workbench.workspaceexplorer.workspaceDataView";

        private static readonly ImageDescriptor DataViewImage = ImageDescriptor.Create("images.explorer", "resources/images/Explorer-32.png");
        
        private IWorkspace iWorkspaceReference;
        
        /// <summary>
        /// Returns the widget factory that can be used to create widgets or does set it.
        /// </summary>
        public override IWidgetFactory WidgetFactory { get; set; }

        /// <summary>
        /// Tells the editor to create its content using the given widget factory.
        /// </summary>
        /// <param name="gridComposite">Panel to place the content widgets of the editor</param>
        public override void CreateContents(IGridPanel gridComposite) {
            gridComposite.GridColumns = 1;
            gridComposite.GridRows = 1;

            ITreeViewer treeViewer = WidgetFactory.CreateInstance<ITreeViewer>(gridComposite);
            gridComposite.AddWidget(treeViewer);

            treeViewer.SelectionDoubleClicked += OnSelectionDoubleClicked;
            treeViewer.ContentProvider = new WorkspaceTreeContentProviderImpl();
            treeViewer.LabelProvider = new WorkspaceTreeLabelProviderImpl();
            treeViewer.Input = iWorkspaceReference;
            treeViewer.Update();

            iWorkspaceReference.Refreshed += (sender, args) => treeViewer.Update();
        }

        /// <summary>
        /// Is invoked when the user performs a double click on a workspace artefact.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="selectionEventArgs">Event arguments</param>
        private void OnSelectionDoubleClicked(object sender, SelectionEventArgs selectionEventArgs) {
            IWorkspaceFile workspaceFile = selectionEventArgs.Selection as IWorkspaceFile;
            if (workspaceFile == null) return;
            
            // TODO Add support of non file system files
            Uri fileLocation = workspaceFile.Location;
            string absolutePath = fileLocation.AbsolutePath;

            FileEditorInput fileEditorInput = new FileEditorInput(new FileInfo(absolutePath)); 
            PlatformUI.Instance.Workbench.ActivePerspective.OpenEditor(fileEditorInput);
        }

        /// <summary>
        /// Tells the instance to initialize its state.
        /// </summary>
        public void Init() {
            iWorkspaceReference = ResourceService.Instance.Workspace;
        }

        /// <summary>
        /// Returns the name of the data view.
        /// </summary>
        public string Name { get { return Messages.WorkspaceDataView_Name; } }

        /// <summary>
        /// Returns the image of the data view.
        /// </summary>
        public ImageDescriptor Image { get { return DataViewImage; } }

        /// <summary>
        /// Provides an implementation of <see cref="ITreeLabelProvider"/> for <see cref="IWorkspace"/> data.
        /// </summary>
        class WorkspaceTreeLabelProviderImpl : ITreeLabelProvider {
            private static readonly ImageDescriptor FolderImageDescriptor = ImageDescriptor.Create("images.folder", "resources/images/Folder-32.png");
            private static readonly ImageDescriptor FileImageDescriptor = ImageDescriptor.Create("images.file", "resources/images/File-32.png");

            /// <inheritdoc />
            public string GetText(object item) {
                IWorkspaceArtefact workspaceArtefact = (IWorkspaceArtefact) item;
                return workspaceArtefact.Name;
            }

            /// <inheritdoc />
            public bool IsEnabled(object item) {
                return true;
            }

            /// <inheritdoc />
            public ImageDescriptor GetImage(object item) {
                IWorkspaceArtefact workspaceArtefact = (IWorkspaceArtefact) item;
                string artefactNature = workspaceArtefact.Nature; // TODO Add support of artefact nature

                if (workspaceArtefact is IWorkspaceArtefactContainer) return FolderImageDescriptor;
                return FileImageDescriptor;
            }

            /// <inheritdoc />
            public string GetToolTipText(object item) {
                return null;
            }
        }

        /// <summary>
        /// Provides an implementation of <see cref="ITreeContentProvider"/> for <see cref="IWorkspace"/> data.
        /// </summary>
        class WorkspaceTreeContentProviderImpl : ITreeContentProvider {
            /// <inheritdoc />
            public object[] GetElements(object input) {
                IWorkspace workspace = (IWorkspace) input;
                return workspace.Artefacts.ToArray();
            }

            /// <inheritdoc />
            public bool HasChildren(object item) {
                IWorkspaceArtefact workspaceArtefact = (IWorkspaceArtefact) item;
                if (workspaceArtefact is IWorkspaceArtefactContainer) return true;
                return false;
            }

            /// <inheritdoc />
            public object[] GetChildren(object item) {
                IWorkspaceArtefactContainer container = (IWorkspaceArtefactContainer) item;
                return container.Artefacts.ToArray();
            }
        }
    }
}
