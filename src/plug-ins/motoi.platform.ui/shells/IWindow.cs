using motoi.platform.ui.images;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines common properties of a window.
    /// </summary>
    public interface IWindow : IViewPartComposite {
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

        /// <summary>
        /// Creates the window and makes it visible to the user.
        /// </summary>
        void Show();

        /// <summary>
        /// Tells to window to close itself.
        /// </summary>
        void Close();

        /// <summary>
        /// Returns the current active view content.
        /// </summary>
        IViewPart Content { get; }

        /// <summary>
        /// Sets the given <paramref name="viewPart"/> as view content.
        /// </summary>
        /// <param name="viewPart">View content to set</param>
        void SetContent(IViewPart viewPart);
    }
}