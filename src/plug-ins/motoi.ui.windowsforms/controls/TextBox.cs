using System;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary> Provides an implementation of <see cref="ITextBox"/>. </summary>
    public class TextBox : System.Windows.Forms.TextBox, ITextBox {
        /// <inheritdoc />
        public TextBox() {
            InitializeComponent();
        }

        #region ITextBox

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PTextBox.GetModelValue(this, PTextBox.VisibilityProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            }
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PTextBox.GetModelValue(this, PTextBox.EnabledProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <inheritdoc />
        int ITextBox.CursorIndex {
            get { return PTextBox.GetModelValue(this, PTextBox.CursorIndexProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.CursorIndexProperty, value);
                SelectionStart = value;
            }
        }

        /// <inheritdoc />
        string ITextBox.Text {
            get { return PTextBox.GetModelValue(this, PTextBox.TextProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.TextProperty, value);
                Text = value;
            }
        }

        /// <summary>
        /// Returns TRUE if the text box is read only and therefore the text cannot be edited.
        /// </summary>
        bool ITextBox.ReadOnly {
            get { return PTextBox.GetModelValue(this, PTextBox.ReadOnlyProperty); }
            set {
                PTextBox.SetModelValue(this, PTextBox.ReadOnlyProperty, value);
                ReadOnly = value;
            }
        }

        #endregion
        
        /// <inheritdoc />
        protected override void OnTextChanged(EventArgs e) {
            PTextBox.SetModelValue(this, PTextBox.TextProperty, Text, EBindingSourceUpdateReason.PropertyChanged);
            base.OnTextChanged(e);
        }

        /// <summary> Notifies the instance to initialize its content. </summary>
        private void InitializeComponent() {
            Margin = new Padding(5);
        }
    }
}