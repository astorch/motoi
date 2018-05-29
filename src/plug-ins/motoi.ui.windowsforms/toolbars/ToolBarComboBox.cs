using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using motoi.platform.ui.bindings;
using motoi.platform.ui.toolbars;
using xcite.collections.nogen;

namespace motoi.ui.windowsforms.toolbars {
    /// <summary>
    /// Provides an implementation of <see cref="IToolBarComboBox"/>.
    /// </summary>
    public class ToolBarComboBox : ToolStripComboBox, IToolBarComboBox {
        /// <summary>
        /// Event that occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns the items source of the items selector or does set it.
        /// </summary>
        ICollection IToolBarComboBox.ItemsSource {
            get { return PToolBarComboBox.GetModelValue<ICollection>(this, PToolBarComboBox.ItemsSourceProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.ItemsSourceProperty, value);
                
                // Update the control collection
                Items.Clear();
                if (value == null) return;
                
                object[] items = value.ToArray();
                Items.AddRange(items);
            }
        }

        /// <summary>
        /// Returns the selected item or does set it.
        /// </summary>
        object IToolBarComboBox.SelectedItem {
            get { return PToolBarComboBox.GetModelValue<object>(this, PToolBarComboBox.SelectedItemProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.SelectedItemProperty, value);
                SelectedItem = value;
            }
        }

        /// <summary>
        /// Returns TRUE if the control is enabled or does set it.
        /// </summary>
        bool IToolBarControl.IsEnabled {
            get { return PToolBarComboBox.GetModelValue<bool>(this, PToolBarComboBox.IsEnabledProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.IsEnabledProperty, value);
                Enabled = value;
            }
        }

        /// <summary>
        /// Returns TRUE if the item is editable.
        /// </summary>
        bool IToolBarComboBox.Editable {
            get { return PToolBarComboBox.GetModelValue<bool>(this, PToolBarComboBox.EditableProperty); }
            set {
                PToolBarComboBox.SetModelValue(this, PToolBarComboBox.EditableProperty, value);
                DropDownStyle = value ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            }
        }

        /// <summary>
        /// Returns the current width of the control or does set it.
        /// </summary>
        int IToolBarControl.Width {
            get { return Size.Width; }
            set { Size = new Size(value, Size.Height); }
        }

        /// <summary>
        /// Löst das <see cref="E:System.Windows.Forms.ToolStripComboBox.SelectedIndexChanged"/>-Ereignis aus.
        /// </summary>
        /// <param name="e">Ein <see cref="T:System.EventArgs"/>, der die Ereignisdaten enthält.</param>
        protected override void OnSelectedIndexChanged(EventArgs e) {
            PToolBarComboBox.SetModelValue(this, PToolBarComboBox.SelectedItemProperty, SelectedItem, EBindingSourceUpdateReason.PropertyChanged);
            base.OnSelectedIndexChanged(e);
        }

        /// <summary>
        /// Löst das <see cref="E:System.Windows.Forms.ToolStripControlHost.LostFocus"/>-Ereignis aus.
        /// </summary>
        /// <param name="e">Ein <see cref="T:System.EventArgs"/>, das die Ereignisdaten enthält.</param>
        protected override void OnLostFocus(EventArgs e) {
            PToolBarComboBox.SetModelValue(this, PToolBarComboBox.SelectedItemProperty, SelectedItem, EBindingSourceUpdateReason.LostFocus);
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Gibt die vom <see cref="T:System.Windows.Forms.ToolStripControlHost"/> verwendeten nicht verwalteten Ressourcen und optional auch die verwalteten Ressourcen frei.
        /// </summary>
        /// <param name="disposing">true, um sowohl verwaltete als auch nicht verwaltete Ressourcen freizugeben. false, um ausschließlich nicht verwaltete Ressourcen freizugeben. </param>
        protected override void Dispose(bool disposing) {
            PToolBarComboBox.DisposeInstance(this);
            base.Dispose(disposing);
        }
    }
}