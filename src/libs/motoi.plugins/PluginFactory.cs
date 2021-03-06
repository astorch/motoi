﻿namespace motoi.plugins {
    /// <summary> Provides a factory for creating instances of <see cref="IPluginInfo"/>. </summary>
    class PluginFactory : AbstractFactory<PluginFactory> {
        /// <summary> Creates a new instance for the given signature. </summary>
        /// <param name="bundle">Bundle which provides the plug-in</param>
        /// <param name="signature">Plug-in signature</param>
        /// <returns>New instance of <see cref="IPluginInfo"/></returns>
        public IPluginInfo CreatePluginInfo(IBundle bundle, IPluginSignature signature) {
            return new PluginInfoImpl {
                State = EPluginState.Found, Signature = signature, Bundle = bundle
            };
        }
    }
}