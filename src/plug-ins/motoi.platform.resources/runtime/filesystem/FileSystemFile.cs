using System;
using System.IO;
using motoi.platform.resources.model;

namespace motoi.platform.resources.runtime.filesystem {
    /// <summary>
    /// Provides an implementation of <see cref="IWorkspaceFile"/>.
    /// </summary>
    public class FileSystemFile : AbstractFileSystemWorkspaceFile {
        private IWorkspaceArtefact iParent;

        /// <summary>
        /// Creates a new instance using the associated <paramref name="file"/>.
        /// </summary>
        /// <param name="file">File system file</param>
        /// <exception cref="ArgumentNullException">If <paramref name="file"/> is NULL</exception>
        public FileSystemFile(FileInfo file) : base(file) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Returns the nature of the artefact.
        /// </summary>
        public override string Nature { get { return "FileSystem.File"; } }

        /// <inheritdoc />
        public override IWorkspaceArtefact Parent {
            get { return iParent ?? (iParent = new FileSystemWorkspaceFolder(FileSystemFile.Directory)); }
        }
    }
}