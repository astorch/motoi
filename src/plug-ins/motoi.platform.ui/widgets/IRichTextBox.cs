using System;
using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary> Defines a rich text box. </summary>
    public interface IRichTextBox : ITextBox {
        /// <summary> Returns the style definition of the text box or does set it. </summary>
        StyleDefinition StyleDefinition { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IRichTextBox"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PRichTextBox : PRichTextBoxControl<IRichTextBox> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IRichTextBox"/> that is used by data binding operations.
    /// </summary>
    public class PRichTextBoxControl<TControl> : PTextBoxControl<TControl> where TControl : class, IRichTextBox {
        /// <summary> Rich Text Style Definition property meta data </summary>
        public static readonly IBindableProperty<StyleDefinition> StyleDefinitionProperty = CreatePropertyInfo(nameof(IRichTextBox.StyleDefinition), (StyleDefinition)null);
    }

    /// <summary> Describes a rich text box style definition. </summary>
    public class StyleDefinition {
        /// <summary> Creates a new instance with the given values. </summary>
        /// <param name="styleName">Name of the style</param>
        /// <param name="styleData">Style data</param>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public StyleDefinition(string styleName, byte[] styleData) {
            if (string.IsNullOrEmpty(styleName)) throw new ArgumentNullException(nameof(styleName));

            StyleName = styleName;
            StyleData = styleData ?? throw new ArgumentNullException(nameof(styleData));
        }

        /// <summary> Returns the name of the style. </summary>
        public string StyleName { get; }

        /// <summary> Returns the style data. </summary>
        public byte[] StyleData { get; }
    }
}