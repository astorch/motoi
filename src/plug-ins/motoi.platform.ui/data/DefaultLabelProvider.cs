using motoi.platform.ui.images;

namespace motoi.platform.ui.data {
    /// <summary> Provides a default implementation of <see cref="ILabelProvider"/>. </summary>
    public class DefaultLabelProvider : ILabelProvider {
        /// <summary> Default instance. </summary>
        public static readonly ILabelProvider Instance = new DefaultLabelProvider();

        /// <inheritdoc />
        public string GetText(object item) {
            return item.ToString();
        }

        /// <inheritdoc />
        public ImageDescriptor GetImage(object item) {
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