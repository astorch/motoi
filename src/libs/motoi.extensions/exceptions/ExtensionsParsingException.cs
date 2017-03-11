using System;

namespace motoi.extensions.exceptions
{
    /// <summary>
    /// Defines an exception that is thrown when the parsing of an extension.xml file went wrong.
    /// </summary>
    public class ExtensionsParsingException : Exception {

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="ex">Inner exception</param>
        public ExtensionsParsingException(string message = null, Exception ex = null) : base(message, ex) { }
    }
}