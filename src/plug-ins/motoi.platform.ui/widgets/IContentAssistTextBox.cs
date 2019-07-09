using System;
using System.Collections.Generic;
using motoi.platform.ui.bindings;
using motoi.platform.ui.images;

namespace motoi.platform.ui.widgets {
    /// <summary> Defines a rich text box with content assist. </summary>
    public interface IContentAssistTextBox : IRichTextBox {
        /// <summary> Returns the currently used content assist model does set it. </summary>
        IContentAssistModel ContentAssistModel { get; set; }

        /// <summary> Returns the currently used content syntax validator or does set it. </summary>
        IContentSyntaxValidator ContentSyntaxValidator { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IContentAssistTextBox"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PContentAssistTextBox : PRichTextBoxControl<IContentAssistTextBox> {
        /// <summary> Content assist model property meta data </summary>
        public static readonly IBindableProperty<IContentAssistModel> ContentAssistModelProperty = CreatePropertyInfo(nameof(IContentAssistTextBox.ContentAssistModel), (IContentAssistModel)null);

        /// <summary> Content syntax validator property meta data </summary>
        public static readonly IBindableProperty<IContentSyntaxValidator> ContentSyntaxValidatorProperty = CreatePropertyInfo(nameof(IContentAssistTextBox.ContentSyntaxValidator), (IContentSyntaxValidator)null);
    }

    /// <summary> Defines the content assist model. </summary>
    public interface IContentAssistModel {
        /// <summary> Returns the trigger pattern for the content assist. </summary>
        string SearchPattern { get; }

        /// <summary> Returns the minimum input fragment length to open the content assist box. </summary>
        int MinFragmentLength { get; }

        /// <summary> Returns the time in milliseconds until the content assist box is shown. </summary>
        int AppearInterval { get; }

        /// <summary> Returns TRUE if the content assist box can be closed using TAB. </summary>
        bool AllowTab { get; }

        /// <summary>
        /// Returns TRUE if the infotip is permanently shown and won't disappear
        /// until the content assist box is closed.
        /// </summary>
        bool AlwaysShowInfotip { get; }

        /// <summary> Returns the duration (ms) the infotip is shown to the user. </summary>
        int InfotipDuration { get; }

        /// <summary> Returns the item provider for the content assist. </summary>
        IContentAssistItemProvider ItemProvider { get; }

        /// <summary>
        /// Notifies the instance that an element of the
        /// <see cref="ItemProvider"/> has been inserted.
        /// </summary>
        /// <param name="item">Item that has been inserted</param>
        /// <param name="index">Index the item has been inserted</param>
        void OnItemInserted(ContentAssistItem item, int index);
    }

    /// <summary> Defines an autocomplete item provider that can be applied to an <see cref="IRichTextBox"/>. </summary>
    public interface IContentAssistItemProvider {
        /// <summary>
        /// Notifies the instance to provide the content assist items according to the 
        /// given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">User input that raised the content assist box.</param>
        /// <returns>Set of content assist items to show</returns>
        IEnumerable<ContentAssistItem> ProvideItems(string text);
    }

    /// <summary> Describes a content assist item. </summary>
    public class ContentAssistItem {
        /// <summary>
        /// Creates a new instance with the given value.
        /// </summary>
        /// <param name="text">Text of the item. Must not be NULL or empty!</param>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is NULL or empty</exception>
        public ContentAssistItem(string text) : this(text, null) {
            // Nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given values. All values can be NULL except of 
        /// <paramref name="text"/>. Note, no infotip is applied.
        /// </summary>
        /// <param name="text">Text of the item. Must not be NULL or empty!</param>
        /// <param name="image">Image of the item</param>
        public ContentAssistItem(string text, ImageDescriptor image) : this(text, image, null, null) {
            // Nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given values. All values can be NULL except of 
        /// <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text of the item. Must not be NULL or empty!</param>
        /// <param name="image">Image of the item</param>
        /// <param name="infotipTitle">Infotip title</param>
        /// <param name="infotipText">Infotip text</param>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is NULL or empty</exception>
        public ContentAssistItem(string text, ImageDescriptor image, string infotipTitle, string infotipText) {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            Text = text;
            Image = image;
            InfotipTitle = infotipTitle;
            InfotipText = infotipText;
        }

        /// <summary> Returns the text of the item. </summary>
        public string Text { get; }

        /// <summary> Returns the image descriptor of the item. </summary>
        public ImageDescriptor Image { get; }

        /// <summary> Returns the infotip title of the item. </summary>
        public string InfotipTitle { get; }

        /// <summary> Returns the infotip text of the item. </summary>
        public string InfotipText { get; }

        /// <summary> Returns the tag of this item or does set it. </summary>
        public object Tag { get; set; }

        /// <summary> Returns the text that is used to display the item in the content assist box. </summary>
        public string ListText { get; set; }
    }

    /// <summary> Defines a syntax validator that can be applied to an <see cref="IRichTextBox"/>. </summary>
    public interface IContentSyntaxValidator {
        /// <summary>
        /// Tells the instance to validate the given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text to validate</param>
        /// <returns>Collection of resolved rich text syntax hints</returns>
        IEnumerable<IContentSyntaxValidationHint> Validate(string text);
    }

    /// <summary> Describes a content syntax validation hint. </summary>
    public interface IContentSyntaxValidationHint {
        /// <summary>  Returns the type of the hint. </summary>
        EContentValidationSyntaxHintType Type { get; }

        /// <summary> Returns the message for the hint. </summary>
        string Message { get; }

        /// <summary>
        /// Returns the region column index where the hint starts. 0 means the first character.
        /// </summary>
        int BeginColumn { get; }

        /// <summary> Returns the region column index where the hint ends. </summary>
        int EndColumn { get; }

        /// <summary> Returns the region line index where the hint starts. 0 means the first line. </summary>
        int BeginLine { get; }

        /// <summary> Returns the region line index where the hint ends.  </summary>
        int EndLine { get; }
    }

    /// <summary> Defines types of rich text syntax hints. </summary>
    public enum EContentValidationSyntaxHintType {
        /// <summary> Indicates an informing hint. </summary>
        Info,

        /// <summary> Indicates a warning hint. </summary>
        Warning,

        /// <summary> Indicates an error hint. </summary>
        Error
    }
}