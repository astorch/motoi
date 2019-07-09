using xcite.csharp;

namespace motoi.platform.ui.shells {
    /// <summary> Defines kinds of window states. </summary>
    public class EWindowState : XEnum<EWindowState> {

        /// <summary> Indicates a normal window state. </summary>
        public static readonly EWindowState Normal = new EWindowState("Normal");

        /// <summary> Indicates a maximized window state. </summary>
        public static readonly EWindowState Maximized = new EWindowState("Maximized");

        /// <summary> Indicates a minimized window state. </summary>
        public static readonly EWindowState Minimized = new EWindowState("Minimized");

        /// <inheritdoc />
        private EWindowState(object uniqueReference) : base(uniqueReference) {
            // Nothing to do
        }
    }
}