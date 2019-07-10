using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using motoi.platform.ui.data;
using motoi.platform.ui.factories;
using motoi.platform.ui.images;
using motoi.platform.ui.widgets;
using motoi.workbench.events;
using motoi.workbench.model;
using motoi.workbench.runtime;
using xcite.csharp;
using xcite.messaging;

namespace motoi.workbench.problemsview {
    /// <summary> Implements a view of problems that occurred within an opened editor. </summary>
    public class ProblemsDataView : AbstractDataView, IPerspectiveListener, IMessageConsumer {
        /// <summary> Data view id of this class. </summary>
        // ReSharper disable once UnusedMember.Global
        public const string Id = "motoi.workbench.problemsview.problemsDataView";

        private readonly ProblemsViewItemCollection _errorCollection = new ProblemsViewItemCollection(EProblemsViewItemType.Error);
        private readonly ProblemsViewItemCollection _warningCollection = new ProblemsViewItemCollection(EProblemsViewItemType.Warning);
        private readonly ProblemsViewItemCollection _infoCollection = new ProblemsViewItemCollection(EProblemsViewItemType.Info);

        private readonly AutoLockStruct<bool> _dataChanged = new AutoLockStruct<bool>();
        private Timer _timer; // TODO Dispose

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
            listViewer.Input = new[] {_errorCollection, _warningCollection, _infoCollection};
            listViewer.Update();

            _timer = new Timer(state => {
                lock (_dataChanged) {
                    if (!_dataChanged.Get()) return;
                    _dataChanged.Set(false);
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
        public override string Name 
            => Messages.ProblemsDataView_Name;

        /// <inheritdoc />
        public override ImageDescriptor Image 
            => null; // TODO Add image

        #region IPerspectiveListener

        /// <inheritdoc />
        public void OnWorkbenchPartClosed(IWorkbenchPart workbenchPart) {
            IProblemsViewItemProvider itemProvider = workbenchPart as IProblemsViewItemProvider;
            if (itemProvider == null) return;
            itemProvider.ItemsFound -= OnItemProviderItemsFound;
        }

        /// <inheritdoc />
        public void OnWorkbenchPartOpened(IWorkbenchPart workbenchPart) {
            IProblemsViewItemProvider itemProvider = workbenchPart as IProblemsViewItemProvider;
            if (itemProvider == null) return;
            itemProvider.ItemsFound += OnItemProviderItemsFound;
        }

        /// <inheritdoc />
        public void OnWorkbenchPartActivated(IWorkbenchPart workbenchPart) {
            // Nothing to do
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

            _errorCollection.Clear();
            _warningCollection.Clear();
            _infoCollection.Clear();

            for (IEnumerator<ProblemsViewItem> itr = items.GetEnumerator(); itr.MoveNext();) {
                ProblemsViewItem item = itr.Current;
                ProblemsViewItemCollection collection = _errorCollection;

                if (item.ItemType == EProblemsViewItemType.Warning)
                    collection = _warningCollection;
                else if (item.ItemType == EProblemsViewItemType.Info)
                    collection = _infoCollection;

                collection.Add(item);
            }

            _dataChanged.Set(true);
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
            public static readonly ColumnDescriptor DescriptionColumn = new ColumnDescriptor(Messages.ProblemsDataView_ColumnDescription_Name, 600);
            public static readonly ColumnDescriptor FileColumn = new ColumnDescriptor(Messages.ProblemsDataView_ColumnFile_Name, 120);
            public static readonly ColumnDescriptor LineColumn = new ColumnDescriptor(Messages.ProblemsDataView_ColumnLine_Name, 80);
            public static readonly ColumnDescriptor ColumnColumn = new ColumnDescriptor(Messages.ProblemsDataView_ColumnColumn_Name, 80);

            /// <inheritdoc />
            public object[] GetElements(object input) {
                return (object[]) input;
            }

            /// <inheritdoc />
            public ColumnDescriptor[] Columns 
                => new[] { DescriptionColumn, FileColumn, LineColumn, ColumnColumn };

            /// <inheritdoc />
            public bool HasChildren(object item) {
                return (item is ProblemsViewItemCollection);
            }

            /// <inheritdoc />
            public object[] GetChildren(object item) {
                ProblemsViewItemCollection itemCollection = item as ProblemsViewItemCollection;
                if (itemCollection == null) return new object[0];
                return itemCollection.ToArray();
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

                EProblemsViewItemType itemType = itemCollection.Kind;
                if (itemType == EProblemsViewItemType.Error) return Messages.ProblemsViewItemType_Error;
                if (itemType == EProblemsViewItemType.Warning) return Messages.ProblemsViewItemType_Warning;
                if (itemType == EProblemsViewItemType.Info) return Messages.ProblemsViewItemType_Info;
                
                return itemType.ToString();
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