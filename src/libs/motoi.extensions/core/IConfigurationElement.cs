namespace motoi.extensions.core {
    /// <summary>
    /// Defines the a Configuration Element.
    /// </summary>
    public interface IConfigurationElement {
        /// <summary>
        /// Returns the id of the associated extension point.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the prefix of the node definition.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Returns the attribute value addressed by the id.
        /// Returns the same as <see cref="GetAttributeValue"/>.
        /// </summary>
        /// <param name="id">Id of the attribute</param>
        /// <returns>Attribute value or null</returns>
        string this[string id] { get; }

        /// <summary>
        /// Returns the attribute value addressed by the id.
        /// </summary>
        /// <param name="id">Id of the attribute</param>
        /// <returns>Attribute value or null</returns>
        string GetAttributeValue(string id);
    }
}