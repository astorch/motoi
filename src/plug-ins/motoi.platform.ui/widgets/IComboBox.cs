using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines a combo box.
    /// </summary>
    public interface IComboBox : IItemsHost {
        /// <summary>
        /// Returns TRUE if the combo box can be edited or does set it.
        /// </summary>
        bool Editable { get; set; }

        /// <summary>
        /// Returns the currently selected item or does set it.
        /// </summary>
        object SelectedItem { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IComboBox"/> that is used by data binding operations.
    /// </summary>
    public class PComboBox : PItemsHostControl<IComboBox> {
        /// <summary>Editable property meta data</summary>
        public static readonly IBindableProperty<bool> EditableProperty = CreatePropertyInfo(_ => _.Editable, true);

        /// <summary>SelectedItem property meta data</summary>
        public static readonly IBindableProperty<object> SelectedItemProperty = CreatePropertyInfo(_ => _.SelectedItem, null, true);
    }
}