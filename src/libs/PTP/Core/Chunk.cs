namespace PTP.Core
{
	/// <summary>
	/// Defines a chunk.
	/// </summary>
	public class Chunk
	{
        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="key">Key of the chunk</param>
        /// <param name="values">Values within the chunk</param>
		internal Chunk(string key, string[] values) { 
			Key = key;
			Values = values;
		}
		
        /// <summary>
        /// Returns the key of the chunk.
        /// </summary>
		public string Key { get; private set; }
		
        /// <summary>
        /// Returns the values within the chunk.
        /// </summary>
		public string[] Values { get; private set; }

	    /// <summary>
	    /// Gibt einen <see cref="T:System.String"/> zurück, der das aktuelle <see cref="T:System.Object"/> darstellt.
	    /// </summary>
	    /// <returns>
	    /// Ein <see cref="T:System.String"/>, der das aktuelle <see cref="T:System.Object"/> darstellt.
	    /// </returns>
	    /// <filterpriority>2</filterpriority>
	    public override string ToString()
        {
            string value = string.Join(",", Values);
            string result = string.Format(ToStringFormat, Key, value);
            return result;
        }

        /// <summary>
        /// ToString-Format
        /// </summary>
	    private const string ToStringFormat = "Chunk { {0}={1} }";
	}
}
