using motoi.platform.ui.widgets;

namespace motoi.platform.ui.layouts
{
    /// <summary>
    /// Defines a layout behaviour used by the <see cref="IGridComposite"/>.
    /// </summary>
    public class GridLayoutBehaviour {

        private int iColumnSpan = 1;
        private int iRowSpan = 1;

        /// <summary>
        /// Returns the number of column span or does set it.
        /// </summary>
        public int ColumnSpan {
            get { return iColumnSpan; }
            set { iColumnSpan = value; }
        }

        /// <summary>
        /// Returns the number of row span or does set it.
        /// </summary>
        public int RowSpan {
            get { return iRowSpan; }
            set { iRowSpan = value; }
        }
    }
}