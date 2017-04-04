using System.Collections.Generic;
using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines a rich text box.
    /// </summary>
    public interface IRichTextBox : ITextBox {
        /// <summary>
        /// Returns the currently used Rich Text Model or does set it.
        /// </summary>
        IRichTextModel RichTextModel { get; set; }

        /// <summary>
        /// Returns the currently used Rich Text Syntax validator or does set it.
        /// </summary>
        IRichTextSyntaxValidator RichTextSyntaxValidator { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IRichTextBox"/> that is used by data binding operations.
    /// </summary>
    public class PRichTextBox : PTextBoxControl<IRichTextBox> {
        /// <summary>Rich Text Model property meta data</summary>
        public static readonly IBindableProperty<IRichTextModel> RichTextModelProperty = CreatePropertyInfo("RichTextModel", (IRichTextModel)null);

        /// <summary> Rich Text Syntax Validator property meta data </summary>
        public static readonly IBindableProperty<IRichTextSyntaxValidator> RichTextSyntaxValidatorProperty = CreatePropertyInfo("RichTextSyntaxValidator", (IRichTextSyntaxValidator)null);
    }

    /// <summary>
    /// Defines model of rich text.
    /// </summary>
    public interface IRichTextModel {
        /// <summary>
        /// Returns a style name.
        /// </summary>
        string StyleName { get; }

        /// <summary>
        /// Returns the XML style data.
        /// </summary>
        byte[] XmlStyleData { get; }

        /// <summary>
        /// Returns the trigger pattern for the auto complete feature.
        /// </summary>
        string AutoCompleteSearchPattern { get; }

        /// <summary>
        /// Returns the minimum input fragment length to open the auto complete box.
        /// </summary>
        int AutoCompleteMinFragmentLength { get; }

        /// <summary>
        /// Returns the time in milliseconds until the auto complete box is shown.
        /// </summary>
        int AutoCompleteAppearInterval { get; }

        /// <summary>
        /// Returns TRUE if the auto complete box can be closed using TAB.
        /// </summary>
        bool AutoCompleteAllowTab { get; }

        /// <summary>
        /// Returns all items of the auto complete box.
        /// </summary>
        IEnumerable<string> AutoCompleteItems { get; }

        /// <summary>
        /// Notifies the instance that an element of the <see cref="AutoCompleteItems"/> has been inserted.
        /// </summary>
        /// <param name="item">Item that has been inserted</param>
        /// <param name="index">Index the item has been inserted</param>
        void OnItemInserted(string item, int index);
    }

    /// <summary>
    /// Defines a syntax validator that can be applied to an <see cref="IRichTextBox"/>.
    /// </summary>
    public interface IRichTextSyntaxValidator {
        /// <summary>
        /// Tells the instance to validate the given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text to validate</param>
        /// <returns>Collection of resolved rich text syntax hints</returns>
        IEnumerable<IRichTextSyntaxHint> Validate(string text);
    }

    /// <summary>
    /// Describes a rich text syntax hint.
    /// </summary>
    public interface IRichTextSyntaxHint {
        /// <summary> 
        /// Returns the type of the hint.
        /// </summary>
        ERichTextSyntaxHintType Type { get; }

        /// <summary>
        /// Returns the message for the hint.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Returns the region column index where the hint starts. 0 means the first character.
        /// </summary>
        int BeginColumn { get; }

        /// <summary>
        /// Returns the region column index where the hint ends.
        /// </summary>
        int EndColumn { get; }

        /// <summary>
        /// Returns the region line index where the hint starts. 0 means the first line.
        /// </summary>
        int BeginLine { get; }

        /// <summary>
        /// Returns the region line index where the hint ends. 
        /// </summary>
        int EndLine { get; }
    }

    /// <summary>
    /// Defines types of rich text syntax hints.
    /// </summary>
    public enum ERichTextSyntaxHintType {
        /// <summary>
        /// Indicates an informing hint.
        /// </summary>
        Info,

        /// <summary>
        /// Indicates a warning hint.
        /// </summary>
        Warning,

        /// <summary>
        /// Indicates an error hint.
        /// </summary>
        Error
    }
}