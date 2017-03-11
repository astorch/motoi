using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines a check box.
    /// </summary>
    public interface ICheckBox : IWidget, IDataBindingSupport {
        /// <summary>
        /// Returns the text of the check box or does set it.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Returns TRUE if the box is check or does set it.
        /// </summary>
        bool? IsChecked { get; set; }

        /// <summary>
        /// Returns TRUE if the box is enabled or does set it.
        /// </summary>
        bool IsEnabled { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="ICheckBox"/> that is used by data binding operations.
    /// </summary>
    public class PCheckBox : PWidget<ICheckBox> {
        /// <summary>Text property meta data</summary>
        public static readonly IBindableProperty TextProperty = CreatePropertyInfo(_ => _.Text, string.Empty);

        /// <summary> Is checked property meta data </summary>
        public static readonly IBindableProperty IsCheckedProperty = CreatePropertyInfo(_ => _.IsChecked, false, true, EDataBindingSourceUpdateTrigger.PropertyChanged);

        /// <summary> Is enabled property meta data </summary>
        public static readonly IBindableProperty IsEnabledProperty = CreatePropertyInfo(_ => _.IsEnabled, true, true);
    }
}