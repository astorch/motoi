using System.IO;
using motoi.platform.ui.actions;

namespace motoi.platform.ui.menus {
    /// <summary> Defines a menu item contribution. </summary>
    public class MenuItemContribution {
        /// <summary> Creates a new instance. </summary>
        /// <param name="id">Id of the menu item</param>
        /// <param name="label">Label of the menu item</param>
        /// <param name="menu">Parent menu</param>
        /// <param name="handler">Action handler</param>
        /// <param name="shortcut">Shortcut</param>
        /// <param name="imageStream">Stream to an image</param>
        public MenuItemContribution(string id, string label, string menu, IActionHandler handler, 
            string shortcut, Stream imageStream) {
            Id = id;
            Label = label;
            Menu = menu;
            ActionHandler = handler ?? ActionHandlerFactory.NullActionHandler;
            Shortcut = shortcut;
            ImageStream = imageStream;
        }

        /// <summary> Returns the id of the menu item. </summary>
        public string Id { get; }

        /// <summary> Returns the label of the menu item. </summary>
        public string Label { get; }

        /// <summary> Returns the name of the parent menu. </summary>
        public string Menu { get; }

        /// <summary> Returns the action handler. </summary>
        public IActionHandler ActionHandler { get; }

        /// <summary> Returns the shortcut. </summary>
        public string Shortcut { get; }

        /// <summary> Returns a stream to an image. </summary>
        public Stream ImageStream { get; }

        /// <summary> Returns TRUE if this item is a separator. </summary>
        public bool IsSeparator { get; set; }
    }
}