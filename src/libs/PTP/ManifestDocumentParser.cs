using System;
using System.IO;

namespace PTP
{
    /// <summary>
    /// Provides an implementation of <see cref="AbstractPlainDocumentParser"/> 
    /// to parse manifest files.
    /// </summary>
    public class ManifestDocumentParser : AbstractPlainDocumentParser
    {
        /// <summary>
        /// Creates a new parser for the given file.
        /// </summary>
        /// <param name="fileInfo">File info</param>
        public ManifestDocumentParser(FileInfo fileInfo) : base(fileInfo) { }

        /// <summary>
        /// Creates a new parser for the given stream.
        /// </summary>
        /// <param name="fileStream">Stream to parse</param>
        public ManifestDocumentParser(Stream fileStream) : base(fileStream) { }

        /// <summary>
        /// Returns the block splitter tokens.
        /// </summary>
        protected override string[] BlockSplitter  { get { return new string[0]; } }

        /// <summary>
        /// Returns the chunk splitter tokens.
        /// </summary>
        protected override string[] ChunkSplitter  { get { return new[] {Environment.NewLine}; } }

        /// <summary>
        /// Returns the ignorable newline tokens.
        /// </summary>
        protected override string[] NLNoBlock      { get { return new[] {"/"}; } }

        /// <summary>
        /// Returns the inline splitter tokens.
        /// </summary>
        protected override string[] InlineSplitter { get { return new[] {","}; } }
    }
}