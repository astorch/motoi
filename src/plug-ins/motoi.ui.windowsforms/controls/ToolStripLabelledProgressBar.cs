using System.Drawing;
using System.Windows.Forms;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Represents a tool strip progress bar with a label. The label is used 
    /// to display additional hints for the user.
    /// </summary>
    public class ToolStripLabelledProgressBar : ToolStripControlHost {
        /// <summary>
        /// Creates a new instance of the <see cref="ToolStripProgressBar"/> class.
        /// </summary>
        public ToolStripLabelledProgressBar()
            : base(CreateControlInstance()) {
            // Nothing to do here
        }

        /// <summary>
        /// Returns the underlying control panel.
        /// </summary>
        private TableLayoutPanel ControlPanel {
            get { return (TableLayoutPanel) Control; }
        }

        /// <summary>
        /// Returns the label control that displays the progress hint.
        /// </summary>
        private Label ProgressHintLabel {
            get { return (Label) ControlPanel.Controls[1]; }
        }

        /// <summary>
        /// Returns the progress bar control.
        /// </summary>
        private TextProgressBar ProgressBar {
            get { return (TextProgressBar)ControlPanel.Controls[2]; }
        }

        /// <summary>
        /// Returns TRUE when the progress is indetermine or does set it.
        /// </summary>
        public bool IsIndetermine {
            get { return ProgressBar.IsIndetermine; }
            set { ProgressBar.IsIndetermine = value; }
        }

        /// <summary>
        /// Returns the current progress hint or does set it.
        /// </summary>
        public string ProgressHint {
            get { return ProgressHintLabel.Text; }
            set { ProgressHintLabel.Text = value; }
        }

        /// <summary>
        /// Returns the current progress value or does set it.
        /// </summary>
        public ushort ProgressValue {
            get { return (ushort) ProgressBar.Value; }
            set {
                ProgressBar.Value = value;
            }
        }

        /// <summary>
        /// Returns the current progress text or doest set it.
        /// </summary>
        public string ProgressText {
            get { return ProgressBar.Text; }
            set { ProgressBar.Text = value; }
        }

        /// <summary>
        /// Returns TRUE if the area which visualizes the progress is visible 
        /// or does set it.
        /// </summary>
        public bool ProgressVisible {
            get { return ProgressHintLabel.Visible; }
            set {
                ProgressHintLabel.Visible = value;
                ProgressBar.Visible = value;
            }
        }

        /// <summary>
        /// Creates the underlying control instance that is bound by control host.
        /// </summary>
        /// <returns>Underlying control instance</returns>
        private static Control CreateControlInstance() {
            TableLayoutPanel panel = new TableLayoutPanel {ColumnCount = 3, RowCount = 1, MinimumSize = new Size(562, 33)};
            panel.Controls.Add(new Label {Width = 2, BorderStyle = BorderStyle.Fixed3D, AutoSize = false, Anchor = AnchorStyles.Top | AnchorStyles.Bottom}, 0, 0);
            panel.Controls.Add(new Label {Width = 380, AutoEllipsis = true, Anchor = AnchorStyles.Bottom | AnchorStyles.Top}, 1, 0);
            panel.Controls.Add(new TextProgressBar { Width = 180, Anchor = AnchorStyles.Top | AnchorStyles.Bottom, ForeColor = Color.Black}, 2, 0);
            return panel; 
        }
    }
}