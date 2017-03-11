using motoi.platform.ui.data;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines the properties of a list viewer.
    /// </summary>
    public interface IListViewer : IWidget, IDataViewer<IListContentProvider, IListLabelProvider> {
        
    }
}