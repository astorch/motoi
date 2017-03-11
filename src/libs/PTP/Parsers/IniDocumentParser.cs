using System;
using System.IO;

namespace PTP.Parsers
{
    /// <summary>
    /// Provides an implementation of <see cref="AbstractPlainDocumentParser"/> 
    /// to parse ini files.
    /// </summary>
    public class IniDocumentParser : AbstractPlainDocumentParser {

        /// <summary>
        /// Creates a new parser for the given file.
        /// </summary>
        /// <param name="fileInfo">File info</param>
        public IniDocumentParser(FileInfo fileInfo) : base(fileInfo) { }

        /// <summary>
        /// Creates a new parser for the given stream.
        /// </summary>
        /// <param name="fileStream">Stream to parse</param>
        public IniDocumentParser(Stream fileStream) : base(fileStream) { }

        /// <summary>
        /// Returns the block splitter tokens.
        /// </summary>
        protected override string[] BlockSplitter { get { return new string[0]; } }

        /// <summary>
        /// Returns the chunk splitter tokens.
        /// </summary>
        protected override string[] ChunkSplitter { get { return new[] { Environment.NewLine };  } }

        /// <summary>
        /// Returns the ignorable newline tokens.
        /// </summary>
        protected override string[] NLNoBlock { get { return new[] { "/" }; } }

        /// <summary>
        /// Returns the inline splitter tokens.
        /// </summary>
        protected override string[] InlineSplitter { get { return new string[0]; } }
    }
}