using Xcite.Csharp.lang;

namespace motoi.platform.resources.model.preference {
    /// <summary>
    /// Defines storage scopes of a preference store.
    /// </summary>
    public class EStoreScope : XEnum<EStoreScope> {
        /// <summary>
        /// Indicates that the settings are stored user specific. So, each user 
        /// has its own settings even the share the same installation.
        /// </summary>
        public static readonly EStoreScope User = new EStoreScope("User");

        /// <summary>
        /// Indicates that the settings are stored application specific. So, all user 
        /// have access to the same settings.
        /// </summary>
        public static readonly EStoreScope Application = new EStoreScope("Application");

        /// <inheritdoc />
        private EStoreScope(object uniqueReference) : base(uniqueReference) {
            // Nothing to do here
        }
    }
}