﻿using System;
using System.Collections;
using System.Windows.Forms;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;
using Xcite.Collections;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="IComboBox"/>.
    /// </summary>
    public class ComboBox : System.Windows.Forms.ComboBox, IComboBox {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ComboBox() {
            InitializeComponent();
        }

        #region IComboBox

        /// <summary>
        /// Returns the items source of the control or does set it.
        /// </summary>
        ICollection IItemsHost.ItemsSource {
            get { return PComboBox.GetModelValue<ICollection>(this, PComboBox.ItemsSourceProperty); }
            set {
                PComboBox.SetModelValue(this, PComboBox.ItemsSourceProperty, value);
                OnItemsSourceChanged(value);
            }
        }

        /// <summary>
        /// Returns TRUE if the combo box can be edited or does set it.
        /// </summary>
        bool IComboBox.Editable {
            get { return PComboBox.GetModelValue<bool>(this, PComboBox.EditableProperty); }
            set {
                PComboBox.SetModelValue(this, PComboBox.EditableProperty, value);
                DropDownStyle = value ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            }
        }

        /// <summary>
        /// Returns the currently selected item or does set it.
        /// </summary>
        object IComboBox.SelectedItem {
            get { return PComboBox.GetModelValue<object>(this, PComboBox.SelectedItemProperty); }
            set {
                PComboBox.SetModelValue(this, PComboBox.SelectedItemProperty, value);
                SelectedItem = value;
            }
        }

        #endregion

        /// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.SelectedValueChanged" /> event. </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnSelectedValueChanged(EventArgs e) {
            PComboBox.SetModelValue(this, PComboBox.SelectedItemProperty, SelectedItem, EBindingSourceUpdateReason.PropertyChanged);
            base.OnSelectedValueChanged(e);
        }

        /// <summary>
        /// Is invoked when the current items source has been changed.
        /// </summary>
        /// <param name="itemsSource">New items source collection</param>
        protected virtual void OnItemsSourceChanged(ICollection itemsSource) {
            try {
                BeginUpdate();
                
                Items.Clear();
                itemsSource.ForEach(obj => Items.Add(obj));
                
            } finally {
                EndUpdate();
            }
        }

        /// <summary>
        /// Notifies the instance to initialize its content.
        /// </summary>
        private void InitializeComponent() {
            Margin = new Padding(5);
        }
    }
}