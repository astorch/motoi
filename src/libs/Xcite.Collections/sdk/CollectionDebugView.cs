using System;
using System.Collections.Generic;

namespace Xcite.Collections.sdk {
    /// <summary>
    /// Provides a debug view that can be applied to implementations of <see cref="ICollection{TItem}"/>.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class CollectionDebugView<TItem> {
        private readonly ICollection<TItem> iCollection;

        /// <summary>
        /// Creates a new instance for the given <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Collection</param>
        public CollectionDebugView(ICollection<TItem> collection) {
            if (collection == null) throw new ArgumentNullException("collection");
            iCollection = collection;
        }

        /// <summary>
        /// Returns all items of the collection as array.
        /// </summary>
        public TItem[] Items {
            get {
                TItem[] resultArray = new TItem[iCollection.Count];
                iCollection.CopyTo(resultArray, 0);
                return resultArray;
            }
        }

        /// <summary>
        /// Returns the underlying collection type name.
        /// </summary>
        public string CollectionType {
            get { return iCollection.GetType().Name; }
        }
    }
}