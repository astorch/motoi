using xcite.csharp;

namespace motoi.platform.resources.model {
    /// <summary> Defines kinds of refresh behaviors. </summary>
    public class ERefreshBehavior : XEnum<ERefreshBehavior> {
        /// <summary>
        /// Indicates that the container itself,
        /// its children and each child of a children should be refreshed.
        /// </summary>
        public static readonly ERefreshBehavior Infinit = new ERefreshBehavior("Infinit");

        /// <summary>
        /// Indicates that the container itself and
        /// its children should be refreshed.
        /// </summary>
        public static readonly ERefreshBehavior Self = new ERefreshBehavior("Self");

        /// <summary>
        /// Indicates that only the children of a container
        /// should be refreshed. Children of children 
        /// will not be refreshed.
        /// </summary>
        public static readonly ERefreshBehavior Children = new ERefreshBehavior("Children");
        
        /// <summary> Protected constructor. </summary>
        /// <param name="uniqueReference">Unique reference to identify this instance</param>
        private ERefreshBehavior(object uniqueReference) : base(uniqueReference) {
            // Currently nothing to do here
        }
    }
}