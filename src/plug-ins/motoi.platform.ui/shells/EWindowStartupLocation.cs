using Xcite.Csharp.lang;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines window startup locations.
    /// </summary>
    public class EWindowStartupLocation : XEnum<EWindowStartupLocation> {
        /// <summary> The startup location is set by code or determined by the windows default location. </summary>
        public static readonly EWindowStartupLocation Manual = new EWindowStartupLocation("Manuel");

        /// <summary> The startup location is the center of the window that owns it. </summary>
        public static readonly EWindowStartupLocation CenterOwner = new EWindowStartupLocation("CenterOwner");

        /// <summary> The startup location is the center of the screen that currently contains the mouse cursor. </summary>
        public static readonly EWindowStartupLocation CenterScreen = new EWindowStartupLocation("CenterScreen");

        /// <inheritdoc />
        private EWindowStartupLocation(object uniqueReference) : base(uniqueReference) {
            // Nothing to do here
        }
    }
}