using System.Drawing;
using System.Windows.Forms;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.controls;

namespace motoi.ui.windowsforms.shells {
    /// <summary> Provides an implementation of <see cref="ITitledAreaDialog"/>. </summary>
    public class TitledAreaDialog : DialogWindow, ITitledAreaDialog {
        private TableLayoutPanel _tableLayoutPanel;
        private Label _lblText;
        private Label _lblTitle;
        private Panel _contentPanel;
        
        /// <inheritdoc />
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
                _lblText.Text = value;
            }
        }

        /// <summary>
        /// Returns the title of the dialog or does set it.
        /// </summary>
        string ITitledAreaDialog.Title {
            get { return PTitledAreaDialog.GetModelValue(this, PTitledAreaDialog.TitleProperty); }
            set {
                PTitledAreaDialog.SetModelValue(this, PTitledAreaDialog.TitleProperty, value);
                _lblTitle.Text = value;
            }
        }

        #endregion

        /// <summary> Performs an initialization of the used components. </summary>
        private void InitializeComponent() {
            ClientSize = new Size(600, 404);
            AutoSize = true;
            ((IDialogWindow)this).WindowResizeMode = EWindowResizeMode.CanResize;
        }

        /// <inheritdoc />
        protected override Control CreateContentControl(Panel contentContainer) {
            CreateControls();
            contentContainer.Controls.Add(_tableLayoutPanel);
            return _contentPanel;
        }

        /// <inheritdoc />
        protected override void OnWindowClosed() {
            _tableLayoutPanel.Controls.Clear();
            _contentPanel.Controls.Clear();
            _tableLayoutPanel = null;
            _contentPanel = null;
            _lblText = null;
            _lblTitle = null;

            base.OnWindowClosed();
        }

        /// <summary> Creates the controls used by this component. </summary>
        private void CreateControls() {
            _tableLayoutPanel = new TableLayoutPanel();

            GradientBackgroundPanel backgroundPanel = new GradientBackgroundPanel {
                    BackColor = Color.WhiteSmoke,
                    BackColorStop = SystemColors.ActiveCaption,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0)
                };

            TableLayoutPanel gradientBackgroundLayoutPanel = new TableLayoutPanel {
                RowCount = 2, ColumnCount = 1, 
                Dock = DockStyle.Fill, 
                BackColor = Color.Transparent
            };
            backgroundPanel.Controls.Add(gradientBackgroundLayoutPanel);

            _lblTitle = new Label {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Margin = new Padding(5)
            };
            gradientBackgroundLayoutPanel.Controls.Add(_lblTitle, 0, 0);

            _lblText = new Label {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(5)
            };
            gradientBackgroundLayoutPanel.Controls.Add(_lblText, 0, 1);

            Separator separator = new Separator {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            _contentPanel = new Panel {
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            // tableLayoutPanel
            _tableLayoutPanel.ColumnCount = 1;
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tableLayoutPanel.Controls.Add(backgroundPanel, 0, 0);
            _tableLayoutPanel.Controls.Add(separator, 0, 1);
            _tableLayoutPanel.Controls.Add(_contentPanel, 0, 2);
            _tableLayoutPanel.Dock = DockStyle.Fill;
            _tableLayoutPanel.Margin = new Padding(0);
            _tableLayoutPanel.RowCount = 5;
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        }
    }
}