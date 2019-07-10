using System;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.layouts;
using motoi.platform.ui.widgets;
using motoi.ui.windowsforms.utils;

namespace motoi.ui.windowsforms.controls {
    /// <summary> Provides an implementation of <see cref="IGridPanel"/>. </summary>
    public class GridPanel : TableLayoutPanel, IGridPanel {

        private int iNextColumn;
        private int iNextRow;
        
        /// <inheritdoc />
        public GridPanel() {
            InitializeComponent();
        }

        #region IGridPanel

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PGridPanel.GetModelValue(this, PGridPanel.VisibilityProperty); }
            set {
                PGridPanel.SetModelValue(this, PGridPanel.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            }
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PGridPanel.GetModelValue(this, PGridPanel.EnabledProperty); }
            set {
                PGridPanel.SetModelValue(this, PGridPanel.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <summary>
        /// Returns the number of layout columns or does set it.
        /// </summary>
        int IGridPanel.GridColumns {
            get {
                return PGridPanel.GetModelValue(this, PGridPanel.GridColumnsProperty);
            }
            set {
                PGridPanel.SetModelValue(this, PGridPanel.GridColumnsProperty, value);
                ColumnCount = value;
            }
        }

        /// <summary>
        /// Returns the number of layout rows or does set it.
        /// </summary>
        int IGridPanel.GridRows {
            get {
                return PGridPanel.GetModelValue(this, PGridPanel.GridRowsProperty);
            }
            set {
                PGridPanel.SetModelValue(this, PGridPanel.GridRowsProperty, value);
                RowCount = value;
            }
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
        void IGridPanel.AddColumnDefinition(string value) {
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
                ((IGridPanel) this).GridColumns++;
        }

        /// <summary>
        /// Adds the given widget to the next free cell of the composite.
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <param name="layoutBehaviour">Additional layout informations</param>
        void IGridPanel.AddWidget(IWidget widget, GridLayoutBehaviour layoutBehaviour) {
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

            if (iNextColumn >= ((IGridPanel) this).GridColumns) {
                iNextColumn = 0;
                iNextRow++;
            }
        }

        /// <summary>
        /// Adds the given widget to the next free cell of the composite. 
        /// The widget gets a default layout behaviour.
        /// </summary>
        /// <param name="widget">Widget</param>
        void IGridPanel.AddWidget(IWidget widget) {
            ((IGridPanel) this).AddWidget(widget, null);
        }

        #endregion

        /// <summary> Notifies the instance to initialize its content. </summary>
        private void InitializeComponent() {
            // Currently nothing to do here
        }
    }
}