using System;
using System.Collections;
using System.Collections.Generic;

namespace motoi.platform.application {
    /// <summary>
    /// Provides access to global platform settings.
    /// </summary>
    public class PlatformSettings : IEnumerable<KeyValuePair<string,string>>, IDisposable {
        /// <summary>
        /// Backing variable of the instance.
        /// </summary>
        private static PlatformSettings iInstance;

        /// <summary>
        /// Returns the instance.
        /// </summary>
        public static PlatformSettings Instance {
            get { return iInstance ?? (iInstance = new PlatformSettings()); }
        }

        /// <summary>
        /// Backing variable for the settings map.
        /// </summary>
        private readonly IDictionary<string,string> iSettingsMap = new Dictionary<string, string>(10);

        /// <summary>
        /// Private constructor.
        /// </summary>
        private PlatformSettings() {
            // Prevent from external invocation
        }

        /// <summary>
        /// Adds the given value for the given key to the settings.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        internal void Add(string key, string value) {
            iSettingsMap.Add(key, value);
        }

        /// <summary>
        /// Returns the value of the given key or null if none exists.
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <returns>Value or null</returns>
        public string Get(string key) {
            string value;
            if (iSettingsMap.TryGetValue(key, out value))
                return value;
            return null;
        }

        /// <summary>
        /// Returns the value of the given key or null if none exists.
        /// Returns the same as <see cref="Get"/>
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <returns>Value or null</returns>
        public string this[string key] { get { return Get(key); } }

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
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return iSettingsMap.GetEnumerator();
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() {
            iSettingsMap.Clear();
            iInstance = null;
        }
    }
}