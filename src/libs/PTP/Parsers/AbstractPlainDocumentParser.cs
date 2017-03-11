using System;
using System.IO;
using PTP.Core;

namespace PTP.Parsers
{
    /// <summary>
    /// Provides an abstract implementation of a plain document (text file) 
    /// parser.
    /// </summary>
    public abstract class AbstractPlainDocumentParser
    {
        /// <summary>
        /// Backing variable for the file info.
        /// </summary>
        private readonly FileInfo iFileInfo;

        /// <summary>
        /// Backing variable for the file stream.
        /// </summary>
        private readonly Stream iFileStream;

        /// <summary>
        /// Creates a new parser for the given file.
        /// </summary>
        /// <param name="fileInfo">File to parse</param>
        protected AbstractPlainDocumentParser(FileInfo fileInfo) : this(fileInfo.OpenRead()) {
            iFileInfo = fileInfo;
        }

        /// <summary>
        /// Creates a new parser for the given stream.
        /// </summary>
        /// <param name="fileStream">Stream to read</param>
        protected AbstractPlainDocumentParser(Stream fileStream) {
            iFileStream = fileStream;
        }

        /// <summary>
        /// Gibt einem <see cref="T:System.Object"/> Gelegenheit zu dem Versuch, Ressourcen freizugeben und andere Bereinigungen durchzuführen, bevor das <see cref="T:System.Object"/> von der Garbage Collection freigegeben wird.
        /// </summary>
        ~AbstractPlainDocumentParser() {
            if(iFileInfo != null)
                iFileStream.Close();
        }

        /// <summary>
        /// Returns the block splitter tokens.
        /// </summary>
        protected abstract string[] BlockSplitter { get; }

        /// <summary>
        /// Returns the chunk splitter tokens.
        /// </summary>
        protected abstract string[] ChunkSplitter { get; }

        /// <summary>
        /// Returns the ignorable newline tokens.
        /// </summary>
        protected abstract string[] NLNoBlock { get; }

        /// <summary>
        /// Returns the inline splitter tokens.
        /// </summary>
        protected abstract string[] InlineSplitter { get; }

        /// <summary>
        /// Returns the file info that is used by this instance.
        /// </summary>
        public FileInfo File { get { return iFileInfo; } }

        /// <summary>
        /// Returns the file stream that is used by this instance.
        /// </summary>
        public Stream Stream { get { return iFileStream; } }

        /// <summary>
        /// Parses the given file and returns an instance of <see cref="IPlainTextDocument"/>.
        /// </summary>
        /// <returns>Instance of IPlainTextDocument</returns>
        public virtual IPlainTextDocument Parse()
        {
            if (iFileStream == null)
                throw new NullReferenceException("File stream is null!");

            IPlainTextDocument document = PlainTextParser.Instance.
                Parse(iFileStream, BlockSplitter, ChunkSplitter, NLNoBlock, InlineSplitter);
            return document;
        }
    }
}