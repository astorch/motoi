namespace motoi.plugins.model {
    /// <summary>
    /// Defines the methods of a plug-in activator.
    /// </summary>
    public interface IPluginActivator {
        /// <summary>
        /// Will be invoked when the plug-in has been activated.
        /// </summary>
        void OnActivate();

        /// <summary>
        /// Will be invoked when the plug-in has been disposed.
        /// </summary>
        void OnDispose();
    }
}