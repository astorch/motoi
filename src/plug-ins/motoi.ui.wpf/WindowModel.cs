using System;
using System.Collections.Generic;
using motoi.platform.ui.bindings;
using motoi.platform.ui.menus;
using motoi.platform.ui.shells;
using motoi.workbench.model;
using Xcite.Collections;

namespace Motoi.UI.WPF
{
    /// <summary>
    /// Provides a MVVM-conform implementation of <see cref="IMainWindow"/>.
    /// </summary>
    public class WindowModel : PropertyChangedDispatcher, IMainWindow {
        private string iWindowTitle = string.Empty;
        private int iWindowWidth;
        private int iWindowHeight;
        private IList<MenuContribution> iMainMenuItems;

        public event EventHandler OnShow;
        public event EventHandler OnClose;

        /// <summary>
        /// Returns the title of the window or does set it.
        /// </summary>
        public string WindowTitle {
            get { return iWindowTitle; }
            set {
                iWindowTitle = value;
                DispatchPropertyChanged(() => WindowTitle);
            }
        }

        /// <summary>
        /// Returns the width of the window or does set it.
        /// </summary>
        public int WindowWidth {
            get { return iWindowWidth; }
            set {
                iWindowWidth = value;
                DispatchPropertyChanged(() => WindowWidth);
            }
        }

        /// <summary>
        /// Returns the height of the window or does set it.
        /// </summary>
        public int WindowHeight {
            get { return iWindowHeight; }
            set {
                iWindowHeight = value;
                DispatchPropertyChanged(() => WindowHeight);
            }
        }

        /// <summary>
        /// Returns the collection main menu items
        /// </summary>
        public IList<MenuContribution> MainMenuItems {
            get { return iMainMenuItems; }
        }

        /// <summary>
        /// Creates the window and makes it visible to the user.
        /// </summary>
        public void Show() {
            if (OnShow == null) return;
            OnShow(this, EventArgs.Empty);
        }

        /// <summary>
        /// Tells to window to close itself.
        /// </summary>
        public void Close() {
            if (OnClose == null) return;
            OnClose(this, EventArgs.Empty);
        }

        /// <summary>
        /// Tells the window to the add the given menu.
        /// </summary>
        /// <param name="menu">Menu and its item to add</param>
        public void AddMenu(MenuContribution menu) {
            if (iMainMenuItems == null)
                iMainMenuItems = new List<MenuContribution>();

            MenuContribution clone = new MenuContribution(menu.Id, menu.Label.Replace('&', '_'));
            ((IEnumerable<MenuItemContribution>)menu.MenuItems).ForEach(x => {
                    MenuItemContribution itemClone = new MenuItemContribution(
                        x.Id, x.Label.Replace('&','_'), x.Menu, x.ActionHandler, x.Shortcut, x.ImageStream);
                    clone.MenuItems.Add(itemClone);
                });
            iMainMenuItems.Add(clone);
            DispatchPropertyChanged(() => MainMenuItems);
        }

        /// <summary>
        /// Returns the current active perspective or does set it.
        /// </summary>
        public IPerspective ActivePerspective { get; set; }
    }
}