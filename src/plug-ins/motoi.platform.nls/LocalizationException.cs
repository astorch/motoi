using System;

namespace motoi.platform.nls {
    /// <summary>
    /// Defines an error that occurred during the initialization
    /// of a plug-in specific localization.
    /// </summary>
    public class LocalizationException : Exception {
        /// <inheritdoc />
        public LocalizationException(string message) : base(message) {
            // Nothing to do here
        }

        /// <inheritdoc />
        public LocalizationException(string message, Exception innerException) : base(message, innerException) {
            // Nothing to do here
        }
    }
}