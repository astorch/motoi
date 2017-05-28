using System.Drawing;
using System.IO;
using System.Windows.Forms;
using motoi.platform.commons;
using motoi.platform.ui.actions;
using motoi.platform.ui.shells;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="IMessageDialog"/>.
    /// </summary>
    public class MessageDialog : DialogWindow, IMessageDialog {
        private TableLayoutPanel iTableLayoutPanel;
        private Label iHeaderLabel;
        private PictureBox iPictureBox;
        private Label iTextLabel;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public MessageDialog() {
            InitializeComponent();
        }

        #region IMessageDialogWindow

        /// <summary>
        /// Returns the dialog type or does set it.
        /// </summary>
        EMessageDialogType IMessageDialog.DialogType { get; set; }

        /// <summary>
        /// Returns the set of dialog results that are available to the user.
        /// </summary>
        EMessageDialogResult[] IMessageDialog.DialogResultSet { get; set; }

        /// <summary>
        /// Creates the dialog and makes it visible to the user.
        /// </summary>
        /// <returns>Dialog close result</returns>
        EMessageDialogResult IMessageDialog.Show() {
            // Add dialog buttons
            EMessageDialogResult[] resultSet = ((IMessageDialog) this).DialogResultSet ?? new[] {EMessageDialogResult.Ok};
            EMessageDialogResult dialogResult = EMessageDialogResult.Cancel;
            
            for (int i = resultSet.Length; --i != -1;) {
                EMessageDialogResult resultItem = resultSet[i];
                string buttonText = LocalizeText(resultItem);

                ((IDialogWindow) this).AddButton(buttonText, new ActionHandlerDelegate(() => {
                    dialogResult = resultItem;
                    Close();
                })); // XXX Closure
            }

            // Add dialog image
            EMessageDialogType dialogType = ((IMessageDialog) this).DialogType;
            Image dialogImage = GetImageForDialogType(dialogType);
            iPictureBox.Image = dialogImage;

            PerformLayout();

            // Show dialog
            ((IDialogWindow)this).Show();

            // Return result
            return dialogResult;
        }

        /// <summary>
        /// Returns the localized text of the given <paramref name="resultItem"/>.
        /// </summary>
        /// <param name="resultItem">Item to localize</param>
        /// <returns>Localized result item text</returns>
        private string LocalizeText(EMessageDialogResult resultItem) {
            if (resultItem == EMessageDialogResult.Ok) return Messages.MessageDialogWindow_Button_Ok;
            if (resultItem == EMessageDialogResult.Cancel) return Messages.MessageDialogWindow_Button_Cancel;
            if (resultItem == EMessageDialogResult.No) return Messages.MessageDialogWindow_Button_No;
            if (resultItem == EMessageDialogResult.Yes) return Messages.MessageDialogWindow_Button_Yes;
            return resultItem.ToString();
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
            using (Stream resourceStream = ResourceLoader.OpenStream(resourcePath)) {
                return new Bitmap(resourceStream);
            }
        }

        /// <summary>
        /// Returns the dialog header or does set it.
        /// </summary>
        string IMessageDialog.Header {
            get { return PMessageDialog.GetModelValue(this, PMessageDialog.HeaderProperty); }
            set {
                PMessageDialog.SetModelValue(this, PMessageDialog.HeaderProperty, value);
                iHeaderLabel.Text = value;
            }
        }

        /// <summary>
        /// Returns the dialog text or does set it.
        /// </summary>
        string IMessageDialog.Text {
            get { return PMessageDialog.GetModelValue(this, PMessageDialog.TextProperty); }
            set {
                PMessageDialog.SetModelValue(this, PMessageDialog.TextProperty, value);
                iTextLabel.Text = value;
            }
        }

        #endregion

        /// <inheritdoc />
        protected override Control CreateContentControl(Panel contentContainer) {
            CreateControls();
            contentContainer.Controls.Add(iTableLayoutPanel);
            
            // We don't support a content control here
            return null;
        }

        /// <inheritdoc />
        protected override void OnWindowClosed() {
            iTableLayoutPanel.Controls.Clear();
            iTableLayoutPanel = null;
            iHeaderLabel = null;
            iPictureBox = null;
            iTextLabel = null;

            base.OnWindowClosed();
        }

        /// <summary>
        /// Performs an initialization of the used components.
        /// </summary>
        private void InitializeComponent() {
            ClientSize = new Size(600, 300);
            AutoSize = true;
        }

        /// <summary>
        /// Creates the controls used by this component.
        /// </summary>
        private void CreateControls() {
            iTableLayoutPanel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };

            iPictureBox = new PictureBox();

            iHeaderLabel = new Label {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                TabStop = false,
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0)
            };

            iTextLabel = new Label {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                TabStop = false
            };

            iTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            iTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            iTableLayoutPanel.Controls.Add(iHeaderLabel, 0, 0);
            iTableLayoutPanel.Controls.Add(iPictureBox, 0, 1);
            iTableLayoutPanel.Controls.Add(iTextLabel, 1, 1);
            
            iTableLayoutPanel.SetColumnSpan(iHeaderLabel, 2);
            
            // iPictureBox 
            ((System.ComponentModel.ISupportInitialize)(iPictureBox)).BeginInit();
            iPictureBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            iPictureBox.Margin = new Padding(5);
            iPictureBox.Size = new Size(48, 48);
            iPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            iPictureBox.TabStop = false;
            ((System.ComponentModel.ISupportInitialize)(iPictureBox)).EndInit();
        }
    }
}