using Xcite.Csharp.lang;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines window styles.
    /// </summary>
    public class EWindowStyle : XEnum<EWindowStyle> {
        /// <summary> A window that has no title bar and no border elements. </summary>
        public static readonly EWindowStyle BlankWindow = new EWindowStyle("BlankWindow"); // None

        /// <summary> A standard window with title bar and border elements. </summary>
        public static readonly EWindowStyle DefaultWindow = new EWindowStyle("DefaultWindow"); // SingleBorder | Three3dBorder

        /// <summary> A fixed tool window. </summary>
        public static readonly EWindowStyle ToolWindow = new EWindowStyle("ToolWindow"); // ToolWindow

        /// <inheritdoc />
        private EWindowStyle(object uniqueReference) : base(uniqueReference) {
            // Nothing to do here
        }
    }
}