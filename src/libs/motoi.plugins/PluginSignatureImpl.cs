namespace motoi.plugins {
    /// <summary>
    /// Provides an implementation of <see cref="IPluginSignature"/>.
    /// </summary>
    class PluginSignatureImpl : IPluginSignature {
        /// <summary>
        /// Returns the name of the plug-in.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the symbolic name of the plug-in.
        /// </summary>
        public string SymbolicName { get; set; }

        /// <summary>
        /// Returns the version of the plug-in.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Returns the vendor of the plug-in.
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Returns the type of the plug-in activator class.
        /// </summary>
        public string PluginActivatorType { get; set; }

        /// <summary>
        /// Returns an instance of <see cref="IPluginActivator"/> that is responsible for the associated plug-in.
        /// </summary>
        public IPluginActivator PluginActivator { get; set; }

        /// <summary>
        /// Returns the names of the dependend plug-ins.
        /// </summary>
        public string[] Dependencies { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0}_{1}", SymbolicName, Version);
        }
    }
}