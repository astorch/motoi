using System;

namespace motoi.platform.ui.bindings.exceptions {
    /// <summary>
    /// Describes an exception that occurs during binding operations.
    /// </summary>
    public class DataBindingException : Exception {
        /// <summary>
        /// Creates a new instance with the given message.
        /// </summary>
        /// <param name="message">Exception message</param>
        public DataBindingException(string message) : base(message) {
            // Nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given message and inner exception.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DataBindingException(string message, Exception innerException) : base(message, innerException) {
            // Nothing to do here, too
        }
    }
}