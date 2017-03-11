﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Xcite.Collections {
    /// <summary>
    /// Provides common extensions for collections.
    /// </summary>
    public static class ListMethodExtensions {
        /// <summary>
        /// Adds the given enumerable collection of items to current list.
        /// </summary>
        /// <typeparam name="TItem">Type of list items</typeparam>
        /// <param name="list">List which will receive the new items</param>
        /// <param name="additionalItems">Items that will be added to the current list</param>
        public static void AddAll<TItem>(this IList<TItem> list, IEnumerable<TItem> additionalItems) {
            for (IEnumerator<TItem> enmtor = additionalItems.GetEnumerator(); enmtor.MoveNext();) {
                TItem item = enmtor.Current;
                list.Add(item);
            }
        }

        /// <summary>
        /// Adds the given elements of the array to current list.
        /// </summary>
        /// <typeparam name="TItem">Type of list items</typeparam>
        /// <param name="list">List which will receive the new items</param>
        /// <param name="additionalItems">Items that will be added to the current list</param>
        public static void AddAll<TItem>(this IList<TItem> list, TItem[] additionalItems) {
            for (int i = -1; i++ < additionalItems.Length; )
                list.Add(additionalItems[i]);
        }

        /// <summary>
        /// Adds the elements of the given collection to current list.
        /// </summary>
        /// <typeparam name="TItem">Type of list items</typeparam>
        /// <param name="list">List which will receive the new items</param>
        /// <param name="additionalItems">Items that will be added to the current list</param>
        public static void AddAll<TItem>(this IList<TItem> list, IList<TItem> additionalItems) {
            for (int i = -1; ++i < additionalItems.Count; )
                list.Add(additionalItems[i]);
        }

        /// <summary>
        /// Returns the index of the element that matches the given <paramref name="predicate"/> within the list. 
        /// If no element is found, -1 is returned.
        /// </summary>
        /// <typeparam name="TItem">Type of list items</typeparam>
        /// <param name="list">List to process</param>
        /// <param name="predicate">Predicate to match the deserved item</param>
        /// <returns>Index of element or -1</returns>
        public static int IndexOf<TItem>(this IList<TItem> list, Predicate<TItem> predicate) {
            for (int i = -1; ++i != list.Count;) {
                if (predicate(list[i])) return i;
            }

            return -1;
        }

        /// <summary>
        /// Defines the ToString()-Format for a dictionary.
        /// </summary>
        private const string DictionaryToStringFormat = "{0}='{1}'; ";

        /// <summary>
        /// Reusable string builder
        /// </summary>
        private static readonly StringBuilder iStringBuilder = new StringBuilder();

        /// <summary>
        /// Returns a formatted string representation of a dictionary.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <returns>String representation of the dictionary</returns>
        public static string ToFormattedString<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> map)
        {
            iStringBuilder.Clear();
            iStringBuilder.Append('{');
            for (IEnumerator<KeyValuePair<TKey, TValue>> enmtor = map.GetEnumerator(); enmtor.MoveNext(); ) {
                KeyValuePair<TKey, TValue> pair = enmtor.Current;
                iStringBuilder.AppendFormat(DictionaryToStringFormat, pair.Key, pair.Value);
            }
            iStringBuilder.Append('}');
            return iStringBuilder.ToString();
        }
    }
}
