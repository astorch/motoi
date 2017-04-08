using System.Windows.Forms;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.controls;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="ITitledAreaDialog"/>.
    /// </summary>
    public class TitledAreaDialog : DialogWindow, ITitledAreaDialog {
        private Label iLblText;
        private Label iLblTitle;
        private TableLayoutPanel iTableLayoutPanel;
        private Panel iContentPanel;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public TitledAreaDialog() {
            InitializeComponent();
        }

        #region ITitledAreaDialog

        /// <summary>
        /// Returns the description text of the dialog or does set it.
        /// </summary>
        string ITitledAreaDialog.Description {
            get { return PTitledAreaDialog.GetModelValue(this, PTitledAreaDialog.DescriptionProperty); }
            set {
                PTitledAreaDialog.SetModelValue(this, PTitledAreaDialog.DescriptionProperty, value);
                iLblText.Text = value;
            }
        }

        /// <summary>
        /// Returns the title of the dialog or does set it.
        /// </summary>
        string ITitledAreaDialog.Title {
            get { return PTitledAreaDialog.GetModelValue(this, PTitledAreaDialog.TitleProperty); }
            set {
                PTitledAreaDialog.SetModelValue(this, PTitledAreaDialog.TitleProperty, value);
                iLblTitle.Text = value;
            }
        }

        #endregion

        /// <summary>
        /// Performs an initialization of the used components.
        /// </summary>
        private void InitializeComponent() {
            // Nothing to do here
        }

        /// <inheritdoc />
        protected override Panel CreateContentControl(Panel contentContainer) {
            CreateControls();
            contentContainer.Controls.Add(iTableLayoutPanel);
            return iContentPanel;
        }

        /// <summary>
        /// Creates the controls used by this component.
        /// </summary>
        private void CreateControls() {
            GradientBackgroundPanel backgroundPanel = new GradientBackgroundPanel();
            iLblText = new Label();
            iLblTitle = new Label();
            iTableLayoutPanel = new TableLayoutPanel();
            Separator separator = new Separator();
            iContentPanel = new Panel();
            backgroundPanel.SuspendLayout();
            iTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // gradientBackgroundPanel1
            // 
            backgroundPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            backgroundPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            backgroundPanel.BackColorStop = System.Drawing.SystemColors.ActiveCaption;
            backgroundPanel.Controls.Add(iLblText);
            backgroundPanel.Controls.Add(iLblTitle);
            backgroundPanel.Dock = DockStyle.Fill;
            backgroundPanel.Location = new System.Drawing.Point(0, 0);
            backgroundPanel.Margin = new Padding(0);
            backgroundPanel.Name = "backgroundPanel";
            backgroundPanel.Size = new System.Drawing.Size(486, 64);
            backgroundPanel.TabIndex = 0;
            // 
            // LblText
            // 
            iLblText.AutoSize = true;
            iLblText.BackColor = System.Drawing.Color.Transparent;
            iLblText.Location = new System.Drawing.Point(9, 30);
            iLblText.Margin = new Padding(5);
            iLblText.Name = "iLblText";
            iLblText.Size = new System.Drawing.Size(145, 25);
            iLblText.TabIndex = 1;
            iLblText.Text = "Description text";
            // 
            // LblTitle
            // 
            iLblTitle.AutoSize = true;
            iLblTitle.BackColor = System.Drawing.Color.Transparent;
            iLblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            iLblTitle.Location = new System.Drawing.Point(9, 4);
            iLblTitle.Margin = new Padding(5);
            iLblTitle.Name = "iLblTitle";
            iLblTitle.Size = new System.Drawing.Size(58, 25);
            iLblTitle.TabIndex = 0;
            iLblTitle.Text = "Title";
            // 
            // tableLayoutPanel1
            // 
            iTableLayoutPanel.ColumnCount = 1;
            iTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            iTableLayoutPanel.Controls.Add(backgroundPanel, 0, 0);
            iTableLayoutPanel.Controls.Add(separator, 0, 1);
            iTableLayoutPanel.Controls.Add(iContentPanel, 0, 2);
            iTableLayoutPanel.Dock = DockStyle.Fill;
            iTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            iTableLayoutPanel.Margin = new Padding(0);
            iTableLayoutPanel.Name = "iTableLayoutPanel";
            iTableLayoutPanel.RowCount = 5;
            iTableLayoutPanel.RowStyles.Add(new RowStyle());
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            iTableLayoutPanel.Size = new System.Drawing.Size(486, 404);
            iTableLayoutPanel.TabIndex = 3;

            // 
            // separator2
            // 
            separator.BorderStyle = BorderStyle.Fixed3D;
            separator.Dock = DockStyle.Fill;
            separator.Location = new System.Drawing.Point(0, 64);
            separator.Margin = new Padding(0);
            separator.Name = "iSeparator";
            separator.Size = new System.Drawing.Size(486, 1);
            separator.TabIndex = 2;
            // 
            // ContentPanel
            // 
            iContentPanel.Dock = DockStyle.Fill;
            iContentPanel.Location = new System.Drawing.Point(0, 65);
            iContentPanel.Margin = new Padding(0);
            iContentPanel.Name = "iContentPanel";
            iContentPanel.Size = new System.Drawing.Size(486, 293);
            iContentPanel.TabIndex = 4;
            // 
            // TitledAreaDialog
            // 
            ClientSize = new System.Drawing.Size(486, 404);
            Controls.Add(iTableLayoutPanel);
            MinimizeBox = false;
            Name = "TitledAreaDialog";
            StartPosition = FormStartPosition.CenterParent;
            backgroundPanel.ResumeLayout(false);
            backgroundPanel.PerformLayout();
            iTableLayoutPanel.ResumeLayout(false);
            iTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}