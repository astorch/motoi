using System.Drawing;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="ITextBlock"/>.
    /// </summary>
    public class TextBlock : Label, ITextBlock {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public TextBlock() {
            InitializeComponent();
        }

        #region ITextBlock

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PTextBlock.GetModelValue(this, PTextBlock.VisibilityProperty); }
            set {
                PTextBlock.SetModelValue(this, PTextBlock.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            }
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PTextBlock.GetModelValue(this, PTextBlock.EnabledProperty); }
            set {
                PTextBlock.SetModelValue(this, PTextBlock.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <inheritdoc />
        string ITextBlock.Text {
            get { return PTextBlock.GetModelValue(this, PTextBlock.TextProperty); }
            set {
                PTextBlock.SetModelValue(this, PTextBlock.TextProperty, value);
                Text = value;
            }
        }

        #endregion

        /// <summary>
        /// Notifies the instance to initialize its content.
        /// </summary>
        private void InitializeComponent() {
            TextAlign = ContentAlignment.MiddleLeft;
            Margin = new Padding(5);
        }
    }
}