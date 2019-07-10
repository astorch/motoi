using xcite.csharp;

namespace motoi.workbench.model {
    /// <summary> Defines kinds of view positions. </summary>
    public class EViewPosition : XEnum<EViewPosition> {
        /// <summary> Indicates that the view shall be placed left of document position. </summary>
        public static readonly EViewPosition Left = new EViewPosition("Left");

        /// <summary> Indicates that the view shall be placed top of document position. </summary>
        public static readonly EViewPosition Top = new EViewPosition("Top");

        /// <summary> Indicates that the view shall be placed right of document position. </summary>
        public static readonly EViewPosition Right = new EViewPosition("Right");

        /// <summary> Indicates that the view shall be placed bottom of document position. </summary>
        public static readonly EViewPosition Bottom = new EViewPosition("Bottom");

        /// <summary> Indicates that the view shall be placed collapsed left of the document position. </summary>
        public static readonly EViewPosition LeftCollapsed = new EViewPosition("LeftCollapsed");

        /// <summary> Indicates that the view shall be placed collapsed right of the document position. </summary>
        public static readonly EViewPosition RightCollapsed = new EViewPosition("RightCollapsed");

        /// <summary> Indicates that the view shall be placed collapsed bottom of the document position. </summary>
        public static readonly EViewPosition BottomCollapsed = new EViewPosition("BottomCollapsed");

        /// <summary> Indicates that the view shall be placed collapsed top of the document position. </summary>
        public static readonly EViewPosition TopCollapsed = new EViewPosition("TopCollapsed");

        /// <summary> Protected constructor. </summary>
        /// <param name="uniqueReference">Unique reference to identify this instance</param>
        private EViewPosition(object uniqueReference) : base(uniqueReference) {
            // Currently nothing to do here
        }
    }
}