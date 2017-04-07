using motoi.platform.ui.bindings;
using motoi.platform.ui.images;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines common properties of a window.
    /// </summary>
    public interface IWindow : IShell {
        /// <summary> Returns the title of the window or does set it. </summary>
        string WindowTitle { get; set; }

        /// <summary> Returns the state of the window or does set it. </summary>
        EWindowState WindowState { get; set; }

        /// <summary> Returns the window startup location or does set it. </summary>
        EWindowStartupLocation WindowStartupLocation { get; set; }

        /// <summary> Returns the window style or does set it. </summary>
        EWindowStyle WindowStyle { get; set; }

        /// <summary> Returns the window resize mode or does set it. </summary>
        EWindowResizeMode WindowResizeMode { get; set; }

        /// <summary> Returns the window icon or does set it. </summary>
        ImageDescriptor WindowIcon { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IWindow"/> that is used by data binding operations.
    /// </summary>
    public class PWindow : PWindowControl<IWindow> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IWindow"/> that is used by data binding operations.
    /// </summary>
    public class PWindowControl<TWindow> : PShellControl<TWindow> where TWindow : class, IWindow {
        /// <summary> Window title property meta data </summary>
        public static readonly IBindableProperty<string> WindowTitleProperty = CreatePropertyInfo(_ => _.WindowTitle, "Untitled");

        
    }
}