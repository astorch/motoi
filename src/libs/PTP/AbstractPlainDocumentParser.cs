using System;
using System.IO;

namespace PTP {
    /// <summary>
    /// Provides an abstract implementation of a plain document (text file) 
    /// parser.
    /// </summary>
    public abstract class AbstractPlainDocumentParser {
        /// <summary> Creates a new parser for the given file. </summary>
        /// <param name="fileInfo">File to parse</param>
        protected AbstractPlainDocumentParser(FileInfo fileInfo) : this(fileInfo.OpenRead()) {
            File = fileInfo;
        }

        /// <summary> Creates a new parser for the given stream. </summary>
        /// <param name="fileStream">Stream to read</param>
        protected AbstractPlainDocumentParser(Stream fileStream) {
            Stream = fileStream;
        }
        
        /// <inheritdoc />
        ~AbstractPlainDocumentParser() {
            if (File != null)
                Stream.Close();
        }

        /// <summary> Returns the block splitter tokens. </summary>
        protected abstract string[] BlockSplitter { get; }

        /// <summary> Returns the chunk splitter tokens. </summary>
        protected abstract string[] ChunkSplitter { get; }

        /// <summary> Returns the ignorable newline tokens. </summary>
        protected abstract string[] NLNoBlock { get; }

        /// <summary> Returns the inline splitter tokens. </summary>
        protected abstract string[] InlineSplitter { get; }

        /// <summary> Returns the file info that is used by this instance. </summary>
        public FileInfo File { get; }

        /// <summary> Returns the file stream that is used by this instance. </summary>
        public Stream Stream { get; }

        /// <summary>
        /// Parses the given file and returns an instance of <see cref="IPlainTextDocument"/>.
        /// </summary>
        /// <returns>Instance of IPlainTextDocument</returns>
        public virtual IPlainTextDocument Parse() {
            if (Stream == null)
                throw new NullReferenceException("File stream is null!");

            IPlainTextDocument document = PlainTextParser.Instance.Parse(Stream, BlockSplitter, ChunkSplitter,
                NLNoBlock, InlineSplitter);
            return document;
        }
    }
}