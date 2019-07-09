using System;
using System.ComponentModel;

namespace motoi.platform.resources.model.preference {
    /// <summary> Provides methods to store and read information as key value pair. </summary>
    public interface IPreferenceStore : INotifyPropertyChanged {
        /// <summary>
        /// Returns the value of the given <paramref name="key"/>. If there is no value, 
        /// an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <param name="key">Key to look up</param>
        /// <returns>Value of the key</returns>
        /// <exception cref="InvalidOperationException">If the key is not present or unset</exception>
        TValue GetValue<TValue>(string key) where TValue : IConvertible;

        /// <summary>
        /// Returns TRUE if the value of the given <paramref name="key"/> exist. The resolved 
        /// value is returned by the out parameter <paramref name="value"/>. This method doesn't throw 
        /// an exception.
        /// </summary>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <param name="key">Key to look up</param>
        /// <param name="value">Resolved value or default (TValue) if the key is not present or unset</param>
        /// <returns>TRUE if a value for the key has been found</returns>
        bool TryGetValue<TValue>(string key, out TValue value) where TValue : IConvertible;

        /// <summary>
        /// Returns the value of the given <paramref name="key"/>. If there is no value, 
        /// the given <paramref name="defaultValue"/> is returned. This method doesn't throw 
        /// an exception.
        /// </summary>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <param name="key">Key to look up</param>
        /// <param name="defaultValue">Returning default value if the key is not present or unset</param>
        /// <returns>Value for the key</returns>
        TValue GetValue<TValue>(string key, TValue defaultValue) where TValue : IConvertible;

        /// <summary> Sets the value of the given <paramref name="key"/>. </summary>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <param name="key">Key the value is assigned</param>
        /// <param name="value">Value to set</param>
        void SetValue<TValue>(string key, TValue value) where TValue : IConvertible;

        /// <summary>
        /// Returns TRUE if the store contains a value
        /// with the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>TRUE or FALSE</returns>
        bool HasValue(string key);

        /// <summary> Notifies the instance to store its data. </summary>
        void Flush();
    }
}