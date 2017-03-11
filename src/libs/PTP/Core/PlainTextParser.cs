using System;
using System.IO;
using System.Linq;

namespace PTP.Core
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class PlainTextParser {
        /// <summary>
        /// Backing variable of the instance.
        /// </summary>
		private static PlainTextParser iInstance;	
		
        /// <summary>
        /// Returns the instance of the parser.
        /// </summary>
		public static PlainTextParser Instance {
			get { return iInstance ?? ( iInstance = new PlainTextParser()); }
		}
		
		private static readonly char[] ValueAssignTokens = { '=', ':' };
		
		/// <summary>
		/// Private construktor.
		/// </summary>
		private PlainTextParser() { }
		
        /// <summary>
        /// Parses the given file with the given splitter tokens.
        /// </summary>
        /// <param name="fileInfo">File to parse</param>
        /// <param name="blockSplitter">Splitter tokens for a block unit</param>
        /// <param name="chunkSplitter">Splitter tokens for a chunk unit</param>
        /// <param name="nlNoBlock">Tokens that mark an ignorable newline character</param>
        /// <param name="inlineSplitter">Splitter tokens for inline value enumerations</param>
        /// <returns>Instance of IPlainTextDocument with the parsed values</returns>
        public IPlainTextDocument Parse(FileInfo fileInfo, string[] blockSplitter, string[] chunkSplitter, string[] nlNoBlock, string[] inlineSplitter) {
			IPlainTextDocument document = Parse(fileInfo.OpenRead(), blockSplitter, chunkSplitter, nlNoBlock, inlineSplitter);
            return document;
		}
		
        /// <summary>
        /// Parses the given stream with the given splitter tokens.
        /// </summary>
        /// <param name="stream">Stream to parse</param>
        /// <param name="blockSplitter">Splitter tokens for a block unit</param>
        /// <param name="chunkSplitter">Splitter tokens for a chunk unit</param>
        /// <param name="nlNoBlock">Tokens that mark an ignorable newline character</param>
        /// <param name="inlineSplitter">Splitter tokens for inline value enumerations</param>
        /// <returns>Instance of IPlainTextDocument with the parsed values</returns>
		public IPlainTextDocument Parse(Stream stream, string[] blockSplitter, string[] chunkSplitter, string[] nlNoBlock, string[] inlineSplitter) {
			// Read the input stream
			TextReader tr = new StreamReader(stream);
			String fileContent = tr.ReadToEnd();
			tr.Close();
			
			// Create the result document
			PlainTextDocumentImpl document = new PlainTextDocumentImpl();
			
			// No file content
			if(string.IsNullOrEmpty(fileContent))
				return null;
			
			// Removing escaped newline tokens
			for(int i = 0; i < nlNoBlock.Length; i++) {
				string nlToken = nlNoBlock[i];
				string rplToken = string.Format(@"{0}\n", nlToken);
				fileContent = fileContent.Replace(rplToken, string.Empty);
			}
			
			// Splitting contents into blocks
			string[] blocks;
			if(blockSplitter.Length > 0)
				blocks =  fileContent.Split(blockSplitter, StringSplitOptions.RemoveEmptyEntries);
			else
				blocks = new[] { fileContent };
			
			// Processing the blocks
			for(int i = 0; i < blocks.Length; i++) {
				string block = blocks[i];
				
				// Splitting into chunks
				string[] chunks = block.Split(chunkSplitter, StringSplitOptions.RemoveEmptyEntries);
				
				// Processing chunks
				for(int j = 0; j < chunks.Length; j++) {
					string chunkStr = chunks[j];
					string normChunkStr = chunkStr.Replace(Environment.NewLine, string.Empty);
					Chunk chunk = ParseChunk(normChunkStr, inlineSplitter);
					document.Chunks.Add(chunk);
				}
			}
			
			return document;
		}
		
		/// <summary>
		/// Parses the given string and creates an instance of <see cref="Chunk"/>
		/// </summary>
		/// <param name="chunkStr">String representing a chunk</param>
		/// <param name="inlineSplitter">Splitter tokens for inline splits</param>
		/// <returns>Instance of a Chunk</returns>
        private Chunk ParseChunk(string chunkStr, string[] inlineSplitter) {			
			// There must be some informations
			if(string.IsNullOrEmpty(chunkStr))
				return null;
			
			// Split into chunk parts
			string[] chunkParts = chunkStr.Split( ValueAssignTokens );
			string key = chunkParts[0];
			string value = chunkParts[1];
			
			// Trim value
			value = value.Trim();
			
			// Split into values
			string[] values = value.Split(inlineSplitter, StringSplitOptions.RemoveEmptyEntries);
			
			// If there is only one value embed him
			if(values.Length == 0 || values.Length == 1)
				values = new[] { value };
			
			// Trim every value
			values = values.Select(val => val.Trim()).ToArray();
			
			// Create chunk and return
			Chunk chunk = new Chunk(key, values);
			return chunk;
		}
	}
}