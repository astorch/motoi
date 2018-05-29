using System;

namespace Tessa {
    /// <summary> Announces an error in the tessa execution. </summary>
    public class TessaException : Exception {
        /// <inheritdoc />
        public TessaException(string message) : base(message) {
            // Accomplish base initializer
        }

        /// <inheritdoc />
        public TessaException(string message, Exception innerException) : base(message, innerException) {
            // Accomplish base initializer
        }
    }
}