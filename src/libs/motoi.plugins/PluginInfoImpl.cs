namespace motoi.plugins {
    /// <summary> Provides an implementation of <see cref="IPluginInfo"/>. </summary>
    class PluginInfoImpl : IPluginInfo {
        /// <summary> Returns the state of the plug-in. </summary>
        public EPluginState State { get; set; }

        /// <summary> Returns the signature of the plug-in. </summary>
        public IPluginSignature Signature { get; set; }

        /// <summary> Returns the bundle for the plug-in. </summary>
        public IBundle Bundle { get; set; }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Signature} - {State} [{Bundle}]";
        }
    }
}