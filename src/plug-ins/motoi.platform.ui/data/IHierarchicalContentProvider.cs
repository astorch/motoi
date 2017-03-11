namespace motoi.platform.ui.data {
    /// <summary>
    /// Specializes <see cref="IContentProvider"/> and adds properties used 
    /// to display hierarchical structured data.
    /// </summary>
    public interface IHierarchicalContentProvider : IContentProvider {
        /// <summary>
        /// Return true if the item has child elements.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it has child elements</returns>
        bool HasChildren(object item);

        /// <summary>
        /// Return all children of the given item.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Children of the item</returns>
        object[] GetChildren(object item);
    }
}