using motoi.platform.ui.widgets;

namespace motoi.platform.ui.layouts {
    /// <summary> Defines a layout behaviour used by the <see cref="IGridPanel"/>. </summary>
    public class GridLayoutBehaviour {
        private int _columnSpan = 1;
        private int _rowSpan = 1;

        /// <summary> Returns the number of column span or does set it. </summary>
        public int ColumnSpan {
            get { return _columnSpan; }
            set { _columnSpan = value; }
        }

        /// <summary> Returns the number of row span or does set it. </summary>
        public int RowSpan {
            get { return _rowSpan; }
            set { _rowSpan = value; }
        }
    }
}