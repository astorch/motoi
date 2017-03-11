namespace motoi.plugins.model {
    /// <summary>
    /// Defines the signature of a plug-in.
    /// </summary>
    public interface IPluginSignature {
        /// <summary>
        /// Returns the name of the plug-in.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the symbolic name of the plug-in.
        /// </summary>
        string SymbolicName { get; }

        /// <summary>
        /// Returns the version of the plug-in.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Returns the vendor of the plug-in.
        /// </summary>
        string Vendor { get; }

        /// <summary>
        /// Returns the type of the plug-in activator class.
        /// </summary>
        string PluginActivatorType { get; }

        /// <summary>
        /// Returns the names of the dependend plug-ins.
        /// </summary>
        string[] Dependencies { get; }
    }
}