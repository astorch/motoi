using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;

namespace Xcite.Collections {
    /// <summary>
    /// Represents a generic indexed collection of key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">Type of keys managed by this collection</typeparam>
    /// <typeparam name="TValue">Type of values managed by this collection</typeparam>
    [Serializable]
    public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>,
        ISerializable, IDeserializationCallback {

        private readonly OrderedDictionary fOrderedDictionary;

        /// <summary>
        /// Creates a new instance using a default capacity.
        /// </summary>
        public OrderedDictionary() : this(5) {
            // Nothing to do
        }

        /// <summary>
        /// Creates a new instance using the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity</param>
        public OrderedDictionary(int capacity) {
            if (capacity < 0) throw new ArgumentException("Capacity must not be negative");
            fOrderedDictionary = new OrderedDictionary(capacity);
        }

        /// <inheritdoc />
        bool IDictionary.Contains(object key) {
            return fOrderedDictionary.Contains(key);
        }

        /// <inheritdoc />
        void IDictionary.Add(object key, object value) {
            fOrderedDictionary.Add(key, value);
        }

        /// <inheritdoc />
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            fOrderedDictionary.Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        void ICollection<KeyValuePair<TKey, TValue>>.Clear() {
            fOrderedDictionary.Clear();
        }

        /// <inheritdoc />
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) {
            return fOrderedDictionary.Contains(item.Key);
        }

        /// <inheritdoc />
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
            return Remove(item.Key);
        }

        /// <inheritdoc />
        int ICollection<KeyValuePair<TKey, TValue>>.Count { get { return fOrderedDictionary.Count; } }

        /// <inheritdoc />
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly { get { return ((IDictionary) this).IsReadOnly; } }

        /// <inheritdoc />
        public void Clear() {
            fOrderedDictionary.Clear();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            for (IDictionaryEnumerator itr = fOrderedDictionary.GetEnumerator(); itr.MoveNext();) {
                yield return new KeyValuePair<TKey, TValue>((TKey) itr.Key, (TValue) itr.Value);
            }
        }

        /// <inheritdoc />
        IDictionaryEnumerator IOrderedDictionary.GetEnumerator() {
            return fOrderedDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        void IOrderedDictionary.Insert(int index, object key, object value) {
            fOrderedDictionary.Insert(index, key, value);
        }

        /// <inheritdoc />
        public void RemoveAt(int index) {
            fOrderedDictionary.RemoveAt(index);
        }

        /// <inheritdoc />
        public TValue this[int index] {
            get { return (TValue) fOrderedDictionary[index]; }
            set { fOrderedDictionary[index] = value; }
        }

        /// <inheritdoc />
        public void Insert(int index, TKey key, TValue value) {
            fOrderedDictionary.Insert(index, key, value);
        }

        /// <inheritdoc />
        public void InsertLast(TKey key, TValue value) {
            int count = Count;
            Insert(count, key, value);
        }

        /// <inheritdoc />
        public void InsertFirst(TKey key, TValue value) {
            Insert(0, key, value);
        }

        /// <inheritdoc />
        public void RemoveFirst() {
            RemoveAt(0);
        }

        /// <inheritdoc />
        public void RemoveLast() {
            int lastItemIndex = Count - 1;
            RemoveAt(lastItemIndex);
        }

        /// <inheritdoc />
        public int IndexOf(TKey key) {
            int i = -1;
            for (IDictionaryEnumerator itr = fOrderedDictionary.GetEnumerator(); itr.MoveNext();) {
                i++;
                if (Equals(key, itr.Key)) return i;
            }
            return -1;
        }

        /// <inheritdoc />
        object IOrderedDictionary.this[int index] {
            get { return fOrderedDictionary[index]; }
            set { fOrderedDictionary[index] = value; }
        }

        /// <inheritdoc />
        IDictionaryEnumerator IDictionary.GetEnumerator() {
            return fOrderedDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        void IDictionary.Remove(object key) {
            Remove((TKey) key);
        }

        /// <inheritdoc />
        object IDictionary.this[object key] {
            get { return fOrderedDictionary[key]; }
            set { fOrderedDictionary[key] = value; }
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key) {
            return fOrderedDictionary.Contains(key);
        }

        /// <inheritdoc />
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) {
            InsertLast(key, value);
        }

        /// <inheritdoc />
        public bool Remove(TKey key) {
            fOrderedDictionary.Remove(key);
            return true;
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value) {
            value = default(TValue);
            if (!fOrderedDictionary.Contains(key)) return false;
            value = this[key];
            return true;
        }

        /// <inheritdoc />
        public TValue this[TKey key] {
            get { return (TValue)fOrderedDictionary[key]; }
            set { fOrderedDictionary[key] = value; }
        }

        /// <inheritdoc />
        TValue IDictionary<TKey, TValue>.this[TKey key] {
            get { return this[key]; }
            set { this[key] = value; }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values { get { return fOrderedDictionary.Values.Cast<TValue>().ToArray(); } }

        /// <inheritdoc />
        public ICollection<TKey> Keys { get { return fOrderedDictionary.Keys.Cast<TKey>().ToArray(); } }

        /// <inheritdoc />
        ICollection IDictionary.Keys { get { return fOrderedDictionary.Keys; } }

        /// <inheritdoc />
        ICollection IDictionary.Values { get { return fOrderedDictionary.Values; } }

        /// <inheritdoc />
        bool IDictionary.IsReadOnly { get { return fOrderedDictionary.IsReadOnly; } }

        /// <inheritdoc />
        bool IDictionary.IsFixedSize { get { return ((IDictionary)fOrderedDictionary).IsFixedSize; } }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return fOrderedDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        void ICollection.CopyTo(Array array, int index) {
            fOrderedDictionary.CopyTo(array, index);
        }

        /// <inheritdoc />
        public int Count { get { return fOrderedDictionary.Count; } }

        /// <inheritdoc />
        object ICollection.SyncRoot { get { return ((ICollection)fOrderedDictionary).SyncRoot; } }

        /// <inheritdoc />
        bool ICollection.IsSynchronized { get { return ((ICollection) fOrderedDictionary).IsSynchronized; } }

        /// <inheritdoc />
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        void IDeserializationCallback.OnDeserialization(object sender) {
            throw new NotSupportedException();
        }
    }
}