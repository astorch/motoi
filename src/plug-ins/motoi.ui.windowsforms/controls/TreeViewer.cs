using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using motoi.platform.ui.data;
using motoi.platform.ui.images;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="ITreeViewer"/>.
    /// </summary>
    public class TreeViewer : TreeView, ITreeViewer {
        private readonly Dictionary<object, TreeNode> iDataObjectTreeNodeMap = new Dictionary<object, TreeNode>(100);
        private object iLastSelectedDataObject;
        private object iLastExpandedDataObject;

        /// <summary>
        /// The handler will be notified when a new selection has been made.
        /// </summary>
        public event EventHandler<SelectionEventArgs> SelectionChanged;

        /// <summary>
        /// The handler will be notified when a selection has been made using a double click.
        /// </summary>
        public event EventHandler<SelectionEventArgs> SelectionDoubleClicked;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public TreeViewer() {
            OnInitialized();
        }

        /// <summary>
        /// Tells the instance to initialize its content.
        /// </summary>
        private void OnInitialized() {
            ContentProvider = null;
            LabelProvider = DefaultTreeLabelProvider.Instance;
            ImageList = new ImageList();

            // see also http://stackoverflow.com/questions/10034714/c-sharp-winforms-highlight-treenode-when-treeview-doesnt-have-focus
            HideSelection = false;
            ShowNodeToolTips = true;
            ShowRootLines = true;
            ShowLines = true;
            ShowPlusMinus = true;
        }

        /// <summary>
        /// Löst das <see cref="E:System.Windows.Forms.TreeView.BeforeSelect"/>-Ereignis aus.
        /// </summary>
        /// <param name="e">Eine Instanz der <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs"/>-Klasse, die die Ereignisdaten enthält. </param>
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e) {
            base.OnBeforeSelect(e);
            
            CustomTreeNode node = e.Node as CustomTreeNode;
            if (node == null) return;

            if (!node.IsEnabled)
                e.Cancel = true;
        }

        /// <inheritdoc />
        public ContextMenuItemProvider ContextMenuItemProvider { get; set; }

        /// <summary>
        /// Löst das <see cref="E:System.Windows.Forms.TreeView.AfterSelect"/>-Ereignis aus.
        /// </summary>
        /// <param name="e">Eine Instanz der <see cref="T:System.Windows.Forms.TreeViewEventArgs"/>-Klasse, die die Ereignisdaten enthält. </param>
        protected override void OnAfterSelect(TreeViewEventArgs e) {
            base.OnAfterSelect(e);
            iLastSelectedDataObject = e.Node.Tag;

            // TODO Add try-catch

            if (SelectionChanged != null)
                SelectionChanged(this, new SelectionEventArgs { Selection = SelectedNode.Tag } );

            if (ContextMenuItemProvider != null) {
                IContextMenuItem[] items = ContextMenuItemProvider(SelectedNode.Tag);

                ContextMenuStrip ctxMenuStrip = items == null ? null : new ContextMenuStrip();
                SelectedNode.ContextMenuStrip = ctxMenuStrip;

                if (items != null) {
                    for (int i = -1; ++i != items.Length; ) {
                        IContextMenuItem item = items[i];
                        ctxMenuStrip.Items.Add(new ToolStripMenuItem(item.Name, null, (sender, args) => item.Action(SelectedNode.Tag)));
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseDoubleClick"/> event. 
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"/> that contains the event data. </param>
        protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e) {
            base.OnNodeMouseDoubleClick(e);

            // TODO Add try-catch

            if (SelectionDoubleClicked != null && SelectedNode != null)
                SelectionDoubleClicked(this, new SelectionEventArgs {Selection = SelectedNode.Tag});
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.AfterExpand" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
        protected override void OnAfterExpand(TreeViewEventArgs e) {
            base.OnAfterExpand(e);
            iLastExpandedDataObject = e.Node.Tag;
        }

        /// <summary>
        /// Returns the input of the data viewer or does set it.
        /// </summary>
        public object Input { get; set; }

        /// <summary>
        /// Returns the currently used content provider or does set it.
        /// </summary>
        public ITreeContentProvider ContentProvider { get; set; }

        /// <summary>
        /// Returns the currently used label provider or does set it.
        /// </summary>
        public ITreeLabelProvider LabelProvider { get; set; }

        /// <summary>
        /// Fully re-creates the tree. Structural and visual properties will be updated.
        /// </summary>
        void IDataViewer<ITreeContentProvider, ITreeLabelProvider>.Update() {
            if (ContentProvider == null) throw new NullReferenceException("ContentProvider is null!");
            if (LabelProvider == null) throw new NullReferenceException("LabelProvider is null!");

            try {
                BeginUpdate();
                
                Nodes.Clear();
                iDataObjectTreeNodeMap.Clear();

                object[] items = ContentProvider.GetElements(Input);
                CreateTree(items, Nodes);
                ((IDataViewer<ITreeContentProvider, ITreeLabelProvider>) this).Refresh();

                TryReenterPreviousState();
            } finally {
                EndUpdate();
            }
        }

        /// <summary>
        /// Recursive method to create the tree based on the given elements. All items will 
        /// be added to the given collection.
        /// </summary>
        /// <param name="elements">Elements to add</param>
        /// <param name="collection">Collection that will get the new items</param>
        private void CreateTree(object[] elements, TreeNodeCollection collection) {
            for (int i = -1; ++i < elements.Length; ) {
                object element = elements[i];
                CustomTreeNode treeNode = new CustomTreeNode();
                treeNode.Tag = element;
                iDataObjectTreeNodeMap.Add(element, treeNode);
                StyleNode(treeNode);
                collection.Add(treeNode);

                if (ContentProvider.HasChildren(element)) {
                    object[] elementChildrens = ContentProvider.GetChildren(element);
                    CreateTree(elementChildrens, treeNode.Nodes);
                }
            }
        }

        /// <summary>
        /// Refreshs the tree. Only visual properties will be updated.
        /// </summary>
        void IDataViewer<ITreeContentProvider, ITreeLabelProvider>.Refresh() {
            if (LabelProvider == null) throw new NullReferenceException("LabelProvider is null!");
            StyleTree(Nodes);
        }

        /// <summary>
        /// Styles all nodes of the given collection using the current Label Provider.
        /// </summary>
        /// <param name="nodes">Collection of nodes to style</param>
        private void StyleTree(TreeNodeCollection nodes) {
            for (IEnumerator enmtor = nodes.GetEnumerator(); enmtor.MoveNext(); ) {
                CustomTreeNode treeNode = enmtor.Current as CustomTreeNode;
                if (treeNode == null) continue;
                StyleNode(treeNode);
                StyleTree(treeNode.Nodes);
            }
        }

        /// <summary>
        /// Styles the given node using the current Label Provider.
        /// </summary>
        /// <param name="treeNode">Node to style</param>
        private void StyleNode(CustomTreeNode treeNode) {
            object element = treeNode.Tag;
            
            // Apply standard properties
            treeNode.Text = LabelProvider.GetText(element);
            treeNode.IsEnabled = LabelProvider.IsEnabled(element);
            treeNode.ToolTipText = LabelProvider.GetToolTipText(element) ?? string.Empty;

            // Apply image
            ImageDescriptor imageDescriptor = LabelProvider.GetImage(element);
            if (imageDescriptor != null) {
                string imageId = imageDescriptor.Id;
                treeNode.ImageKey = imageId;
                treeNode.SelectedImageKey = imageId;

                if (!ImageList.Images.ContainsKey(imageId)) {
                    Stream imageStream = imageDescriptor.ImageStream;
                    if (imageStream != null) {
                        ImageList.Images.Add(imageId, new Bitmap(imageStream));
                    }
                }
            }
        }

        /// <summary>
        /// Tries to re-expand all nodes that have been expanded at last. Additionally, the last selected item 
        /// is re-selected, too.
        /// </summary>
        private void TryReenterPreviousState() {
            if (iLastExpandedDataObject != null) {
                TreeNode treeNode;
                if (iDataObjectTreeNodeMap.TryGetValue(iLastExpandedDataObject, out treeNode))
                    treeNode.Expand();
            }

            if (iLastSelectedDataObject != null) {
                TreeNode treeNode;
                if (iDataObjectTreeNodeMap.TryGetValue(iLastSelectedDataObject, out treeNode))
                    SelectedNode = treeNode;
                else
                    SelectedNode = null;
            }
        }

        /// <summary>
        /// Extends <see cref="TreeNode"/> to add additional behaviours.
        /// </summary>
        class CustomTreeNode : TreeNode {
            private Color iForeColor = Color.Transparent;
            private bool iIsEnabled = true;

            /// <summary>
            /// Returns true if the node is enabled or does set it.
            /// </summary>
            public bool IsEnabled {
                get { return iIsEnabled; }
                set {
                    iIsEnabled = value;
                    CheckLayout();
                }
            }

            /// <summary>
            /// Updates the layout of the tree node.
            /// </summary>
            private void CheckLayout() {
                if (!IsEnabled) {
                    iForeColor = ForeColor;
                    ForeColor = SystemColors.GrayText;
                } else {
                    if (iForeColor != Color.Transparent)
                        ForeColor = iForeColor;
                }
            }
        }
    }
}