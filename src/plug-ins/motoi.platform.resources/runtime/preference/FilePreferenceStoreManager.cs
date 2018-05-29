using System;
using System.Collections.Generic;
using System.IO;
using motoi.platform.resources.model.preference;
using NLog;

namespace motoi.platform.resources.runtime.preference {
    /// <summary>
    /// Provides an implementation of <see cref="IPreferenceStoreManager"/> that operates on 
    /// the local file system.
    /// </summary>
    public class FilePreferenceStoreManager : IPreferenceStoreManager {
        private const string PreferencesFolderName = ".preferences";
        private readonly Dictionary<string, IPreferenceStore> iPreferenceStoreTable = new Dictionary<string, IPreferenceStore>(23);
        private readonly Logger _log = LogManager.GetCurrentClassLogger(typeof(PreferenceStoreManager));
        private string iPreferenceFolderBasePath;

        /// <inheritdoc />
        public IPreferenceStore GetStore(string storeName, EStoreScope storeScope) {
            if (string.IsNullOrEmpty(storeName)) return null;

            string storeBasePath = iPreferenceFolderBasePath;
            if (storeScope == EStoreScope.User)
                storeBasePath = Path.Combine(storeBasePath, Environment.UserName.ToLowerInvariant());

            if (!iPreferenceStoreTable.TryGetValue(storeName, out IPreferenceStore prefStore)) {
                // Create the store base path if neccessary
                if (!Directory.Exists(storeBasePath))
                    Directory.CreateDirectory(storeBasePath);
                
                // Finalize the store path
                string storePath = Path.Combine(storeBasePath, storeName + ".prefs");

                // Create the store instance
                PreferenceStore preferenceStore = new PreferenceStore(storeName, storePath);
                preferenceStore.Load();
                iPreferenceStoreTable.Add(storeName, prefStore = preferenceStore);
            }

            return prefStore;
        }

        /// <inheritdoc />
        public void Initialize() {
            string metaDataFolder = ResourceService.Instance.MetaDataFolder;
            string preferencesFolder = Path.Combine(metaDataFolder, PreferencesFolderName);
            if (!Directory.Exists(preferencesFolder))
                Directory.CreateDirectory(preferencesFolder);

            iPreferenceFolderBasePath = preferencesFolder;
        }

        /// <inheritdoc />
        public void Dispose() {
            using (Dictionary<string, IPreferenceStore>.ValueCollection.Enumerator storeItr = iPreferenceStoreTable.Values.GetEnumerator()) {
                while (storeItr.MoveNext()) {
                    IPreferenceStore store = storeItr.Current;
                    if (store == null) continue;

                    try {
                        store.Flush();
                    } catch (Exception ex) {
                        _log.Error(ex, "Error on flushing store. Reason: {0}");
                    }
                }
            }

            iPreferenceStoreTable.Clear();
        }
    }
}