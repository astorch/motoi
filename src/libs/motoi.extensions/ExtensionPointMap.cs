using System.Collections;
using System.Collections.Generic;
using xcite.collections;

namespace motoi.extensions
{
    /// <summary>
    /// Implements a map for handling multiple configuration elements for one id.
    /// </summary>
    class ExtensionPointMap : IEnumerable<KeyValuePair<string, IList<IConfigurationElement>>> {

        /// <summary>
        /// Backing variable for the map.
        /// </summary>
        private readonly IDictionary<string, IList<IConfigurationElement>> iIdToCollectionMap = new Dictionary<string, IList<IConfigurationElement>>(100); 

        /// <summary>
        /// Adds the given configuration element for the given id.
        /// </summary>
        /// <param name="id">Id of the configuration element</param>
        /// <param name="configurationElement">Configuration element to add</param>
        public void Add(string id, IConfigurationElement configurationElement) {
            IList<IConfigurationElement> elements;
            if (!iIdToCollectionMap.TryGetValue(id, out elements))
                iIdToCollectionMap[id] = (elements = new List<IConfigurationElement>(10));

            elements.Add(configurationElement);
        }

        /// <summary>
        /// Adds the given configuration elements for the given id.
        /// </summary>
        /// <param name="id">Id of the configuration element</param>
        /// <param name="configurationElements">Configuration elements to add</param>
        public void AddAll(string id, IList<IConfigurationElement> configurationElements) {
            IList<IConfigurationElement> elements;
            if (!iIdToCollectionMap.TryGetValue(id, out elements))
                iIdToCollectionMap[id] = configurationElements;
            else
                configurationElements.ForEach(elements.Add);
        }

        /// <summary>
        /// Adds all items of the given map to this one.
        /// </summary>
        /// <param name="map">Map which contains all items that will be added to this one</param>
        public void Merge(ExtensionPointMap map) {
            for (IEnumerator<string> enmtor = map.iIdToCollectionMap.Keys.GetEnumerator(); enmtor.MoveNext();) {
                string id = enmtor.Current;
                IList<IConfigurationElement> elements = map.iIdToCollectionMap[id];
                AddAll(id, elements);
            }
        }

        /// <summary>
        /// Returns all configuration elements that are associated with the given id.
        /// </summary>
        /// <param name="id">Id of the configuration elements</param>
        /// <returns>Array of configuration elements</returns>
        public IConfigurationElement[] GetConfigurationElements(string id) {
            IList<IConfigurationElement> elements;
            if (!iIdToCollectionMap.TryGetValue(id, out elements))
                return new IConfigurationElement[0];

            return elements.ToArray();
        }

        /// <summary>
        /// Returns all configuration elements that are associated with the given id.
        /// Returns the same as <see cref="GetConfigurationElements"/>.
        /// </summary>
        /// <param name="id">Id of the configuration elements</param>
        /// <returns>Array of the configuration elements</returns>
        public IConfigurationElement[] this[string id] { get { return GetConfigurationElements(id); } }

        /// <summary>
        /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<string, IList<IConfigurationElement>>> GetEnumerator() {
            return iIdToCollectionMap.GetEnumerator();
        }

        /// <summary>
        /// Returns the count of items.
        /// </summary>
        public int Count { get { return iIdToCollectionMap.Count; } }

        /// <summary>
        /// Clears the map and removes all entries.
        /// </summary>
        public void Clear() {
            iIdToCollectionMap.Clear();
        }

        public override string ToString() {
            string result = string.Format("ExtensionPointMap Items: {0}", Count);
            return result;
        }
    }
}