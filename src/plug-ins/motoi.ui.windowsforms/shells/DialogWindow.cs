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
        private Panel iContentPanel;

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
            try {
                iFlowLayoutPanel.SuspendLayout();

                MotoiButton button = new MotoiButton {AutoSize = true, Text = label};
                ((IButton) button).ActionHandler = actionHandler;
                iFlowLayoutPanel.Controls.Add(button);

                return button;
            } finally {
                iFlowLayoutPanel.ResumeLayout(true);
            }
        }

        /// <inheritdoc />
        void IShell.SetContent(IWidgetCompound widgetCompound) {
            try {
                SuspendLayout();

                // Remove old content
                iContentPanel.Controls.Clear();

                // Save new content
                WindowContent = widgetCompound;
                if (widgetCompound == null) return;

                // Add new content
                Control contentControl = CastUtil.Cast<Control>(widgetCompound);
                contentControl.Dock = DockStyle.Fill; // XXX
                contentControl.Margin = new Padding(0); // XXX
                contentControl.Padding = new Padding(0); // XXX
                iContentPanel.Controls.Add(contentControl);
            } finally {
                ResumeLayout(true);
            }
        }

        /// <summary>
        /// Notifies the instance to create a content control as a child of the given 
        /// content container. The content control is used to place the content set 
        /// by <see cref="IShell.SetContent"/>.
        /// </summary>
        /// <param name="contentContainer">Container of the content control</param>
        /// <returns>Content control</returns>
        protected virtual Panel CreateContentControl(Panel contentContainer) {
            return contentContainer;
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

            // Content panel
            Panel contentContainer = new Panel { Dock = DockStyle.Fill };
            iTableLayoutPanel.Controls.Add(contentContainer, 0, 0);
            iContentPanel = CreateContentControl(contentContainer);

            // Separator
            Separator separator = new Separator { Dock = DockStyle.Fill };
            iTableLayoutPanel.Controls.Add(separator, 0, 1);

            // Button panel
            iFlowLayoutPanel = new FlowLayoutPanel {FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Bottom};
            iFlowLayoutPanel.AutoSize = true;
            iTableLayoutPanel.Controls.Add(iFlowLayoutPanel, 0, 2);

            ((IWindow) this).WindowStartupLocation = EWindowStartupLocation.CenterOwner;
        }
    }
}