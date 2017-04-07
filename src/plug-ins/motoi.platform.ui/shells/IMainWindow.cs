using motoi.platform.ui.menus;
using motoi.platform.ui.messaging;
using motoi.platform.ui.toolbars;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines the properties of an application main window.
    /// </summary>
    public interface IMainWindow : IWindow, IUIInvoker {
        /// <summary>
        /// Tells the window to add the given menu.
        /// </summary>
        /// <param name="menu">Menu and its item to add</param>
        void AddMenu(MenuContribution menu);

        /// <summary>
        /// Tells the window to add the given group to the toolbar.
        /// </summary>
        /// <param name="group"></param>
        void AddToolbarGroup(ToolbarGroupContribution group);
    }
}