using System.Collections.Generic;
using System.Collections.Specialized;

namespace Xcite.Collections {
    /// <summary>
    /// Represents a generic indexed collection of key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">Type of keys managed by this collection</typeparam>
    /// <typeparam name="TValue">Type of values managed by this collection</typeparam>
    public interface IOrderedDictionary<TKey, TValue> : IOrderedDictionary, IDictionary<TKey, TValue> {

        /// <summary>
        /// Returns the element with the specified key or does set it.
        /// </summary>
        /// <param name="key">Key of the element to get or set.</param>
        /// <returns>Element with the specified key</returns>
        /// <exception cref="KeyNotFoundException">If there is no element for the specified key</exception>
        new TValue this[TKey key] { get; set; }

        /// <summary>
        /// Returns the element with the specified index or does set it.
        /// </summary>
        /// <param name="index">Index of the element to get or set.</param>
        /// <returns>Element with the specified index</returns>
        /// <exception cref="KeyNotFoundException">If there is no element for the specified key</exception>
        new TValue this[int index] { get; set; }

        /// <summary>
        /// Inserts the element with the given key at the given index.
        /// </summary>
        /// <param name="index">Index the element is inserted to</param>
        /// <param name="key">Key of the element to insert</param>
        /// <param name="value">Value of the element to insert</param>
        void Insert(int index, TKey key, TValue value);

        /// <summary>
        /// Inserts the element with the given key at the first position. All 
        /// remaining elements are moved.
        /// </summary>
        /// <param name="key">Key of the element to insert</param>
        /// <param name="value">Value of the element to insert</param>
        void InsertFirst(TKey key, TValue value);

        /// <summary>
        /// Inserts the element with the given key at the last position.
        /// </summary>
        /// <param name="key">Key of the element to insert</param>
        /// <param name="value">Value of the element to insert</param>
        void InsertLast(TKey key, TValue value);

        /// <summary>
        /// Removes the element at the first position.
        /// </summary>
        void RemoveFirst();

        /// <summary>
        /// Removes the element at the last position.
        /// </summary>
        void RemoveLast();

        /// <summary>
        /// Returns the index of the element with the specified key. If no element 
        /// can be found, -1 is returned.
        /// </summary>
        /// <param name="key">Key of the element to look up</param>
        /// <returns>Index of the element with the specified key or -1</returns>
        int IndexOf(TKey key);
    }
}