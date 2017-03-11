using System;

namespace Tessa.Exceptions {
    /// <summary>
    /// Defines an exception that is thrown by the packager when no 'signatur.mf' 
    /// file could be found.
    /// </summary>
    public class NoSignaturException : Exception {
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="ex">Preceding exception</param>
        public NoSignaturException(string message = null, Exception ex = null) : base(message, ex) {
            // Currently nothing to do here
        }
    }
}