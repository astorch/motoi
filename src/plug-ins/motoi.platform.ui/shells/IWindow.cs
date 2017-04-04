using motoi.platform.ui.bindings;
using motoi.platform.ui.images;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines common properties of a window.
    /// </summary>
    public interface IWindow : IShell {
        /// <summary>
        /// Returns the title of the window or does set it.
        /// </summary>
        string WindowTitle { get; set; }

        /// <summary>
        /// Returns the width of the window or does set it.
        /// </summary>
        int WindowWidth { get; set; }

        /// <summary>
        /// Returns the height of the window or does set it.
        /// </summary>
        int WindowHeight { get; set; }

        /// <summary>
        /// Returns the state of the window or does set it.
        /// </summary>
        EWindowState WindowState { get; set; }

        /// <summary>
        /// Returns the window top location or does set it.
        /// </summary>
        int WindowTopLocation { get; set; }

        /// <summary>
        /// Returns the window left location or does set it.
        /// </summary>
        int WindowLeftLocation { get; set; }

        /// <summary>
        /// Returns the window icon or does set it.
        /// </summary>
        ImageDescriptor WindowIcon { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IWindow"/> that is used by data binding operations.
    /// </summary>
    public class PWindow<TControl> : PShell<TControl> where TControl : class, IWindow {
        /// <summary> Window title property meta data </summary>
        public static readonly IBindableProperty<string> WindowTitleProperty = CreatePropertyInfo(_ => _.WindowTitle, "Untitled");

        /// <summary> Window width property meta data </summary>
        public static readonly IBindableProperty<int> WindowWidthProperty = CreatePropertyInfo(_ => _.WindowWidth, 800);

        /// <summary> Window height property meta data </summary>
        public static readonly IBindableProperty<int> WindowHeightProperty = CreatePropertyInfo(_ => _.WindowHeight, 600);
    }
}