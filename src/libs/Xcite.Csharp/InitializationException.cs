using System;

namespace Xcite.Csharp {
    /// <summary>
    /// Defines an exception that is thrown when an error during the initialization of 
    /// an object occurred.
    /// </summary>
    public class InitializationException : Exception {
        /// <inheritdoc />
        public InitializationException(string message, Exception innerException) : base(message, innerException) {
            // Nothing to do here
        }
    }
}