using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.actions;
using motoi.platform.ui.shells;
using motoi.platform.ui.widgets;
using motoi.ui.windowsforms.controls;
using MotoiButton = motoi.ui.windowsforms.controls.Button;
using motoi.ui.windowsforms.utils;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="IDialogWindow"/>.
    /// </summary>
    public class DialogWindow : Window, IDialogWindow {
        private FlowLayoutPanel iFlowLayoutPanel;
        private TableLayoutPanel iTableLayoutPanel;

        /// <inheritdoc />
        public DialogWindow() {
            InitializeComponents();
        }

        #region IDialogWindow

        /// <summary>
        /// Creates the dialogs and makes it visible to the user. If the flag 
        /// <paramref name="modal"/> is TRUE the dialog has a modal behaviour.
        /// </summary>
        /// <param name="modal">TRUE or FALSE</param>
        void IDialogWindow.Show(bool modal) {
            if (modal)
                ShowDialog();
            else
                Show();
        }

        /// <summary>
        /// Creates the window and makes it visible to the user.
        /// </summary>
        void IShell.Show() {
            ((IDialogWindow) this).Show(true);
        }

        /// <inheritdoc />
        IButton IDialogWindow.AddButton(string label, IActionHandler actionHandler) {
            MotoiButton button = new MotoiButton {AutoSize = true, Text = label};
            ((IButton) button).ActionHandler = actionHandler;
            iFlowLayoutPanel.Controls.Add(button);
            PerformLayout();
            return button;
        }

        /// <inheritdoc />
        void IShell.SetContent(IWidgetCompound widgetCompound) {
            IWidgetCompound dialogContent = WindowContent;
            if (dialogContent != null) {
                Control oldContent = ToContentControl(dialogContent, true);
                iTableLayoutPanel.Controls.Remove(oldContent);
            }

            WindowContent = widgetCompound;
            if (widgetCompound == null) return;

            Control newContent = ToContentControl(widgetCompound, false);
            iTableLayoutPanel.Controls.Add(newContent, 0, 0);
            
            PerformLayout();
        }

        /// <summary>
        /// Returns a control type of the given widget compound.
        /// </summary>
        /// <param name="widgetCompound">Widget compound to cast</param>
        /// <param name="remove">TRUE if the control is going to be removed</param>
        /// <returns>Control type of the widget compound</returns>
        protected virtual Control ToContentControl(IWidgetCompound widgetCompound, bool remove) {
            return CastUtil.Cast<Control>(widgetCompound);
        }

        #endregion

        /// <summary>
        /// Performs an initialization of the used components.
        /// </summary>
        private void InitializeComponents() {
            iTableLayoutPanel = new TableLayoutPanel { RowCount = 3, ColumnCount = 1, Dock = DockStyle.Fill};
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 1f)); // Seperator
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Buttons
            Controls.Add(iTableLayoutPanel);

            Separator separator = new Separator { Dock = DockStyle.Fill };
            iTableLayoutPanel.Controls.Add(separator, 0, 1);

            iFlowLayoutPanel = new FlowLayoutPanel {FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Bottom};
            iFlowLayoutPanel.AutoSize = true;
            iTableLayoutPanel.Controls.Add(iFlowLayoutPanel, 0, 2);

            ((IWindow) this).WindowStartupLocation = EWindowStartupLocation.CenterOwner;
        }
    }
}