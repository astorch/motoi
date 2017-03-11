using System.Collections;
using motoi.platform.ui.bindings;

namespace motoi.platform.ui.toolbars {
    /// <summary>
    /// Describes a combo box.
    /// </summary>
    public interface IToolBarComboBox : IToolBarControl {
        /// <summary>
        /// Returns the selected item or does set it.
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// Returns the items source of the items selector or does set it.
        /// </summary>
        ICollection ItemsSource { get; set; }

        /// <summary>
        /// Returns TRUE if the item is editable.
        /// </summary>
        bool Editable { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IToolBarComboBox"/> that is used by data binding operations.
    /// </summary>
    public class PToolBarComboBox : PToolBarControl<IToolBarComboBox> {
        /// <summary> SelectedItem property meta data </summary>
        public static readonly IBindableProperty SelectedItemProperty = CreatePropertyInfo<object>("SelectedItem", null, true);

        /// <summary> ItemsSource property meta data </summary>
        public static readonly IBindableProperty ItemsSourceProperty = CreatePropertyInfo("ItemsSource", new ArrayList(0));

        /// <summary> Editable property meta data  </summary>
        public static readonly IBindableProperty EditableProperty = CreatePropertyInfo("Editable", true);
    }
}