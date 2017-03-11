using System;
using System.Windows.Forms;
using motoi.platform.ui.layouts;
using motoi.platform.ui.widgets;
using motoi.ui.windowsforms.utils;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="IGridComposite"/>.
    /// </summary>
    public class CompositePanel : TableLayoutPanel, IGridComposite {

        private int iNextColumn;
        private int iNextRow;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public CompositePanel() {
            InitializeComponent();
        }

        #region IGridComposite

        /// <summary>
        /// Returns the number of layout columns or does set it.
        /// </summary>
        int IGridComposite.GridColumns {
            get { return ColumnCount; }
            set { ColumnCount = value; }
        }

        /// <summary>
        /// Returns the number of layout rows or does set it.
        /// </summary>
        int IGridComposite.GridRows {
            get { return RowCount; }
            set { RowCount = value; }
        }

        /// <summary>
        /// Adds a new column. The column width is calculated based on the given <paramref name="value"/>.
        /// <list type="bullet">
        ///     <item>Auto - The width is calculated automatically.</item>
        ///     <item>* - The column grabs all available space.</item>
        ///     <item>125 - The column has a fixed width of 125.</item>
        /// </list>
        /// </summary>
        /// <param name="value">Value of column definition</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is NULL or empty</exception>
        /// <exception cref="FormatException">If <paramref name="value"/> cannot be parsed</exception>
        void IGridComposite.AddColumnDefinition(string value) {
            ColumnDefinition columnDefinition = new ColumnDefinition(value);
            ColumnStyle columnStyle = new ColumnStyle();
            SizeBehavior mode = columnDefinition.Mode;

            columnStyle.SizeType = mode == SizeBehavior.Auto
                ? SizeType.AutoSize
                : (mode == SizeBehavior.Fill ? SizeType.Percent : SizeType.Absolute);

            if (columnStyle.SizeType == SizeType.Percent)
                columnStyle.Width = 1.0f;

            if (columnStyle.SizeType == SizeType.Absolute)
                columnStyle.Width = columnDefinition.Width;

            ColumnStyles.Add(columnStyle);

            if (ColumnStyles.Count > ColumnCount)
                ((IGridComposite) this).GridColumns++;
        }

        /// <summary>
        /// Adds the given widget to the next free cell of the composite.
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <param name="layoutBehaviour">Additional layout informations</param>
        void IGridComposite.AddWidget(IWidget widget, GridLayoutBehaviour layoutBehaviour) {
            Control wdgCtrl = CastUtil.Cast<Control>(widget);

            // Add the control
            Controls.Add(wdgCtrl, iNextColumn, iNextRow);
            wdgCtrl.Dock = DockStyle.Fill;

            // Handle layout behaviour
            GridLayoutBehaviour behaviour = layoutBehaviour ?? new GridLayoutBehaviour();
            iNextColumn += behaviour.ColumnSpan;

            // Apply span
            SetColumnSpan(wdgCtrl, behaviour.ColumnSpan);
            SetRowSpan(wdgCtrl, behaviour.RowSpan);

            if (iNextColumn >= ((IGridComposite) this).GridColumns) {
                iNextColumn = 0;
                iNextRow++;
            }
        }

        /// <summary>
        /// Adds the given widget to the next free cell of the composite. 
        /// The widget gets a default layout behaviour.
        /// </summary>
        /// <param name="widget">Widget</param>
        void IGridComposite.AddWidget(IWidget widget) {
            ((IGridComposite) this).AddWidget(widget, null);
        }

        #endregion

        /// <summary>
        /// Notifies the instance to initialize its content.
        /// </summary>
        private void InitializeComponent() {
            // Currently nothing to do here
        }
    }
}