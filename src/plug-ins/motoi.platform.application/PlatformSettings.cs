using System;
using System.Collections;
using System.Collections.Generic;

namespace motoi.platform.application {
    /// <summary> Provides access to global platform settings. </summary>
    public class PlatformSettings : IEnumerable<KeyValuePair<string,string>>, IDisposable {
        private static PlatformSettings _instance;

        /// <summary> Returns the instance. </summary>
        public static PlatformSettings Instance 
            => _instance ?? (_instance = new PlatformSettings());
        
        private readonly IDictionary<string,string> _settingsMap = new Dictionary<string, string>(10);

        /// <inheritdoc />
        private PlatformSettings() {
            // Prevent from external invocation
        }

        /// <summary> Adds the given value for the given key to the settings. </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        internal void Add(string key, string value) {
            _settingsMap.Add(key, value);
        }

        /// <summary> Returns the value of the given key or null if none exists. </summary>
        /// <param name="key">Key of the value</param>
        /// <returns>Value or null</returns>
        public string Get(string key) {
            if (_settingsMap.TryGetValue(key, out string value)) return value;
            return null;
        }

        /// <summary>
        /// Returns the value of the given key or null if none exists.
        /// Returns the same as <see cref="Get"/>
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <returns>Value or null</returns>
        public string this[string key] 
            => Get(key);
        
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        
        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return _settingsMap.GetEnumerator();
        }

        /// <inheritdoc />
        public void Dispose() {
            _settingsMap.Clear();
            _instance = null;
        }
    }
}