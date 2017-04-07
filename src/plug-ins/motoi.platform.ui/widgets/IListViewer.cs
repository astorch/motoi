using motoi.platform.ui.data;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines the properties of a list viewer.
    /// </summary>
    public interface IListViewer : IWidget, IDataViewer<IListContentProvider, IListLabelProvider> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IListViewer"/> that is used by data binding operations.
    /// </summary>
    public class PListViewer : PWidgetControl<IListViewer> {
        
    }
}