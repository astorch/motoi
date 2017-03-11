using System;
using System.Windows.Forms;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="ICheckBox"/>.
    /// </summary>
    public class CheckBox : System.Windows.Forms.CheckBox, ICheckBox {

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.CheckBox" /> class.</summary>
        public CheckBox() {
            InitializeComponent();
        }

        #region ICheckBox

        /// <summary>
        /// Returns the text of the check box or does set it.
        /// </summary>
        string ICheckBox.Text {
            get { return PCheckBox.GetModelValue<string>(this, PCheckBox.TextProperty); }
            set {
                PCheckBox.SetModelValue(this, PCheckBox.TextProperty, value);
                Text = value;
            }
        }

        /// <summary>
        /// Returns TRUE if the box is check or does set it.
        /// </summary>
        bool? ICheckBox.IsChecked {
            get { return PCheckBox.GetModelValue<bool?>(this, PCheckBox.IsCheckedProperty); }
            set {
                PCheckBox.SetModelValue(this, PCheckBox.IsCheckedProperty, value);
                CheckState = (!value.HasValue
                    ? CheckState.Indeterminate
                    : (value.Value ? CheckState.Checked : CheckState.Unchecked));
            }
        }

        /// <summary>
        /// Returns TRUE if the box is enabled or does set it.
        /// </summary>
        bool ICheckBox.IsEnabled {
            get { return PCheckBox.GetModelValue<bool>(this, PCheckBox.IsEnabledProperty); }
            set {
                PCheckBox.SetModelValue(this, PCheckBox.IsEnabledProperty, value);
                Enabled = value;
            }
        }

        #endregion

        /// <summary>Raises the <see cref="E:System.Windows.Forms.CheckBox.CheckStateChanged" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnCheckStateChanged(EventArgs e) {
            bool? checkedValue = CheckState == CheckState.Indeterminate
                ? (bool?)null
                : (CheckState == CheckState.Checked);
            PCheckBox.SetModelValue(this, PCheckBox.IsCheckedProperty, checkedValue, EBindingSourceUpdateReason.PropertyChanged);
            base.OnCheckStateChanged(e);
        }

        /// <summary>
        /// Notifies the instance to initialize its content.
        /// </summary>
        private void InitializeComponent() {
            Margin = new Padding(8, 5, 5, 5);
        }
    }
}