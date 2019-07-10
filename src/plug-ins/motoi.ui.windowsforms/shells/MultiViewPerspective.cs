using motoi.workbench.model;
using WeifenLuo.WinFormsUI.Docking;

namespace motoi.ui.windowsforms.shells {
    /// <summary> Provides an implementation of <see cref="IMultiViewPerspective"/>. </summary>
    public class MultiViewPerspective : SingleViewPerspective, IMultiViewPerspective {
        /// <inheritdoc />
        protected override void ConfigureDockPanel(DockPanel dockPanel) {
            dockPanel.DocumentStyle = DocumentStyle.DockingMdi;
        }

        /// <inheritdoc />
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