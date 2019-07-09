using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary> Defines a text block. </summary>
    public interface ITextBlock : IWidget {
        /// <summary> Returns the text or does set it. </summary>
        string Text { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="ITextBlock"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PTextBlock : PWidgetControl<ITextBlock> {
        /// <summary> Text property meta data </summary>
        public static readonly IBindableProperty<string> TextProperty = CreatePropertyInfo(nameof(ITextBlock.Text), (string) null);
    }
}