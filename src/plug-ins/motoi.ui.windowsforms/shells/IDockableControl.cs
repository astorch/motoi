using WeifenLuo.WinFormsUI.Docking;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Defines a dockable control that is going to be notified if it's attached to a dock panel.
    /// </summary>
    interface IDockableControl {
        /// <summary>
        /// Tells the control that it is going to be attached to the given <paramref name="dockPanel"/>.
        /// </summary>
        /// <param name="dockPanel">Dock panel this is instance is attached to</param>
        void Attach(DockPanel dockPanel);
    }
}