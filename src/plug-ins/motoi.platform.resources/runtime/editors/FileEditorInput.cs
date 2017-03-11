using System;
using System.IO;
using motoi.platform.resources.model.editors;
using motoi.platform.resources.runtime.filesystem;

namespace motoi.platform.resources.runtime.editors {
    /// <summary>
    /// Provides an implementation of <see cref="IEditorInput"/>.
    /// </summary>
    public class FileEditorInput : FileSystemFile, IEditorInput {
        /// <summary>
        /// Creates a new instance using the given <paramref name="location"/>.
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <exception cref="ArgumentNullException">if <paramref name="location"/> is NULL</exception>
        /// <exception cref="ArgumentException">If <paramref name="location"/> points to no existing file</exception>
        public FileEditorInput(string location) : this(new Uri(location)) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance using the given <paramref name="location"/>.
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <exception cref="ArgumentNullException">if <paramref name="location"/> is NULL</exception>
        /// <exception cref="ArgumentException">If <paramref name="location"/> points to no existing file</exception>
        public FileEditorInput(Uri location) : this(new FileInfo(location.LocalPath)) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance using the associated <paramref name="file"/>.
        /// </summary>
        /// <param name="file">File system file</param>
        /// <exception cref="ArgumentNullException">If <paramref name="file"/> is NULL</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="file"/> does not exist</exception>
        public FileEditorInput(FileInfo file) : base(file) {
            if (!file.Exists) throw new ArgumentException(string.Format("Given file '{0}' does not exist!", file.FullName));
        }

        /// <summary>
        /// Opens a stream to the editor input data using read mode.
        /// </summary>
        /// <returns>Stream to read</returns>
        public Stream OpenRead() {
            return FileSystemFile.OpenRead();
        }

        /// <summary>
        /// Opens a stream to the editor input data using write mode.
        /// </summary>
        /// <returns>Stream to write</returns>
        public Stream OpenWrite() {
            return FileSystemFile.Create();
        }
    }
}