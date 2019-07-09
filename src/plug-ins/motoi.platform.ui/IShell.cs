using motoi.platform.ui.bindings;

namespace motoi.platform.ui {
    /// <summary> Defines a shell for widgets. </summary>
    public interface IShell : IWidgetCompound {
        /// <summary> Returns the owner of the window or does set it. </summary>
        IShell Owner { get; set; }

        /// <summary> Returns the width of the shell or does set it. </summary>
        int Width { get; set; }

        /// <summary> Returns the height of the shell or does set it. </summary>
        int Height { get; set; }

        /// <summary> Returns the top location of the shell or does set it. </summary>
        int TopLocation { get; set; }

        /// <summary> Returns the left location of the shell or does set it. </summary>
        int LeftLocation { get; set; }

        /// <summary> Creates the shell and makes it visible to the user. </summary>
        void Show();

        /// <summary> Closes the shell. </summary>
        void Close();

        /// <summary> Returns the current active view content. </summary>
        IWidgetCompound Content { get; }

        /// <summary> Sets the given <paramref name="widgetCompound"/> as view content. </summary>
        /// <param name="widgetCompound">View content to set</param>
        void SetContent(IWidgetCompound widgetCompound);
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IShell"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PShell : PShellControl<IShell> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IShell"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PShellControl<TShell> : PWigdetCompoundControl<TShell> where TShell : class, IShell {
        /// <summary> Width property meta data </summary>
        public static readonly IBindableProperty<int> WidthProperty = CreatePropertyInfo(nameof(IShell.Width), 400);
        
        /// <summary> Height property meta data </summary>
        public static readonly IBindableProperty<int> HeightProperty = CreatePropertyInfo(nameof(IShell.Height), 300);

        /// <summary> Left location property meta data </summary>
        public static readonly IBindableProperty<int> TopLocationProperty = CreatePropertyInfo(nameof(IShell.TopLocation), 0);

        /// <summary> Top location property meta data </summary>
        public static readonly IBindableProperty<int> LeftLocationProperty = CreatePropertyInfo(nameof(IShell.LeftLocation), 0);
    }
}