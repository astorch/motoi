using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using motoi.platform.resources.model.preference;

namespace motoi.platform.resources.runtime.preference {
    /// <summary>
    /// Provides an implementation of <see cref="IPreferenceStore"/>.
    /// </summary>
    public class PreferenceStore : IPreferenceStore {

        private static readonly IFormatProvider iFormatProvider = CultureInfo.InvariantCulture;

        private readonly string iStoreName;
        private readonly string iStorePath;
        private readonly Dictionary<string, string> iValueTable = new Dictionary<string, string>(23);

        /// <summary>
        /// Creates a new instance that is associated with the given <paramref name="storeName"/>. 
        /// The data is loaded from and written to the given <paramref name="storePath"/>.
        /// </summary>
        /// <param name="storeName">Name of the store</param>
        /// <param name="storePath">Path the store reads its data from or stores it</param>
        /// <exception cref="ArgumentNullException">If any argument is NULL or empty</exception>
        public PreferenceStore(string storeName, string storePath) {
            if (string.IsNullOrEmpty(storeName)) throw new ArgumentNullException("storeName");
            if (string.IsNullOrEmpty(storePath)) throw new ArgumentNullException("storePath");

            iStoreName = storeName;
            iStorePath = storePath;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public virtual TValue GetValue<TValue>(string key) where TValue : IConvertible {
            TValue value;
            if (!TryGetValue(key, out value)) throw new InvalidOperationException("Key is not present or unset");
            return value;
        }

        /// <inheritdoc />
        public virtual bool TryGetValue<TValue>(string key, out TValue value) where TValue : IConvertible {
            value = default(TValue);
            if (!HasValue(key)) return false;

            string strValue = iValueTable[key];
            value = (TValue) Convert.ChangeType(strValue, typeof(TValue), iFormatProvider);
            return true;
        }

        /// <inheritdoc />
        public virtual TValue GetValue<TValue>(string key, TValue defaultValue) where TValue : IConvertible {
            TValue tableValue;
            if (!TryGetValue(key, out tableValue)) return defaultValue;
            return tableValue;
        }

        /// <inheritdoc />
        public virtual void SetValue<TValue>(string key, TValue value) where TValue : IConvertible {
            if (string.IsNullOrEmpty(key)) return;

            if (value == null) {
                iValueTable.Remove(key);
            } else {
                iValueTable[key] = value.ToString(iFormatProvider);
                RaisePropertyChanged(key);
            }
        }

        /// <inheritdoc />
        public virtual bool HasValue(string key) {
            if (string.IsNullOrEmpty(key)) return false;
            return iValueTable.ContainsKey(key);
        }

        /// <inheritdoc />
        public virtual void Flush() {
            List<string> fileEntries = new List<string>(iValueTable.Count);
            using (Dictionary<string, string>.Enumerator itr = iValueTable.GetEnumerator()) {
                while (itr.MoveNext()) {
                    KeyValuePair<string, string> keyValuePair = itr.Current;
                    string entry = string.Format("{0}={1}", keyValuePair.Key, keyValuePair.Value);
                    fileEntries.Add(entry);
                }
            }

            File.WriteAllLines(iStorePath, fileEntries);
        }

        /// <summary>
        /// Fills the data of the instance with the information from the assigned store path.
        /// </summary>
        public void Load() {
            if (!File.Exists(iStorePath)) return;

            string[] fileEntries = File.ReadAllLines(iStorePath);
            for (int i = -1; ++i != fileEntries.Length;) {
                string entry = fileEntries[i];
                string[] keyValuePair = entry.Split(new[] {'='}, 2);
                string key = keyValuePair[0];
                string value = keyValuePair[1];

                iValueTable[key] = value;
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
            return string.Format("{0} '{1}'", GetType().Name, iStoreName);
        }
    }
}