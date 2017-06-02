using System.Drawing;
using System.IO;
using System.Windows.Forms;
using motoi.platform.commons;
using motoi.platform.ui.actions;
using motoi.platform.ui.shells;

namespace motoi.ui.windowsforms.shells {
    /// <summary> Provides an implementation of <see cref="IMessageDialog"/>. </summary>
    public class MessageDialog : DialogWindow, IMessageDialog {
        private TableLayoutPanel iTableLayoutPanel;
        private Label iHeaderLabel;
        private PictureBox iPictureBox;
        private Label iTextLabel;

        /// <summary> Creates a new instance. </summary>
        public MessageDialog() {
            InitializeComponent();
        }

        #region IMessageDialogWindow

        /// <inheritdoc />
        string IMessageDialog.Header {
            get { return PMessageDialog.GetModelValue(this, PMessageDialog.HeaderProperty); }
            set {
                PMessageDialog.SetModelValue(this, PMessageDialog.HeaderProperty, value);
                iHeaderLabel.Text = value;
            }
        }

        /// <inheritdoc />
        string IMessageDialog.Text {
            get { return PMessageDialog.GetModelValue(this, PMessageDialog.TextProperty); }
            set {
                PMessageDialog.SetModelValue(this, PMessageDialog.TextProperty, value);
                iTextLabel.Text = value;
            }
        }

        /// <inheritdoc />
        EMessageDialogType IMessageDialog.DialogType { get; set; }

        /// <inheritdoc />
        EMessageDialogResult[] IMessageDialog.DialogResultSet { get; set; }

        /// <inheritdoc />
        EMessageDialogResult IMessageDialog.Show() {
            // Add dialog buttons
            AddMessageDialogButtons(((IMessageDialog) this).DialogResultSet ?? new[] {EMessageDialogResult.Ok});

            // Add dialog image
            EMessageDialogType dialogType = ((IMessageDialog) this).DialogType;
            Image dialogImage = GetImageForDialogType(dialogType);
            iPictureBox.Image = dialogImage;

            PerformLayout();

            // Show dialog
            MessageDialogResult = EMessageDialogResult.None;
            ((IDialogWindow) this).Show();

            // Return result
            return MessageDialogResult;
        }

        /// <summary>
        /// Creates and adds a button for each result item of the given collection.
        /// </summary>
        /// <param name="resultSet">Collection of avaiable result items</param>
        protected virtual void AddMessageDialogButtons(EMessageDialogResult[] resultSet) {
            for (int i = resultSet.Length; --i != -1;) {
                EMessageDialogResult resultItem = resultSet[i];
                string buttonText = LocalizeText(resultItem);

                ((IDialogWindow) this).AddButton(buttonText, new ActionHandlerDelegate(() => {
                    MessageDialogResult = resultItem;
                    Close();
                })); // XXX Closure
            }
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
            if (resultItem == EMessageDialogResult.None) return Messages.MessageDialogWindow_Button_None;
            if (resultItem == EMessageDialogResult.Close) return Messages.MessageDialogWindow_Button_Close;
            return resultItem.ToString();
        }

        /// <summary>
        /// Returns an image that expresses the given <paramref name="dialogType"/>.
        /// </summary>
        /// <param name="dialogType">Dialog type</param>
        /// <returns>Image</returns>
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

        #endregion

        /// <summary> Returns the dialog result or does set it. </summary>
        protected EMessageDialogResult MessageDialogResult { get; set; }

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

        /// <summary> Performs an initialization of the used components.
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
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100F));
            
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