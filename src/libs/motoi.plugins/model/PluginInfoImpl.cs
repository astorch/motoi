using motoi.plugins.enums;

namespace motoi.plugins.model {
    /// <summary>
    /// Provides an implementation of <see cref="IPluginInfo"/>.
    /// </summary>
    class PluginInfoImpl : IPluginInfo {
        /// <summary>
        /// Returns the state of the plug-in.
        /// </summary>
        public EPluginState State { get; set; }

        /// <summary>
        /// Returns the signature of the plug-in.
        /// </summary>
        public IPluginSignature Signature { get; set; }

        /// <summary>
        /// Returns the bundle for the plug-in.
        /// </summary>
        public IBundle Bundle { get; set; }

        /// <inheritdoc />
        public override string ToString() {
            return string.Format("{0} - {1} [{2}]", Signature, State, Bundle);
        }
    }
}