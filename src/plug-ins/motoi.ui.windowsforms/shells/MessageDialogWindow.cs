using System.Drawing;
using System.IO;
using System.Windows.Forms;
using motoi.platform.commons;
using motoi.platform.ui.shells;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="IMessageDialogWindow"/>.
    /// </summary>
    public class MessageDialogWindow : DialogWindow, IMessageDialogWindow {
        private TableLayoutPanel iTableLayoutPanel;
        private PictureBox iPictureBox;
        private Label iTextLabel;
        private FlowLayoutPanel iFlowLayoutPanel;
        private controls.Separator iSeparator2;
        private Label iHeaderLabel;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public MessageDialogWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// Returns the dialog type or does set it.
        /// </summary>
        EMessageDialogType IMessageDialogWindow.DialogType { get; set; }

        /// <summary>
        /// Returns the set of dialog results that are available to the user.
        /// </summary>
        EMessageDialogResult[] IMessageDialogWindow.DialogResultSet { get; set; }

        /// <summary>
        /// Creates the dialog and makes it visible to the user.
        /// </summary>
        /// <returns>Dialog close result</returns>
        EMessageDialogResult IMessageDialogWindow.Show() {
            // Add dialog buttons
            EMessageDialogResult[] resultSet = ((IMessageDialogWindow) this).DialogResultSet ?? new[] {EMessageDialogResult.Ok};
            EMessageDialogResult dialogResult = EMessageDialogResult.Cancel;
            
            for (int i = resultSet.Length; --i != -1;) {
                EMessageDialogResult resultItem = resultSet[i];
                string buttonText = LocalizeText(resultItem);

                Button button = new Button { AutoSize = true, Tag = resultItem, Text = buttonText };
                button.Click += (sender, args) => {
                    dialogResult = (EMessageDialogResult) ((Button) sender).Tag;
                    Close();
                }; // XXX Closure
                iFlowLayoutPanel.Controls.Add(button);
            }

            // Add dialog image
            EMessageDialogType dialogType = ((IMessageDialogWindow) this).DialogType;
            Image dialogImage = GetImageForDialogType(dialogType);
            iPictureBox.Image = dialogImage;

            // Show dialog
            ((IDialogWindow)this).Show();
            
            // Clean up
            Controls.Clear();

            // Return result
            return dialogResult;
        }

        private string LocalizeText(EMessageDialogResult item) {
            if (item == EMessageDialogResult.Ok) return "OK";
            if (item == EMessageDialogResult.Cancel) return "Abbrechen";
            if (item == EMessageDialogResult.No) return "Nein";
            if (item == EMessageDialogResult.Yes) return "Ja";
            return string.Empty;
        }

        private Image GetImageForDialogType(EMessageDialogType dialogType) {
            string pathFormat = "resources/images/{0}";
            string imageName;
            if (dialogType == EMessageDialogType.Info)
                imageName = "Info-32.png";
            else if (dialogType == EMessageDialogType.Error)
                imageName = "Error-32.png";
            else if (dialogType == EMessageDialogType.Warning)
                imageName = "Warning-32.png";
            else if (dialogType == EMessageDialogType.Question)
                imageName = "Question-64.png";
            else
                imageName = "Info-64.png";

            string resourcePath = string.Format(pathFormat, imageName);
            Stream resourceStream = ResourceLoader.OpenStream(resourcePath);
            return new Bitmap(resourceStream);
        }

        /// <summary>
        /// Returns the dialog header or does set it.
        /// </summary>
        string IMessageDialogWindow.Header {
            get { return iHeaderLabel.Text; }
            set { iHeaderLabel.Text = value; }
        }

        /// <summary>
        /// Returns the dialog text or does set it.
        /// </summary>
        string IMessageDialogWindow.Text {
            get { return iTextLabel.Text; }
            set { iTextLabel.Text = value; }
        }

        private void InitializeComponent() {
            this.iTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.iHeaderLabel = new System.Windows.Forms.Label();
            this.iPictureBox = new System.Windows.Forms.PictureBox();
            this.iTextLabel = new System.Windows.Forms.Label();
            this.iFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.iSeparator2 = new motoi.ui.windowsforms.controls.Separator();
            this.iTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // iTableLayoutPanel
            // 
            this.iTableLayoutPanel.AutoSize = true;
            this.iTableLayoutPanel.ColumnCount = 2;
            this.iTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.iTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.iTableLayoutPanel.Controls.Add(this.iHeaderLabel, 0, 0);
            this.iTableLayoutPanel.Controls.Add(this.iPictureBox, 0, 1);
            this.iTableLayoutPanel.Controls.Add(this.iTextLabel, 1, 1);
            this.iTableLayoutPanel.Controls.Add(this.iFlowLayoutPanel, 0, 3);
            this.iTableLayoutPanel.Controls.Add(this.iSeparator2, 0, 2);
            this.iTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.iTableLayoutPanel.Name = "iTableLayoutPanel";
            this.iTableLayoutPanel.RowCount = 4;
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.iTableLayoutPanel.Size = new System.Drawing.Size(438, 203);
            this.iTableLayoutPanel.TabIndex = 0;
            // 
            // iHeaderLabel
            // 
            this.iHeaderLabel.AutoSize = true;
            this.iTableLayoutPanel.SetColumnSpan(this.iHeaderLabel, 2);
            this.iHeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.iHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iHeaderLabel.Location = new System.Drawing.Point(5, 5);
            this.iHeaderLabel.Margin = new System.Windows.Forms.Padding(5);
            this.iHeaderLabel.Name = "iHeaderLabel";
            this.iHeaderLabel.Size = new System.Drawing.Size(428, 29);
            this.iHeaderLabel.TabIndex = 0;
            this.iHeaderLabel.Text = "Header";
            // 
            // iPictureBox
            // 
            this.iPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iPictureBox.Location = new System.Drawing.Point(5, 44);
            this.iPictureBox.Margin = new System.Windows.Forms.Padding(5);
            this.iPictureBox.Name = "iPictureBox";
            this.iPictureBox.Size = new System.Drawing.Size(48, 48);
            this.iPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iPictureBox.TabIndex = 1;
            this.iPictureBox.TabStop = false;
            // 
            // iTextLabel
            // 
            this.iTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTextLabel.Location = new System.Drawing.Point(63, 44);
            this.iTextLabel.Margin = new System.Windows.Forms.Padding(5);
            this.iTextLabel.Name = "iTextLabel";
            this.iTextLabel.Size = new System.Drawing.Size(370, 107);
            this.iTextLabel.TabIndex = 2;
            this.iTextLabel.Text = "Text text text";
            // 
            // iFlowLayoutPanel
            // 
            this.iFlowLayoutPanel.AutoSize = true;
            this.iTableLayoutPanel.SetColumnSpan(this.iFlowLayoutPanel, 2);
            this.iFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.iFlowLayoutPanel.Location = new System.Drawing.Point(3, 161);
            this.iFlowLayoutPanel.Name = "iFlowLayoutPanel";
            this.iFlowLayoutPanel.Size = new System.Drawing.Size(432, 39);
            this.iFlowLayoutPanel.TabIndex = 3;
            // 
            // iSeparator2
            // 
            this.iSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.iTableLayoutPanel.SetColumnSpan(this.iSeparator2, 2);
            this.iSeparator2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iSeparator2.Location = new System.Drawing.Point(3, 156);
            this.iSeparator2.Name = "iSeparator2";
            this.iSeparator2.Size = new System.Drawing.Size(432, 2);
            this.iSeparator2.TabIndex = 4;
            // 
            // MessageDialogWindow
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(438, 203);
            this.Controls.Add(this.iTableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageDialogWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.iTableLayoutPanel.ResumeLayout(false);
            this.iTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}