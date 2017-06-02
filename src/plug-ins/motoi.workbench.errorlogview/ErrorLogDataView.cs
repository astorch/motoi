using System.Collections.Generic;
using System.Linq;
using motoi.platform.commons;
using motoi.platform.ui.data;
using motoi.platform.ui.factories;
using motoi.platform.ui.images;
using motoi.platform.ui.widgets;
using motoi.workbench.runtime;

namespace motoi.workbench.errorlogview {
    /// <summary> Implements a data view that shows all logged entries of the <see cref="PlatformErrorLog"/>. </summary>
    public class ErrorLogDataView : AbstractDataView {
        private readonly LinkedList<ErrorLogEntry> fErrorViewItemSet = new LinkedList<ErrorLogEntry>();
        private IListViewer fListViewer;

        /// <inheritdoc />
        public override IWidgetFactory WidgetFactory { get; set; }

        /// <inheritdoc />
        public override void Init() {
            PlatformErrorLog.Instance.Added += OnPlatformErrorLogEntryAdded;
        }

        /// <summary>
        /// Is invoked when the an error log entry has been added to the <see cref="PlatformErrorLog"/>.
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="errorLogEntry">Event arguments</param>
        private void OnPlatformErrorLogEntryAdded(object sender, ErrorLogEntry errorLogEntry) {
            fErrorViewItemSet.AddFirst(errorLogEntry);
            PlatformUI.Instance.Invoker.InvokeAsync(lstVwr => lstVwr.Update(), fListViewer);
        }

        /// <inheritdoc />
        public override void CreateContents(IGridPanel gridComposite) {
            gridComposite.GridColumns = 1;
            gridComposite.GridRows = 1;

            fListViewer = WidgetFactory.CreateInstance<IListViewer>(gridComposite);
            gridComposite.AddWidget(fListViewer);

            fListViewer.ContentProvider = new ErrorsViewListContentProvider();
            fListViewer.LabelProvider = new ErrorsViewListLabelProvider();
            fListViewer.Input = fErrorViewItemSet;
            fListViewer.SelectionDoubleClicked += OnSelectionDoubleClicked;
            fListViewer.Update();
        }

        /// <summary>
        /// Is invoked when a list viewer item has been double clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="selectionEventArgs">Event arguments</param>
        private void OnSelectionDoubleClicked(object sender, SelectionEventArgs selectionEventArgs) {
            ErrorLogEntry entry = selectionEventArgs.Selection as ErrorLogEntry;
            if (entry == null) return;
            MessageDialog.ShowException(entry.Exception, entry.Message);
        }

        /// <inheritdoc />
        public override string Name { get { return Messages.ErrorLogDataView_Name; } }

        /// <inheritdoc />
        public override ImageDescriptor Image { get { return ImageDescriptor.Create("images.error-file", "resources/images/File-Warning-32.png"); } }

        /// <summary>
        /// Implements the <see cref="IListLabelProvider"/> for the <see cref="ErrorLogDataView"/>.
        /// </summary>
        class ErrorsViewListLabelProvider : IListLabelProvider {
            private static readonly ImageDescriptor ErrorImage = ImageDescriptor.Create("images.error", "resources/images/ErrorCircle-32.png");
            private static readonly ImageDescriptor WarningImage = ImageDescriptor.Create("images.warning", "resources/images/Warning-32.png");

            /// <inheritdoc />
            public string GetText(object item) {
                return null;
            }

            /// <inheritdoc />
            public ImageDescriptor GetImage(object item) {
                return null;
            }

            /// <inheritdoc />
            public string GetToolTipText(object item) {
                return null;
            }

            /// <inheritdoc />
            public bool IsEnabled(object item) {
                return true;
            }

            /// <inheritdoc />
            public string GetText(object element, ColumnDescriptor column) {
                ErrorLogEntry item = (ErrorLogEntry) element;

                if (column == ErrorsViewListContentProvider.MessageColumn) return item.Message;
                if (column == ErrorsViewListContentProvider.PluginColumn)  return item.PluginName;
                if (column == ErrorsViewListContentProvider.DateColumn) return item.Timestamp.ToString();

                return null;
            }

            /// <inheritdoc />
            public ImageDescriptor GetImage(object element, ColumnDescriptor column) {
                if (column != ErrorsViewListContentProvider.MessageColumn) return null;
                ErrorLogEntry item = (ErrorLogEntry)element;
                return item.LogEntryType == ELogEntryType.Error
                    ? ErrorImage
                    : WarningImage;
            }
        }

        /// <summary>
        /// Implements the <see cref="IListContentProvider"/> for the <see cref="ErrorLogDataView"/>.
        /// </summary>
        class ErrorsViewListContentProvider : IListContentProvider {
            public static readonly ColumnDescriptor MessageColumn = new ColumnDescriptor(Messages.ErrorLogDataView_ColumnMessage_Name, 600);
            public static readonly ColumnDescriptor PluginColumn = new ColumnDescriptor(Messages.ErrorLogDataView_ColumnPlugin_Name, 250);
            public static readonly ColumnDescriptor DateColumn = new ColumnDescriptor(Messages.ErrorLogDataView_ColumnDate_Name, 250);

            /// <inheritdoc />
            public object[] GetElements(object input) {
                return ((IEnumerable<ErrorLogEntry>) input).ToArray();
            }

            /// <inheritdoc />
            public bool HasChildren(object item) {
                return false;
            }

            /// <inheritdoc />
            public object[] GetChildren(object item) {
                return null;
            }

            /// <inheritdoc />
            public ColumnDescriptor[] Columns { get { return new[] {MessageColumn, PluginColumn, DateColumn}; } }
        }
    }
}