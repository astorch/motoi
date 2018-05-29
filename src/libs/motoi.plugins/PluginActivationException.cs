using System;

namespace motoi.plugins {
    /// <summary>
    /// Defines an exception that indicates an error during the activation process of a plug-in.
    /// </summary>
    public class PluginActivationException : Exception {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public PluginActivationException(string message) : base(message) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/> and predecessing <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Predecessing exception</param>
        public PluginActivationException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }
    }
}