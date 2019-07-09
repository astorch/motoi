using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.data;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="IListViewer"/>.
    /// </summary>
    public class ListViewer : ListView, IListViewer {
        private IListContentProvider iContentProvider;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ListViewer() {
            InitializeComponent();
        }

        #region IListViewer

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PListViewer.GetModelValue(this, PListViewer.VisibilityProperty); }
            set {
                PListViewer.SetModelValue(this, PListViewer.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            }
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PListViewer.GetModelValue(this, PListViewer.EnabledProperty); }
            set {
                PListViewer.SetModelValue(this, PListViewer.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <summary>
        /// The handler will be notified when a new selection has been made.
        /// </summary>
        public event EventHandler<SelectionEventArgs> SelectionChanged;

        /// <summary>
        /// The handler will be notified when a selection has been made using a double click.
        /// </summary>
        public event EventHandler<SelectionEventArgs> SelectionDoubleClicked;

        /// <summary>
        /// Returns the input of the data viewer or does set it.
        /// </summary>
        public object Input { get; set; }

        /// <summary>
        /// Returns the currently used content provider or does set it.
        /// </summary>
        public IListContentProvider ContentProvider {
            get { return iContentProvider; }
            set {
                iContentProvider = value;
                OnContentProviderChanged(value);
            }
        }

        /// <summary>
        /// Returns the currently used label provider or does set it.
        /// </summary>
        public IListLabelProvider LabelProvider { get; set; }

        /// <summary>
        /// Fully re-creates the tree. Structural and visual properties will be updated.
        /// </summary>
        void IDataViewer<IListContentProvider, IListLabelProvider>.Update() {
            if (ContentProvider == null) throw new NullReferenceException("ContentProvider is null!");
            if (LabelProvider == null) throw new NullReferenceException("LabelProvider is null!");

            try {
                BeginUpdate();

                // Create groups and items
                object[] elements = ContentProvider.GetElements(Input);
                CreateGroupsAndItems(elements);

                
                // TODO Move Refresh into region
            } finally {
                EndUpdate();
            }

            // Refresh visual
            ((IDataViewer<IListContentProvider, IListLabelProvider>)this).Refresh();
        }

        /// <summary>
        /// Refreshs the tree. Only visual properties will be updated.
        /// </summary>
        void IDataViewer<IListContentProvider, IListLabelProvider>.Refresh() {
            if (LabelProvider == null) throw new NullReferenceException("LabelProvider is null!");

            // Style groups
            for (IEnumerator grpItr = Groups.GetEnumerator(); grpItr.MoveNext();) {
                ListViewGroup group = grpItr.Current as ListViewGroup;
                if (group == null) continue;
                object element = group.Tag;
                
                group.Header = LabelProvider.GetText(element);
                StyleListViewItems(group.Items);
            }

            // Style items
            StyleListViewItems(Items);
        }

        #endregion

        /// <summary>
        /// Is invoked when the content provider has been changed.
        /// </summary>
        /// <param name="contentProvider">New content provider</param>
        protected virtual void OnContentProviderChanged(IListContentProvider contentProvider) {
            ColumnDescriptor[] columns = contentProvider == null ? null : contentProvider.Columns;
            CreateColumns(columns);
        }

        /// <summary>
        /// Styles all list view items of the given <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Collection of items to style</param>
        private void StyleListViewItems(ListViewItemCollection collection) {
            for (IEnumerator itr = collection.GetEnumerator(); itr.MoveNext(); ) {
                ListViewItem item = itr.Current as ListViewItem;
                if (item == null) continue;

                object element = item.Tag;
                for (int i = -1; ++i != Columns.Count; ) {
                    ColumnHeader column = Columns[i];
                    ColumnDescriptor columnDescriptor = column.Tag as ColumnDescriptor;
                    string text = LabelProvider.GetText(element, columnDescriptor);
            
                    if (i == 0)
                        item.Text = text;
                    else
                        item.SubItems.Add(text);
                }
            }
        }

        /// <summary>
        /// Creates list view items and groups for the given data <paramref name="elements"/>.
        /// </summary>
        /// <param name="elements">Data element collection</param>
        private void CreateGroupsAndItems(object[] elements) {
            Items.Clear();
//            Groups.Clear();

            for (int i = -1; ++i != elements.Length;) {
                object element = elements[i];
                bool hasChildren = ContentProvider.HasChildren(element);
                if (hasChildren) {
                    object[] children = ContentProvider.GetChildren(element);

                    // Don't create empty groups
                    if (children == null || children.Length == 0) continue;
                    
                    // Try to re-use existing groups
                    ListViewGroup group = Groups.Cast<ListViewGroup>().FirstOrDefault(grp => Equals(grp.Tag, element));
                    if (group == null) {
                        group = new ListViewGroup {Tag = element};
                        Groups.Add(group);
                    }

                    for (int j = -1; ++j != children.Length;) {
                        CreateListViewItem(children[j], group);
                    }
                } else {
                    CreateListViewItem(element, null);
                }
            }
        }

        /// <summary>
        /// Creates a new list view item for the given <paramref name="element"/>. If the given <paramref name="group"/> 
        /// is not NULL, the created item will be added to this.
        /// </summary>
        /// <param name="element">Data object to create a list view item for</param>
        /// <param name="group">Group for the item</param>
        private void CreateListViewItem(object element, ListViewGroup group) {
            ListViewItem item = group == null ? new ListViewItem() : new ListViewItem(group);
            item.Tag = element;
            Items.Add(item);
        }

        /// <summary>
        /// Creates all columns for the viewer based on the given <paramref name="columns"/> descriptor.
        /// </summary>
        /// <param name="columns">Column descriptor collection</param>
        private void CreateColumns(ColumnDescriptor[] columns) {
            Columns.Clear();
            if (columns == null || columns.Length == 0) return;

            for (int i = -1; ++i != columns.Length;) {
                ColumnDescriptor columnDescriptor = columns[i];

                ColumnHeader columnHeader = new ColumnHeader {
                    Text = columnDescriptor.HeaderText,
                    Width = (int)columnDescriptor.DefaultWidth,
                    TextAlign = MapTextAlign(columnDescriptor.Orientation),
                    Tag = columnDescriptor
                };

                Columns.Add(columnHeader);
            }
        }

        /// <summary>
        /// Returns a corresponding <see cref="HorizontalAlignment"/> to the given <paramref name="orientation"/>.
        /// </summary>
        /// <param name="orientation">Orientation to map</param>
        /// <returns>Corresponding <see cref="HorizontalAlignment"/></returns>
        private HorizontalAlignment MapTextAlign(EColumnTextOrientation orientation) {
            if (orientation == EColumnTextOrientation.Left) return HorizontalAlignment.Left;
            if (orientation == EColumnTextOrientation.Right) return HorizontalAlignment.Right;
            return HorizontalAlignment.Center;
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDoubleClick" /> event.</summary>
        /// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);

            if (SelectedItems.Count == 0) return;
            if (SelectionDoubleClicked == null) return;

            object selectedItem = SelectedItems[0].Tag;
            SelectionDoubleClicked(this, new SelectionEventArgs {Selection = selectedItem});
        }

        /// <inheritdoc />
        protected override void OnSelectedIndexChanged(EventArgs e) {
            base.OnSelectedIndexChanged(e);

            if (SelectedItems.Count == 0) return;
            if (SelectionChanged == null) return;

            object selectedItem = SelectedItems[0].Tag;
            SelectionChanged(this, new SelectionEventArgs { Selection = selectedItem });
        }

        /// <summary>
        /// Notifies the instance to initialize its content.
        /// </summary>
        private void InitializeComponent() {
            DoubleBuffered = true; // TODO Check if it has any advantages
            View = View.Details;
            FullRowSelect = true;
        }
    }
}