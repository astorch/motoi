using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using motoi.platform.resources.model.preference;

namespace motoi.platform.resources.runtime.preference {
    /// <summary> Provides an implementation of <see cref="IPreferenceStore"/>. </summary>
    public class PreferenceStore : IPreferenceStore {

        private static readonly IFormatProvider _formatProvider = CultureInfo.InvariantCulture;

        private readonly string _storeName;
        private readonly string _storePath;
        private readonly Dictionary<string, string> _valueTable = new Dictionary<string, string>(23);

        /// <summary>
        /// Creates a new instance that is associated with the given <paramref name="storeName"/>. 
        /// The data is loaded from and written to the given <paramref name="storePath"/>.
        /// </summary>
        /// <param name="storeName">Name of the store</param>
        /// <param name="storePath">Path the store reads its data from or stores it</param>
        /// <exception cref="ArgumentNullException">If any argument is NULL or empty</exception>
        public PreferenceStore(string storeName, string storePath) {
            if (string.IsNullOrEmpty(storeName)) throw new ArgumentNullException(nameof(storeName));
            if (string.IsNullOrEmpty(storePath)) throw new ArgumentNullException(nameof(storePath));

            _storeName = storeName;
            _storePath = storePath;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public virtual TValue GetValue<TValue>(string key) where TValue : IConvertible {
            if (!TryGetValue(key, out TValue value)) throw new InvalidOperationException("Key is not present or unset");
            return value;
        }

        /// <inheritdoc />
        public virtual bool TryGetValue<TValue>(string key, out TValue value) where TValue : IConvertible {
            value = default;
            if (!HasValue(key)) return false;

            string strValue = _valueTable[key];
            value = (TValue) Convert.ChangeType(strValue, typeof(TValue), _formatProvider);
            return true;
        }

        /// <inheritdoc />
        public virtual TValue GetValue<TValue>(string key, TValue defaultValue) where TValue : IConvertible {
            if (!TryGetValue(key, out TValue tableValue)) return defaultValue;
            return tableValue;
        }

        /// <inheritdoc />
        public virtual void SetValue<TValue>(string key, TValue value) where TValue : IConvertible {
            if (string.IsNullOrEmpty(key)) return;

            if (value == null) {
                _valueTable.Remove(key);
            } else {
                _valueTable[key] = value.ToString(_formatProvider);
                RaisePropertyChanged(key);
            }
        }

        /// <inheritdoc />
        public virtual bool HasValue(string key) {
            if (string.IsNullOrEmpty(key)) return false;
            return _valueTable.ContainsKey(key);
        }

        /// <inheritdoc />
        public virtual void Flush() {
            List<string> fileEntries = new List<string>(_valueTable.Count);
            using (Dictionary<string, string>.Enumerator itr = _valueTable.GetEnumerator()) {
                while (itr.MoveNext()) {
                    KeyValuePair<string, string> keyValuePair = itr.Current;
                    string entry = $"{keyValuePair.Key}={keyValuePair.Value}";
                    fileEntries.Add(entry);
                }
            }

            File.WriteAllLines(_storePath, fileEntries);
        }

        /// <summary>
        /// Fills the data of the instance with the information from the assigned store path.
        /// </summary>
        public void Load() {
            if (!File.Exists(_storePath)) return;

            string[] fileEntries = File.ReadAllLines(_storePath);
            for (int i = -1; ++i != fileEntries.Length;) {
                string entry = fileEntries[i];
                string[] keyValuePair = entry.Split(new[] {'='}, 2);
                string key = keyValuePair[0];
                string value = keyValuePair[1];

                _valueTable[key] = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Name of changed property</param>
        protected virtual void RaisePropertyChanged(string propertyName) {
            if (string.IsNullOrEmpty(propertyName)) return;
            PropertyChangedEventHandler propertyChangedEventHandler = PropertyChanged;
            if (propertyChangedEventHandler == null) return;
            propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{GetType().Name} '{_storeName}'";
        }
    }
}