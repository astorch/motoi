using motoi.workbench.model;
using WeifenLuo.WinFormsUI.Docking;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="IMultiViewPerspective"/>.
    /// </summary>
    public class MultiViewPerspective : SingleViewPerspective, IMultiViewPerspective, IDockableControl {
        /// <summary>
        /// Returns the <see cref="DocumentStyle"/> for the given <paramref name="dockPanel"/> based on the type 
        /// of this class. This class is intended to be overridden.
        /// </summary>
        /// <param name="dockPanel">Dock panel</param>
        /// <returns>Document style</returns>
        protected override DocumentStyle GetDocumentStyle(DockPanel dockPanel) {
            return DocumentStyle.DockingMdi;
        }

        /// <summary>
        /// Converts the given <paramref name="viewPosition"/> into the corresponding <see cref="DockState"/>. 
        /// If there is no corresponding element, <see cref="DockState.DockLeftAutoHide"/> is returned.
        /// </summary>
        /// <param name="viewPosition">View position to convert</param>
        /// <returns>Corresponding <see cref="DockState"/></returns>
        protected override DockState ConvertToDockState(EViewPosition viewPosition) {
            if (viewPosition == EViewPosition.Left) return DockState.DockLeft;
            if (viewPosition == EViewPosition.Top) return DockState.DockTop;
            if (viewPosition == EViewPosition.Right) return DockState.DockRight;
            if (viewPosition == EViewPosition.Bottom) return DockState.DockBottom;
            if (viewPosition == EViewPosition.LeftCollapsed) return DockState.DockLeftAutoHide;
            if (viewPosition == EViewPosition.RightCollapsed) return DockState.DockRightAutoHide;
            if (viewPosition == EViewPosition.BottomCollapsed) return DockState.DockBottomAutoHide;
            if (viewPosition == EViewPosition.TopCollapsed) return DockState.DockTopAutoHide;

            return DockState.DockLeftAutoHide;
        }
    }
}