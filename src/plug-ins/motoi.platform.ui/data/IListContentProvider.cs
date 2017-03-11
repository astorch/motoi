using motoi.platform.ui.widgets;

namespace motoi.platform.ui.data {
    /// <summary>
    /// Specializes <see cref="IHierarchicalContentProvider"/> and adds properties used 
    /// by <see cref="IListViewer"/>.
    /// </summary>
    public interface IListContentProvider : IHierarchicalContentProvider {
        /// <summary>
        /// Returns the columns to display the content.
        /// </summary>
        ColumnDescriptor[] Columns { get; }
    }

    /// <summary>
    /// Describes a column that is displayed by a <see cref="IListViewer"/>.
    /// </summary>
    public class ColumnDescriptor {
        /// <summary>
        /// Creates a new instance with the given <paramref name="headerText"/>. This instance uses a default width of 120 
        /// and a left orientation.
        /// </summary>
        /// <param name="headerText">Header text</param>
        public ColumnDescriptor(string headerText) : this(headerText, 120) {
            // Nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="headerText"/> and <paramref name="defaultWidth"/>. This 
        /// instance uses a left orientation.
        /// </summary>
        /// <param name="headerText">Header text</param>
        /// <param name="defaultWidth">Default width</param>
        public ColumnDescriptor(string headerText, uint defaultWidth) : this(headerText, defaultWidth, EColumnTextOrientation.Left) {
            // Nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="headerText"/>, <paramref name="defaultWidth"/> and <paramref name="orientation"/>.
        /// </summary>
        /// <param name="headerText">Header text</param>
        /// <param name="defaultWidth">Default width</param>
        /// <param name="orientation">Orientation</param>
        public ColumnDescriptor(string headerText, uint defaultWidth, EColumnTextOrientation orientation) {
            HeaderText = headerText ?? string.Empty;
            DefaultWidth = defaultWidth;
            Orientation = orientation;
        }

        /// <summary>
        /// Returns the header text.
        /// </summary>
        public string HeaderText { get; private set; }

        /// <summary>
        /// Returns the default width.
        /// </summary>
        public uint DefaultWidth { get; private set; }

        /// <summary>
        /// Returns the orientation.
        /// </summary>
        public EColumnTextOrientation Orientation { get; private set; }

        /// <summary>
        /// Defines kinds of column text orientations.
        /// </summary>
        public enum EColumnTextOrientation {
            /// <summary>
            /// Indicates a left oriented text alignment.
            /// </summary>
            Left,
            /// <summary>
            /// Indicates a center oriented text alignment.
            /// </summary>
            Center,
            /// <summary>
            /// Indicates a right oriented text alignment
            /// </summary>
            Right
        }
    }
}