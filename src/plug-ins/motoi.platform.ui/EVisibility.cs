using Xcite.Csharp.lang;

namespace motoi.platform.ui {
    /// <summary>
    /// Defines kinds of visibility of widets.
    /// </summary>
    public class EVisibility : XEnum<EVisibility> {
        /// <summary> Indicates that the widget is fully visible. </summary>
        public static readonly EVisibility Visible = new EVisibility("Visible");

        /// <summary> Indicates that the widget is hidden but fills the target space. </summary>
        public static readonly EVisibility Hidden = new EVisibility("Hidden");

        /// <summary> Indicates that the widget is hidden and does not fills any space. </summary>
        public static readonly EVisibility Collapsed = new EVisibility("Collapsed");
        
        /// <inheritdoc />
        private EVisibility(object uniqueReference) : base(uniqueReference) {
            // Nothing to do here
        }
    }
}