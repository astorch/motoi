using System;

namespace motoi.moose {
    /// <summary>
    /// Exception that indicates that a plug-in is not known by the service.
    /// </summary>
    public class UnknownPluginException : Exception {
        /// <summary>
        /// Creates a new instance using the given parameters.
        /// </summary>
        /// <param name="pluginName">Name of the unknown plug-in</param>
        public UnknownPluginException(string pluginName) 
            : base(string.Format("There is no plug-in with the given name '{0}'", pluginName)) { }

//        public UnknownPluginException(string pluginName, string message)
//            : base(string.Format("There is no plug-in with the given name '{0}'. {1}", pluginName, message)) { }
//
//        public UnknownPluginException(string pluginName, Exception innerException)
//            : base(string.Format("There is no plug-in with the given name '{0}'", pluginName), innerException) { }
    }
}