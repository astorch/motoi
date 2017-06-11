using System.Collections.Generic;
using xcite.collections;

namespace motoi.extensions.core
{
    /// <summary>
    /// Provides an implementation of <see cref="IConfigurationElement"/>.
    /// </summary>
    class ConfigurationElementImpl : IConfigurationElement {
        /// <summary>
        /// Backing variable for the id value map.
        /// </summary>
        private readonly IDictionary<string, string> iAttributeMap;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="id">Id of the extension element</param>
        /// <param name="prefix">Prefix of the extension element definition</param>
        /// <param name="attributes">Attributes map</param>
        public ConfigurationElementImpl(string id, string prefix, IDictionary<string,string> attributes) {
            Id = id;
            Prefix = prefix;
            iAttributeMap = attributes;
        }

        /// <summary>
        /// Returns the id of the associated extension point.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Returns the prefix of the node definition.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Returns the attribute value addressed by the id.
        /// Returns the same as <see cref="IConfigurationElement.GetAttributeValue"/>.
        /// </summary>
        /// <param name="id">Id of the attribute</param>
        /// <returns>Attribute value or null</returns>
        public string this[string id] {
            get { return GetAttributeValue(id); }
        }

        /// <summary>
        /// Returns the attribute value addressed by the id.
        /// </summary>
        /// <param name="id">Id of the attribute</param>
        /// <returns>Attribute value or null</returns>
        public string GetAttributeValue(string id) {
            string value;
            if (iAttributeMap.TryGetValue(id, out value))
                return value;
            return null;
        }

        /// <inheritdoc />
        public override string ToString() {
            string result = string.Format("{0} id='{1}' attributes='{2}'",
                Prefix, Id, iAttributeMap.ToFormattedString());
            return result;
        }
    }
}