using motoi.platform.ui.bindings;

namespace motoi.platform.ui.shells {
    /// <summary> Defines the properties of a Titled Area Dialog. </summary>
    public interface ITitledAreaDialog : IDialogWindow {
        /// <summary> Returns the title of the dialog or does set it. </summary>
        string Title { get; set; }

        /// <summary> Returns the description text of the dialog or does set it. </summary>
        string Description { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="ITitledAreaDialog"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PTitledAreaDialog : PDialogWindowControl<ITitledAreaDialog> {
        /// <summary> Title property meta data </summary>
        public static readonly IBindableProperty<string> TitleProperty = CreatePropertyInfo(nameof(ITitledAreaDialog.Title), string.Empty);

        /// <summary> Description property meta data </summary>
        public static readonly IBindableProperty<string> DescriptionProperty = CreatePropertyInfo(nameof(ITitledAreaDialog.Description), string.Empty);
    }
}