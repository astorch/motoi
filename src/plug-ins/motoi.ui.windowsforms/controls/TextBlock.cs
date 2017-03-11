using System.Drawing;
using System.Windows.Forms;
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

        /// <summary>
        /// Returns the text or does set it.
        /// </summary>
        string ITextBlock.Text {
            get { return PTextBlock.GetModelValue<string>(this, PTextBlock.TextProperty); }
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