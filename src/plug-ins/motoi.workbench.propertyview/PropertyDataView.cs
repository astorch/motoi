using motoi.platform.ui.factories;
using motoi.platform.ui.images;
using motoi.platform.ui.widgets;
using motoi.workbench.runtime;

namespace motoi.workbench.propertyview {
    /// <summary>
    /// Provides a view of properties that are associated with a selected workbench file.
    /// </summary>
    public class PropertyDataView : AbstractDataView {
        /// <summary> Data view id of this class. </summary>
        // ReSharper disable once UnusedMember.Global
        public const string Id = "motoi.workbench.propertyview.propertyDataView";

        /// <inheritdoc />
        public override IWidgetFactory WidgetFactory { get; set; }

        /// <inheritdoc />
        public override void Init() {
            // Initialize registry
            PropertyViewItemsProviderRegistry.Instance.Handshake();
        }

        /// <inheritdoc />
        public override void CreateContents(IGridPanel gridComposite) {
//            gridComposite.GridColumns = 1;
//            gridComposite.GridRows = 1;
//
//            IListViewer listViewer = WidgetFactory.CreateInstance<IListViewer>(gridComposite);
//            gridComposite.AddWidget(listViewer);
        }

        /// <inheritdoc />
        public override string Name { get { return Messages.PropertyDataView_Name; } }

        /// <inheritdoc />
        public override ImageDescriptor Image { get { return null; } }
    }
}
