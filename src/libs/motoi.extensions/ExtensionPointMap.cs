using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace motoi.extensions {
    /// <summary> Implements a map for handling multiple configuration elements for one id. </summary>
    class ExtensionPointMap : IEnumerable<KeyValuePair<string, IList<IConfigurationElement>>> {
        /// <summary> Backing variable for the map. </summary>
        private readonly IDictionary<string, IList<IConfigurationElement>> _idToCollectionMap = new Dictionary<string, IList<IConfigurationElement>>(100);

        /// <summary> Adds the given configuration element for the given id. </summary>
        /// <param name="id">Id of the configuration element</param>
        /// <param name="configurationElement">Configuration element to add</param>
        public void Add(string id, IConfigurationElement configurationElement) {
            if (!_idToCollectionMap.TryGetValue(id, out IList<IConfigurationElement> elements))
                _idToCollectionMap[id] = (elements = new List<IConfigurationElement>(10));

            elements.Add(configurationElement);
        }

        /// <summary> Adds the given configuration elements for the given id. </summary>
        /// <param name="id">Id of the configuration element</param>
        /// <param name="configurationElements">Configuration elements to add</param>
        public void AddAll(string id, IList<IConfigurationElement> configurationElements) {
            if (!_idToCollectionMap.TryGetValue(id, out IList<IConfigurationElement> elements)) {
                _idToCollectionMap[id] = configurationElements;
            } else {
                for (int i = -1; ++i != configurationElements.Count;) {
                    IConfigurationElement cfgEl = configurationElements[i];
                    elements.Add(cfgEl);
                }
            }
        }

        /// <summary> Adds all items of the given map to this one. </summary>
        /// <param name="map">Map which contains all items that will be added to this one</param>
        public void Merge(ExtensionPointMap map) {
            using (IEnumerator<string> itr = map._idToCollectionMap.Keys.GetEnumerator()) {
                while (itr.MoveNext()) {
                    string id = itr.Current;
                    if (id == null) continue;
                    
                    IList<IConfigurationElement> elements = map._idToCollectionMap[id];
                    AddAll(id, elements);
                }
            }
        }

        /// <summary> Returns all configuration elements that are associated with the given id. </summary>
        /// <param name="id">Id of the configuration elements</param>
        /// <returns>Array of configuration elements</returns>
        public IConfigurationElement[] GetConfigurationElements(string id) {
            if (!_idToCollectionMap.TryGetValue(id, out IList<IConfigurationElement> elements))
                return new IConfigurationElement[0];

            return elements.ToArray();
        }

        /// <summary>
        /// Returns all configuration elements that are associated with the given id.
        /// Returns the same as <see cref="GetConfigurationElements"/>.
        /// </summary>
        /// <param name="id">Id of the configuration elements</param>
        /// <returns>Array of the configuration elements</returns>
        public IConfigurationElement[] this[string id] 
            => GetConfigurationElements(id);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, IList<IConfigurationElement>>> GetEnumerator() {
            return _idToCollectionMap.GetEnumerator();
        }

        /// <summary> Returns the count of items. </summary>
        public int Count 
            => _idToCollectionMap.Count;

        /// <summary> Clears the map and removes all entries. </summary>
        public void Clear() {
            _idToCollectionMap.Clear();
        }

        /// <inheritdoc />
        public override string ToString() {
            string result = $"ExtensionPointMap Items: {Count.ToString()}";
            return result;
        }
    }
}