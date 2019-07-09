using System.Collections.Generic;
using System.Linq;

namespace motoi.extensions {
    /// <summary> Provides an implementation of <see cref="IConfigurationElement"/>. </summary>
    class ConfigurationElementImpl : IConfigurationElement {
        /// <summary> Backing variable for the id value map. </summary>
        private readonly IDictionary<string, string> _attributeMap;

        /// <summary> Creates a new instance. </summary>
        /// <param name="id">Id of the extension element</param>
        /// <param name="prefix">Prefix of the extension element definition</param>
        /// <param name="attributes">Attributes map</param>
        public ConfigurationElementImpl(string id, string prefix, IDictionary<string, string> attributes) {
            Id = id;
            Prefix = prefix;
            _attributeMap = attributes;
        }

        /// <summary> Returns the id of the associated extension point. </summary>
        public string Id { get; }

        /// <summary> Returns the prefix of the node definition. </summary>
        public string Prefix { get; }

        /// <summary>
        /// Returns the attribute value addressed by the id.
        /// Returns the same as <see cref="IConfigurationElement.GetAttributeValue"/>.
        /// </summary>
        /// <param name="id">Id of the attribute</param>
        /// <returns>Attribute value or null</returns>
        public string this[string id] 
            => GetAttributeValue(id);

        /// <summary> Returns the attribute value addressed by the id. </summary>
        /// <param name="id">Id of the attribute</param>
        /// <returns>Attribute value or null</returns>
        public string GetAttributeValue(string id) {
            if (_attributeMap.TryGetValue(id, out string value)) return value;
            return null;
        }

        /// <inheritdoc />
        public override string ToString() {
            string attributesLine = string.Join(", ", _attributeMap.Select(entry => $"{entry.Key}={entry.Value}"));
            return $"{Prefix} id='{Id}' attributes='{attributesLine}'";
        }
    }
}