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
        private Control iContentControl;

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
            // If there is only one button, we can make it default
            if (iFlowLayoutPanel.Controls.Count == 1) {
                AcceptButton = (IButtonControl) iFlowLayoutPanel.Controls[0];
            }

            // Make the dialog visible
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
            return ((IDialogWindow) this).AddButton(label, actionHandler, false);
        }

        /// <inheritdoc />
        IButton IDialogWindow.AddButton(string label, IActionHandler actionHandler, bool isDefault) {
            using (iFlowLayoutPanel.DeferLayout()) {
                MotoiButton button = new MotoiButton { AutoSize = true, Text = label };
                ((IButton)button).ActionHandler = actionHandler;
                iFlowLayoutPanel.Controls.Add(button);

                if (isDefault) {
                    AcceptButton = button;
                }

                return button;
            }
        }

        /// <inheritdoc />
        void IShell.SetContent(IWidgetCompound widgetCompound) {
            using (this.DeferLayout()) {
                // Remove old content
                iContentControl.Controls.Clear();

                // Save new content
                WindowContent = widgetCompound;
                if (widgetCompound == null) return;

                // Add new content
                Control dialogContent = CastUtil.Cast<Control>(widgetCompound);
                dialogContent.Dock = DockStyle.Fill; // XXX
                iContentControl.Controls.Add(dialogContent);
            }
        }

        /// <summary>
        /// Notifies the instance to create a content control as a child of the given 
        /// content container. The content control is used to place the content set 
        /// by <see cref="IShell.SetContent"/>.
        /// </summary>
        /// <param name="contentContainer">Container of the content control</param>
        /// <returns>Content control</returns>
        protected virtual Control CreateContentControl(Panel contentContainer) {
            return contentContainer;
        }

        /// <inheritdoc />
        protected override void OnWindowClosed() {
            iTableLayoutPanel.Controls.Clear();
            iFlowLayoutPanel.Controls.Clear();
            
            if (iContentControl != null)
                iContentControl.Controls.Clear();

            iTableLayoutPanel = null;
            iFlowLayoutPanel = null;
            iContentControl = null;
            
            base.OnWindowClosed();
        }

        #endregion

        /// <summary>
        /// Performs an initialization of the used components.
        /// </summary>
        private void InitializeComponents() {
            iTableLayoutPanel = new TableLayoutPanel { RowCount = 3, ColumnCount = 1, Dock = DockStyle.Fill };
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 1f)); // Seperator
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Buttons
            Controls.Add(iTableLayoutPanel);

            // Content panel
            Panel contentContainer = new Panel { Dock = DockStyle.Fill };
            iTableLayoutPanel.Controls.Add(contentContainer, 0, 0);
            iContentControl = CreateContentControl(contentContainer);

            // Separator
            Separator separator = new Separator { Dock = DockStyle.Fill };
            iTableLayoutPanel.Controls.Add(separator, 0, 1);

            // Button panel
            iFlowLayoutPanel = new FlowLayoutPanel {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                AutoSize = true // Reduce taken space to needed
            };
            iTableLayoutPanel.Controls.Add(iFlowLayoutPanel, 0, 2);

            ((IWindow) this).WindowStartupLocation = EWindowStartupLocation.CenterOwner;
            ((IWindow) this).WindowStyle = EWindowStyle.DialogWindow;
            ((IWindow) this).WindowResizeMode = EWindowResizeMode.NoResize;
        }
    }
}