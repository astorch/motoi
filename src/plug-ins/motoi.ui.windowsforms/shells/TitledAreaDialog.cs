using System.Drawing;
using System.Windows.Forms;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.controls;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="ITitledAreaDialog"/>.
    /// </summary>
    public class TitledAreaDialog : DialogWindow, ITitledAreaDialog {
        private TableLayoutPanel iTableLayoutPanel;
        private Label iLblText;
        private Label iLblTitle;
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
            ClientSize = new Size(600, 404);
            AutoSize = true;
            ((IDialogWindow)this).WindowResizeMode = EWindowResizeMode.CanResize;
        }

        /// <inheritdoc />
        protected override Control CreateContentControl(Panel contentContainer) {
            CreateControls();
            contentContainer.Controls.Add(iTableLayoutPanel);
            return iContentPanel;
        }

        /// <inheritdoc />
        protected override void OnWindowClosed() {
            iTableLayoutPanel.Controls.Clear();
            iContentPanel.Controls.Clear();
            iTableLayoutPanel = null;
            iContentPanel = null;
            iLblText = null;
            iLblTitle = null;

            base.OnWindowClosed();
        }

        /// <summary>
        /// Creates the controls used by this component.
        /// </summary>
        private void CreateControls() {
            iTableLayoutPanel = new TableLayoutPanel();

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

            iLblTitle = new Label {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Margin = new Padding(5)
            };
            gradientBackgroundLayoutPanel.Controls.Add(iLblTitle, 0, 0);

            iLblText = new Label {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(5)
            };
            gradientBackgroundLayoutPanel.Controls.Add(iLblText, 0, 1);

            Separator separator = new Separator {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            iContentPanel = new Panel {
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            // tableLayoutPanel
            iTableLayoutPanel.ColumnCount = 1;
            iTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            iTableLayoutPanel.Controls.Add(backgroundPanel, 0, 0);
            iTableLayoutPanel.Controls.Add(separator, 0, 1);
            iTableLayoutPanel.Controls.Add(iContentPanel, 0, 2);
            iTableLayoutPanel.Dock = DockStyle.Fill;
            iTableLayoutPanel.Margin = new Padding(0);
            iTableLayoutPanel.RowCount = 5;
            iTableLayoutPanel.RowStyles.Add(new RowStyle());
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        }
    }
}