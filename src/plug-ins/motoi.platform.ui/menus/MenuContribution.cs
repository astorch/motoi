using System.Collections.Generic;

namespace motoi.platform.ui.menus {
    /// <summary> Defines a menu contribtion. </summary>
    public class MenuContribution {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="id">Id of the menu</param>
        /// <param name="label">Label of the menu</param>
        public MenuContribution(string id, string label) {
            Id = id;
            Label = label;
            MenuItems = new List<MenuItemContribution>(10);
        }

        /// <summary> Returns the id of the menu. </summary>
        public string Id { get; private set; }

        /// <summary>  Returns the label of the menu. </summary>
        public string Label { get; private set; }

        /// <summary>  Returns the associated menu items. </summary>
        public IList<MenuItemContribution> MenuItems { get; private set; }
    }
}