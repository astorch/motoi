﻿using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary> Defines a check box. </summary>
    public interface ICheckBox : IWidget {
        /// <summary> Returns the text of the check box or does set it. </summary>
        string Text { get; set; }

        /// <summary> Returns TRUE if the box is check or does set it. </summary>
        bool? IsChecked { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="ICheckBox"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PCheckBox : PWidgetControl<ICheckBox> {
        /// <summary>Text property meta data</summary>
        public static readonly IBindableProperty<string> TextProperty = CreatePropertyInfo(nameof(ICheckBox.Text), string.Empty);

        /// <summary> Is checked property meta data </summary>
        public static readonly IBindableProperty<bool?> IsCheckedProperty = CreatePropertyInfo(nameof(ICheckBox.IsChecked), (bool?)false, true, EDataBindingSourceUpdateTrigger.PropertyChanged);
    }
}