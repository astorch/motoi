using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using motoi.platform.commons;
using motoi.platform.ui.data;
using motoi.platform.ui.factories;
using motoi.platform.ui.images;
using motoi.platform.ui.widgets;
using motoi.workbench.events;
using motoi.workbench.model;
using motoi.workbench.runtime;
using Xcite.Csharp.lang;

namespace motoi.workbench.problemsview {
    /// <summary>
    /// Provides a view of problems that occurred within an opened editor.
    /// </summary>
    public class ProblemsDataView : AbstractDataView, IPerspectiveListener, IMessageConsumer {
        /// <summary> Data view id of this class. </summary>
        // ReSharper disable once UnusedMember.Global
        public const string Id = "motoi.workbench.problemsview.problemsDataView";

        private readonly ProblemsViewItemCollection iErrorCollection = new ProblemsViewItemCollection(EProblemsViewItemType.Error);
        private readonly ProblemsViewItemCollection iWarningCollection = new ProblemsViewItemCollection(EProblemsViewItemType.Warning);
        private readonly ProblemsViewItemCollection iInfoCollection = new ProblemsViewItemCollection(EProblemsViewItemType.Info);

        private readonly AutoLockStruct<bool> iDataChanged = new AutoLockStruct<bool>();
        private Timer iTimer; // TODO Dispose

        /// <inheritdoc />
        public override IWidgetFactory WidgetFactory { get; set; }

        /// <inheritdoc />
        public override void CreateContents(IGridPanel gridComposite) {
            gridComposite.GridColumns = 1;
            gridComposite.GridRows = 1;

            IListViewer listViewer = WidgetFactory.CreateInstance<IListViewer>(gridComposite);
            gridComposite.AddWidget(listViewer);

            listViewer.ContentProvider = new ProblemsViewListContentProvider();
            listViewer.LabelProvider = new ProblemsViewListLabelProvider();
            listViewer.Input = new[] {iErrorCollection, iWarningCollection, iInfoCollection};
            listViewer.Update();

            iTimer = new Timer(state => {
                lock (iDataChanged) {
                    if (!iDataChanged.Get()) return;
                    iDataChanged.Set(false);
                }
                PlatformUI.Instance.Invoker.InvokeAsync(() => listViewer.Update());
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        /// <inheritdoc />
        public override void Init() {
            PlatformUI.Instance.Workbench.ActivePerspective.AddPerspectiveListener(this); // TODO Remove listener
            MessageDispatcher.Instance.Subscribe(this);
        }

        /// <inheritdoc />
        public override string Name { get { return "Problems"; } }

        /// <inheritdoc />
        public override ImageDescriptor Image { get { return null; } } // TODO Add image

        #region IPerspectiveListener

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been closed.
        /// </summary>
        /// <param name="workbenchPart">Closed workbench part</param>
        public void OnWorkbenchPartClosed(IWorkbenchPart workbenchPart) {
            IProblemsViewItemProvider itemProvider = workbenchPart as IProblemsViewItemProvider;
            if (itemProvider == null) return;
            itemProvider.ItemsFound -= OnItemProviderItemsFound;
        }

        /// <summary>
        /// Tells the instance that the given <paramref name="workbenchPart"/> has been opened.
        /// </summary>
        /// <param name="workbenchPart">Opened workbench part</param>
        public void OnWorkbenchPartOpened(IWorkbenchPart workbenchPart) {
            IProblemsViewItemProvider itemProvider = workbenchPart as IProblemsViewItemProvider;
            if (itemProvider == null) return;
            itemProvider.ItemsFound += OnItemProviderItemsFound;
        }

        /// <summary>
        /// Is invoked when an observed <see cref="IProblemsViewItemProvider"/> raised an event 
        /// that he has been found some items.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="eventArgs">Event arguments</param>
        private void OnItemProviderItemsFound(object sender, ItemsFoundEventArgs eventArgs) {
            if (eventArgs == null) return;
            IEnumerable<ProblemsViewItem> items = eventArgs.Items;
            if (items == null) return;

            iErrorCollection.Clear();
            iWarningCollection.Clear();
            iInfoCollection.Clear();

            for (IEnumerator<ProblemsViewItem> itr = items.GetEnumerator(); itr.MoveNext();) {
                ProblemsViewItem item = itr.Current;
                ProblemsViewItemCollection collection = iErrorCollection;

                if (item.ItemType == EProblemsViewItemType.Warning)
                    collection = iWarningCollection;
                else if (item.ItemType == EProblemsViewItemType.Info)
                    collection = iInfoCollection;

                collection.Add(item);
            }

            iDataChanged.Set(true);
        }

        #endregion

        #region IMessageConsumer

        /// <inheritdoc />
        public void OnMessageReceived(object eventSource, object eventArguments, object dataObject) {
            if (!(eventArguments is IEnumerable<ProblemsViewItem>)) return;
            ProblemsViewItem[] items = ((IEnumerable<ProblemsViewItem>) eventArguments).ToArray();
            OnItemProviderItemsFound(eventSource, new ItemsFoundEventArgs(items));
        }

        #endregion

        /// <summary>
        /// Provides an implementation of <see cref="IListContentProvider"/> for the Problems Data View.
        /// </summary>
        class ProblemsViewListContentProvider : IListContentProvider {
            public static readonly ColumnDescriptor DescriptionColumn = new ColumnDescriptor("Description", 600);
            public static readonly ColumnDescriptor FileColumn = new ColumnDescriptor("File", 120);
            public static readonly ColumnDescriptor LineColumn = new ColumnDescriptor("Line", 80);
            public static readonly ColumnDescriptor ColumnColumn = new ColumnDescriptor("Column", 80);

            /// <inheritdoc />
            public object[] GetElements(object input) {
                return (object[]) input;
            }

            /// <inheritdoc />
            public ColumnDescriptor[] Columns {
                get { return new[] { DescriptionColumn, FileColumn, LineColumn, ColumnColumn }; }
            }

            /// <inheritdoc />
            public bool HasChildren(object item) {
                return (item is ProblemsViewItemCollection);
            }

            /// <inheritdoc />
            public object[] GetChildren(object item) {
                ProblemsViewItemCollection itemCollection = item as ProblemsViewItemCollection;
                if (itemCollection == null) return new object[0];
                return Enumerable.ToArray(itemCollection);
            }
        }

        /// <summary>
        /// Provides an implementation of <see cref="IListLabelProvider"/> for the Problems Data View.
        /// </summary>
        class ProblemsViewListLabelProvider : IListLabelProvider {
            /// <inheritdoc />
            public string GetText(object item) {
                ProblemsViewItemCollection itemCollection = item as ProblemsViewItemCollection;
                if (itemCollection == null) return null;
                return itemCollection.Kind.ToString();
            }

            /// <inheritdoc />
            public ImageDescriptor GetImage(object item) {
                return null;
            }

            /// <inheritdoc />
            public string GetText(object element, ColumnDescriptor column) {
                ProblemsViewItem item = element as ProblemsViewItem;
                if (item == null) return null;
                if (column == ProblemsViewListContentProvider.DescriptionColumn) return item.Description;
                if (column == ProblemsViewListContentProvider.LineColumn) return item.Line.ToString();
                if (column == ProblemsViewListContentProvider.ColumnColumn) return item.Column.ToString();
                if (column == ProblemsViewListContentProvider.FileColumn) {
                    Uri location = item.WorkspaceArtefact.Location;
                    return (location == null ? string.Empty : location.LocalPath.Split('\\').Last());
                }

                return null;
            }

            /// <inheritdoc />
            public ImageDescriptor GetImage(object element, ColumnDescriptor column) {
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