using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines a text box.
    /// </summary>
    public interface ITextBox : IWidget, IDataBindingSupport {
        /// <summary>
        /// Returns the text content of the text box or does set it.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Returns the curent cursor index of the text box or does set it.
        /// </summary>
        int CursorIndex { get; set; }

        /// <summary>
        /// Returns TRUE if the text box is read only and therefore the text cannot be edited.
        /// </summary>
        bool ReadOnly { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="ITextBox"/> that is used by data binding operations.
    /// </summary>
    public class PTextBox : PTextBoxControl<ITextBox> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="ITextBox"/> that is used by data binding operations.
    /// </summary>
    public class PTextBoxControl<TControl> : PWidget<TControl> where TControl : IDataBindingSupport {
        /// <summary>Text property meta data</summary>
        public static readonly IBindableProperty TextProperty = CreatePropertyInfo("Text", (string)null, true, EDataBindingSourceUpdateTrigger.PropertyChanged);

        /// <summary>Cursor index property meta data</summary>
        public static readonly IBindableProperty CursorIndexProperty = CreatePropertyInfo("CursorIndex", 0, true, EDataBindingSourceUpdateTrigger.PropertyChanged);

        /// <summary> Read only property meta data </summary>
        public static readonly IBindableProperty ReadOnlyProperty = CreatePropertyInfo("ReadOnly", false);
    }
}