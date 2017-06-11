using xcite.csharp;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines window resize modes.
    /// </summary>
    public class EWindowResizeMode : XEnum<EWindowResizeMode> {
        /// <summary> The user cannot resize the window. There are no maximize or minimize boxes. </summary>
        public static readonly EWindowResizeMode NoResize = new EWindowResizeMode("NoResize");

        /// <summary> The user can fully resize the window. The maximize and minimize boxes are available. </summary>
        public static readonly EWindowResizeMode CanResize = new EWindowResizeMode("CanResize");

        /// <summary>
        /// The user can only minimize the window and restore it from the taskbar. The maximize box 
        /// is not available.
        /// </summary>
        public static readonly EWindowResizeMode CanMinimize = new EWindowResizeMode("CanMinimize");
        
        /// <inheritdoc />
        private EWindowResizeMode(object uniqueReference) : base(uniqueReference) {
            // Nothing to do here
        }
    }
}