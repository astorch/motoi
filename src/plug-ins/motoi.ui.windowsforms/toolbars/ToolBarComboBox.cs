using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using motoi.platform.ui.bindings;
using motoi.platform.ui.toolbars;

namespace motoi.ui.windowsforms.toolbars {
    /// <summary> Provides an implementation of <see cref="IToolBarComboBox"/>. </summary>
    public class ToolBarComboBox : ToolStripComboBox, IToolBarComboBox {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <inheritdoc />
        ICollection IToolBarComboBox.ItemsSource {
            get { return PToolBarComboBox.GetModelValue<ICollection>(this, PToolBarComboBox.ItemsSourceProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.ItemsSourceProperty, value);
                
                // Update the control collection
                Items.Clear();
                if (value == null) return;
                
                object[] items = new object[value.Count];
                value.CopyTo(items, 0);
                
                Items.AddRange(items);
            }
        }
        
        /// <inheritdoc />
        object IToolBarComboBox.SelectedItem {
            get { return PToolBarComboBox.GetModelValue<object>(this, PToolBarComboBox.SelectedItemProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.SelectedItemProperty, value);
                SelectedItem = value;
            }
        }

        /// <inheritdoc />
        bool IToolBarControl.IsEnabled {
            get { return PToolBarComboBox.GetModelValue<bool>(this, PToolBarComboBox.IsEnabledProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.IsEnabledProperty, value);
                Enabled = value;
            }
        }
        
        /// <inheritdoc />
        bool IToolBarComboBox.Editable {
            get { return PToolBarComboBox.GetModelValue<bool>(this, PToolBarComboBox.EditableProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.EditableProperty, value);
                DropDownStyle = value ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            }
        }

        /// <inheritdoc />
        int IToolBarControl.Width {
            get { return Size.Width; }
            set { Size = new Size(value, Size.Height); }
        }
        
        /// <inheritdoc />
        protected override void OnSelectedIndexChanged(EventArgs e) {
            PToolBarComboBox.SetModelValue(this, PToolBarComboBox.SelectedItemProperty, SelectedItem, EBindingSourceUpdateReason.PropertyChanged);
            base.OnSelectedIndexChanged(e);
        }
        
        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e) {
            PToolBarComboBox.SetModelValue(this, PToolBarComboBox.SelectedItemProperty, SelectedItem, EBindingSourceUpdateReason.LostFocus);
            base.OnLostFocus(e);
        }
        
        /// <inheritdoc />
        protected override void Dispose(bool disposing) {
            PToolBarComboBox.DisposeInstance(this);
            base.Dispose(disposing);
        }
    }
}