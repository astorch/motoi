using motoi.platform.ui.widgets;

namespace motoi.platform.ui.data {
    /// <summary>
    /// Specializes <see cref="IHierarchicalContentProvider"/> and adds properties used 
    /// by <see cref="IListViewer"/>.
    /// </summary>
    public interface IListContentProvider : IHierarchicalContentProvider {
        /// <summary> Returns the columns to display the content. </summary>
        ColumnDescriptor[] Columns { get; }
    }
}