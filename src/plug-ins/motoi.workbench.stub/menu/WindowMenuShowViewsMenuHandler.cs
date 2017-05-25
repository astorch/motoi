using System.Linq;
using motoi.platform.ui.actions;
using motoi.platform.ui.data;
using motoi.platform.ui.factories;
using motoi.platform.ui.images;
using motoi.platform.ui.shells;
using motoi.platform.ui.widgets;
using motoi.workbench.model;
using motoi.workbench.registries;

namespace motoi.workbench.stub.menu {
    /// <summary>
    /// Implements an action that displays all available views in a dialog window and provides 
    /// the option to open a selected one.
    /// </summary>
    public class WindowMenuShowViewsMenuHandler : AbstractActionHandler {
        /// <inheritdoc />
        public override void Run() {
            ITitledAreaDialog dialogWindow = UIFactory.NewShell<ITitledAreaDialog>();
            dialogWindow.Title = Messages.WindowMenuShowViewsMenuHandler_DialogTitle;
            dialogWindow.Description = Messages.WindowMenuShowViewsMenuHandler_DialogDescription;
            dialogWindow.Width = 440;
            
            IGridPanel gridComposite = UIFactory.NewWidget<IGridPanel>(dialogWindow);

            gridComposite.GridColumns = 1;
            gridComposite.GridRows = 1;
            IListViewer listViewer = UIFactory.NewWidget<IListViewer>(gridComposite);
            gridComposite.AddWidget(listViewer);

            IViewReference selectedViewReference = null;
            bool closedOk = false;
            listViewer.ContentProvider = new ListViewerContentProviderImpl();
            listViewer.LabelProvider = new ListViewerLabelProviderImpl();
            listViewer.Input = DataViewRegistry.Instance.GetViewReferences().OrderBy(view => view.Title).ToArray();
            listViewer.Update();
            listViewer.SelectionChanged += (sender, args) => selectedViewReference = (IViewReference) args.Selection;
            dialogWindow.AddButton(Messages.WindowMenuShowViewsMenuHandler_DialogButtonCancel, 
                                   new ActionHandlerDelegate(() => dialogWindow.Close()));
            dialogWindow.AddButton(Messages.WindowMenuShowViewsMenuHandler_DialogButtonOk, 
                                   new ActionHandlerDelegate(() => {
                                        closedOk = true;
                                        dialogWindow.Close();
                                   }));

            dialogWindow.SetContent(gridComposite);
            dialogWindow.Show(true);

            if (closedOk && selectedViewReference != null) {
                string viewId = selectedViewReference.Id;
                PlatformUI.Instance.Workbench.ActivePerspective.OpenView(viewId, EViewPosition.Bottom);
            }
        }

        /// <summary>
        /// Implements <see cref="IListLabelProvider"/> for a set of <see cref="IViewReference"/>.
        /// </summary>
        class ListViewerLabelProviderImpl : IListLabelProvider {
            /// <inheritdoc />
            public string GetText(object item) {
                return ((IViewReference) item).Title;
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
                return ((IViewReference) element).Title;
            }

            /// <inheritdoc />
            public ImageDescriptor GetImage(object element, ColumnDescriptor column) {
                return null;
            }
        }

        /// <summary>
        /// Implements <see cref="IListContentProvider"/> for a set of <see cref="IViewReference"/>.
        /// </summary>
        class ListViewerContentProviderImpl : IListContentProvider {
            /// <inheritdoc />
            public object[] GetElements(object input) {
                return (IViewReference[]) input;
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
            public ColumnDescriptor[] Columns { get { return new[] {new ColumnDescriptor(Messages.WindowMenuShowViewsMenuHandler_ColumnViews_Name, 400)}; } }
        }
    }
}