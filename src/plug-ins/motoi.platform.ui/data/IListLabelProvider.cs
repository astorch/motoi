using motoi.platform.ui.images;

namespace motoi.platform.ui.data {
    /// <summary>
    /// Specializes <see cref="ILabelProvider"/> and provides additional properties used 
    /// by <see cref="IListLabelProvider"/>.
    /// </summary>
    public interface IListLabelProvider : ILabelProvider {
        /// <summary>
        /// Return the text for the given <paramref name="element"/> that is shown in the given <paramref name="column"/>.
        /// </summary>
        /// <param name="element">Element to provide text</param>
        /// <param name="column">Column where the text is shown</param>
        /// <returns>Text for the element</returns>
        string GetText(object element, ColumnDescriptor column);

        /// <summary>
        /// Return an <see cref="ImageDescriptor"/> for the <paramref name="element"/>. If you return NULL, 
        /// no image is shown.
        /// </summary>
        /// <param name="element">Element to provide an image</param>
        /// <param name="column">Column where the image is shown</param>
        /// <returns><see cref="ImageDescriptor"/> for the element</returns>
        ImageDescriptor GetImage(object element, ColumnDescriptor column);
    }
}