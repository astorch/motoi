﻿using System;
using System.IO;

namespace PTP {
    /// <summary>
    /// Provides an implementation of <see cref="AbstractPlainDocumentParser"/> 
    /// to parse manifest files.
    /// </summary>
    public class ManifestDocumentParser : AbstractPlainDocumentParser {
        /// <summary> Creates a new parser for the given file. </summary>
        /// <param name="fileInfo">File info</param>
        public ManifestDocumentParser(FileInfo fileInfo) : base(fileInfo) {
            // Nothing to do here
        }

        /// <summary> Creates a new parser for the given stream. </summary>
        /// <param name="fileStream">Stream to parse</param>
        public ManifestDocumentParser(Stream fileStream) : base(fileStream) {
            // Nothing to do here
        }

        /// <inheritdoc />
        protected override string[] BlockSplitter 
            => new string[0];

        /// <inheritdoc />
        protected override string[] ChunkSplitter 
            => new[] {Environment.NewLine};


        /// <inheritdoc />
        protected override string[] NLNoBlock 
            => new[] {"/"};

        /// <inheritdoc />
        protected override string[] InlineSplitter 
            => new[] {","};
    }
}