namespace motoi.plugins.enums {
    /// <summary>
    /// Defines lifecycle status of plug-ins.
    /// </summary>
    public enum EPluginState {
        /// <summary>
        /// The plug-in has been found within the plug-in directory.
        /// </summary>
        Found,
        /// <summary>
        /// All dependencies of the plug-in could be resolved. It can be used.
        /// </summary>
        Provided,
        /// <summary>
        /// The plug-in has been activated.
        /// </summary>
        Activated,
        /// <summary>
        /// The plug-in has been disposed.
        /// </summary>
        Disposed
    }
}