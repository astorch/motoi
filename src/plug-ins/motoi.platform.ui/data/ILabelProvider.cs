using motoi.platform.ui.images;

namespace motoi.platform.ui.data {
    /// <summary>
    /// Defines the interface of a Label Provider. Label Providers are used by 
    /// data viewers to style the data that is being displayed.
    /// </summary>
    public interface ILabelProvider {
        /// <summary> Return the text for the item. </summary>
        /// <param name="item">Item</param>
        /// <returns>Text of the item</returns>
        string GetText(object item);

        /// <summary>
        /// Return an <see cref="ImageDescriptor"/> for the <paramref name="item"/>.
        /// If you return NULL, no image is shown.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns><see cref="ImageDescriptor"/> for the item</returns>
        ImageDescriptor GetImage(object item);

        /// <summary> Return the tooltip for the item. If you return NULL the default text is used. </summary>
        /// <param name="item">Item</param>
        /// <returns>Tooltip of the item</returns>
        string GetToolTipText(object item);

        /// <summary> Return true, if the item shall be enabled. </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item shall be enabled</returns>
        bool IsEnabled(object item);
    }
}