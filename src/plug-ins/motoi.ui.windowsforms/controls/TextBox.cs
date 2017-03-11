using System;
using System.Windows.Forms;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="ITextBox"/>.
    /// </summary>
    public class TextBox : System.Windows.Forms.TextBox, ITextBox {

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TextBox" /> class.</summary>
        public TextBox() {
            InitializeComponent();
        }

        #region ITextBox

        /// <summary>
        /// Returns the curent cursor index of the text box or does set it.
        /// </summary>
        int ITextBox.CursorIndex {
            get { return PTextBox.GetModelValue<int>(this, PTextBox.CursorIndexProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.CursorIndexProperty, value);
                SelectionStart = value;
            }
        }

        /// <summary>
        /// Returns the text content of the text box or does set it.
        /// </summary>
        string ITextBox.Text {
            get { return PTextBox.GetModelValue<string>(this, PTextBox.TextProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.TextProperty, value);
                Text = value;
            }
        }

        /// <summary>
        /// Returns TRUE if the text box is read only and therefore the text cannot be edited.
        /// </summary>
        bool ITextBox.ReadOnly {
            get { return PTextBox.GetModelValue<bool>(this, PTextBox.ReadOnlyProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.ReadOnlyProperty, value);
                ReadOnly = value;
            }
        }

        #endregion

        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnTextChanged(EventArgs e) {
            PTextBox.SetModelValue(this, PTextBox.TextProperty, Text, EBindingSourceUpdateReason.PropertyChanged);
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Notifies the instance to initialize its content.
        /// </summary>
        private void InitializeComponent() {
            Margin = new Padding(5);
        }
    }
}