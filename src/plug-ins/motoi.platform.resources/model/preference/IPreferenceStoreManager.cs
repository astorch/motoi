using System;
using xcite.csharp;

namespace motoi.platform.resources.model.preference {
    /// <summary> Provides access to the preference stores. </summary>
    public interface IPreferenceStoreManager : IDisposable, IInitializable {
        /// <summary>
        /// Returns an instance of <see cref="IPreferenceStore"/> for
        /// the given <paramref name="storeName"/> and <paramref name="storeScope"/>. 
        /// Note, even if there hasn't been a store before, a new one is created.
        /// So this method always returns an operable instance.
        /// The location the store saves its settings is parametrized
        /// using the <paramref name="storeScope"/>. 
        /// </summary>
        /// <param name="storeName">Name of the store to get or create</param>
        /// <param name="storeScope">Scope the store loads its data or writes to</param>
        /// <returns>Instance of <see cref="IPreferenceStore"/></returns>
        IPreferenceStore GetStore(string storeName, EStoreScope storeScope);
    }
}