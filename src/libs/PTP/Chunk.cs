namespace PTP {
    /// <summary> Defines a chunk. </summary>
    public class Chunk {
        /// <summary> Internal constructor. </summary>
        /// <param name="key">Key of the chunk</param>
        /// <param name="values">Values within the chunk</param>
        internal Chunk(string key, string[] values) {
            Key = key;
            Values = values;
        }

        /// <summary> Returns the key of the chunk. </summary>
        public string Key { get; }

        /// <summary> Returns the values within the chunk. </summary>
        public string[] Values { get; }
        
        /// <inheritdoc />
        public override string ToString() {
            string value = string.Join(",", Values);
            const string toStringFormat = "Chunk { {0}={1} }";
            string result = string.Format(toStringFormat, Key, value);
            return result;
        }
    }
}