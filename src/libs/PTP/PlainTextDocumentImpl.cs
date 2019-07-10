using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PTP {
    /// <summary> Provides a default implementation of <see cref="IPlainTextDocument"/> </summary>
    internal class PlainTextDocumentImpl : IPlainTextDocument {
        /// <inheritdoc />
        public PlainTextDocumentImpl() {
            Chunks = new List<Chunk>(10);
        }

        /// <summary> Returns the collection of chunks. </summary>
        public IList<Chunk> Chunks { get; }
        
        /// <inheritdoc />
        public string SelectValue(string xplainQuery) {
            string[] resultSet = SelectValues(xplainQuery);
            if (resultSet == null || resultSet.Length == 0)
                return null;

            return resultSet[0];
        }
        
        /// <inheritdoc />
        public string[] SelectValues(string xplainQuery) {
            if (xplainQuery.Contains("/"))
                throw new NotImplementedException();

            string lookupKey = xplainQuery.ToLowerInvariant();

            Chunk[] chunks = Chunks.ToArray();
            for (int i = 0; i < chunks.Length; i++) {
                Chunk chunk = chunks[i];
                if (chunk.Key.ToLowerInvariant() == lookupKey)
                    return chunk.Values;
            }

            return null;
        }
        
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        
        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator() {
            for (int i = -1; ++i < Chunks.Count;) {
                Chunk chunk = Chunks[i];
                KeyValuePair<string, string[]> pair = new KeyValuePair<string, string[]>(chunk.Key, chunk.Values);
                yield return pair;
            }
        }
    }
}