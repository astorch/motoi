using motoi.platform.ui.actions;
using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary> Defines a button. </summary>
    public interface IButton : IWidget {
        /// <summary> Returns the text of the button or does set it. </summary>
        string Text { get; set; }

        /// <summary> Returns the action handle of the button or does set it. </summary>
        IActionHandler ActionHandler { get; set; }

        /// <summary> Refreshes the button state. </summary>
        void Refresh();
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IButton"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PButton : PWidgetControl<IButton> {
        /// <summary> Text property meta data </summary>
        public static readonly IBindableProperty<string> TextProperty = CreatePropertyInfo(nameof(IButton.Text), string.Empty);

        /// <summary> Action handler property meta data </summary>
        public static readonly IBindableProperty<IActionHandler> ActionHandlerProperty = CreatePropertyInfo(nameof(IButton.ActionHandler), (IActionHandler) null);
    }
}