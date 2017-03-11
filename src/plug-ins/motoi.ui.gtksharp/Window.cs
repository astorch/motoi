using Gtk;
using motoi.platform.ui.menus;
using motoi.platform.ui.shells;
using motoi.workbench.model;

namespace Motoi.UI.GtkSharp {
    /// <summary>
    /// Provides an implementation of <see cref="IMainWindow"/> based on Gtk 
    /// API.
    /// </summary>
    public class Window : Gtk.Window, IMainWindow {

        public Window(string title) : base(title) {
        }

        public Window() : this(string.Empty) { }

        /// <summary>
        /// Returns the title of the window or does set it.
        /// </summary>
        public string WindowTitle {
            get { return Title; }
            set {
                Title = value;
                Notify("Title");
            }
        }

        /// <summary>
        /// Returns the width of the window or does set it.
        /// </summary>
        public int WindowWidth {
            get { return DefaultWidth; }
            set {
                DefaultWidth = value;
                Notify("DefaultWidth");
            }
        }

        /// <summary>
        /// Returns the height of the window or does set it.
        /// </summary>
        public int WindowHeight {
            get { return DefaultHeight; }
            set {
                DefaultHeight = value;
                Notify("DefaultHeight");
            }
        }

        /// <summary>
        /// Tells to window to close itself.
        /// </summary>
        public void Close() {
            base.Destroy();
            Application.Quit();
        }

        private MenuBar iMenuBar;
        private VBox iVBox;

        /// <summary>
        /// Tells the window to the add the given menu.
        /// </summary>
        /// <param name="menu">Menu and its item to add</param>
        public void AddMenu(MenuContribution menu) {
            if (iMenuBar == null) {
                iMenuBar = new MenuBar();
                iVBox = new VBox(false, 2);
                Add(iVBox);
            }
            
            Menu fileMenu = new Menu();

            MenuItem fileMenuItem = new MenuItem(menu.Label.Replace('&', '_'));
            fileMenuItem.Submenu = fileMenu;

            foreach (MenuItemContribution mi in menu.MenuItems) {
                MenuItem mnuItem = new MenuItem(mi.Label.Replace('&', '_'));
                mnuItem.Activated += (sender, args) => mi.ActionHandler.Run();
                mnuItem.SetStateFlags(mi.ActionHandler.IsEnabled ? StateFlags.Normal : StateFlags.Insensitive, false);
                fileMenu.Append(mnuItem);
            }

            iMenuBar.Append(fileMenuItem);
            iVBox.PackStart(iMenuBar, false, false, 0);
        }

        /// <summary>
        /// Returns the current active perspective or does set it.
        /// </summary>
        public IPerspective ActivePerspective { get; set; }
    }
}