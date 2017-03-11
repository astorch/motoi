using System.Collections.Generic;

namespace PTP.Core
{
	/// <summary>
	/// Defines a PlainTextDocument.
	/// </summary>
	public interface IPlainTextDocument : IEnumerable<KeyValuePair<string,string[]>> {
		/// <summary>
		/// Selects the value referenced by the given xplain query. If the value doesn't 
		/// exists null will be returned.
		/// </summary>
        /// <param name="xplainQuery">Xplain query</param>
		/// <returns>Value or null</returns>
		string SelectValue(string xplainQuery);
		
		/// <summary>
		/// Selects the valuew referenced by the given xplain query. If the values don't 
		/// exists null will be returned.
		/// </summary>
        /// <param name="xplainQuery">Xplain query</param>
		/// <returns>Values or null</returns>
		string[] SelectValues(string xplainQuery);
	}
}
