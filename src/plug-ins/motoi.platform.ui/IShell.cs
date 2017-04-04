using motoi.platform.ui.bindings;

namespace motoi.platform.ui {
    /// <summary>
    /// Defines a shell for widgets.
    /// </summary>
    public interface IShell : IWidgetCompound, IDataBindingSupport {
        /// <summary>
        /// Creates the shell and makes it visible to the user.
        /// </summary>
        void Show();

        /// <summary>
        /// Closes the shell.
        /// </summary>
        void Close();

        /// <summary>
        /// Returns the current active view content.
        /// </summary>
        IWidgetCompound Content { get; }

        /// <summary>
        /// Sets the given <paramref name="widgetCompound"/> as view content.
        /// </summary>
        /// <param name="widgetCompound">View content to set</param>
        void SetContent(IWidgetCompound widgetCompound);
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IShell"/> that is used by data binding operations.
    /// </summary>
    public class PShell<TShell> : PWigdetCompount<TShell> where TShell : class, IShell {
        
    }
}