namespace motoi.plugins {
    /// <summary> Defines informations about a plug-in. </summary>
    public interface IPluginInfo {
        /// <summary> Returns the state of the plug-in. </summary>
        EPluginState State { get; }

        /// <summary> Returns the signature of the plug-in. </summary>
        IPluginSignature Signature { get; }

        /// <summary> Returns the bundle for the plug-in. </summary>
        IBundle Bundle { get; }
    }
}