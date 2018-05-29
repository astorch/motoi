using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PTP
{
	/// <summary>
	/// Provides a default implementation of <see cref="IPlainTextDocument"/>
	/// </summary>
	internal class PlainTextDocumentImpl : IPlainTextDocument {
        /// <summary>
        /// Constructor.
        /// </summary>
		public PlainTextDocumentImpl() {
			Chunks = new List<Chunk>(10);
		}
		
        /// <summary>
        /// Returns the collection of chunks.
        /// </summary>
		public IList<Chunk> Chunks  { get; private set; }

	    /// <summary>
	    /// Selects the value referenced by the given xplain query. If the value doesn't 
	    /// exists null will be returned.
	    /// </summary>
        /// <param name="xplainQuery">Xplain query</param>
	    /// <returns>Value or null</returns>
	    public string SelectValue(string xplainQuery) {
			string[] resultSet = SelectValues(xplainQuery);
			if(resultSet == null || resultSet.Length == 0)
				return null;
			
			return resultSet[0];
		}

	    /// <summary>
	    /// Selects the valuew referenced by the given xplain query. If the values don't 
	    /// exists null will be returned.
	    /// </summary>
	    /// <param name="xplainQuery">Xplain query</param>
	    /// <returns>Values or null</returns>
	    public string[] SelectValues(string xplainQuery) {
			if(xplainQuery.Contains("/"))
				throw new NotImplementedException();
			
			string lookupKey = xplainQuery.ToLowerInvariant();
			
			Chunk[] chunks = Chunks.ToArray();
			for(int i = 0; i < chunks.Length; i++) {
				Chunk chunk = chunks[i];
				if(chunk.Key.ToLowerInvariant() == lookupKey)
					return chunk.Values;
			}
			
			return null;
		}

	    /// <summary>
	    /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
	    /// </summary>
	    /// <returns>
	    /// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
	    /// </returns>
	    /// <filterpriority>2</filterpriority>
	    IEnumerator IEnumerable.GetEnumerator() {
	        return GetEnumerator();
	    }


	    /// <summary>
	    /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
	    /// </summary>
	    /// <returns>
	    /// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
	    /// </returns>
	    /// <filterpriority>1</filterpriority>
	    public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator() {
	        for (int i = -1; ++i < Chunks.Count;) {
                Chunk chunk = Chunks[i];
                KeyValuePair<string, string[]> pair = new KeyValuePair<string, string[]>(chunk.Key, chunk.Values);
	            yield return pair;
	        }
	    }
	}
}
