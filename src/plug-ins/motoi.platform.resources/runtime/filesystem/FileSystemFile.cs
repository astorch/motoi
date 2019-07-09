using System;
using System.IO;
using motoi.platform.resources.model;

namespace motoi.platform.resources.runtime.filesystem {
    /// <summary> Provides an implementation of <see cref="IWorkspaceFile"/>. </summary>
    public class FileSystemFile : AbstractFileSystemWorkspaceFile {
        private IWorkspaceArtefact _parent;

        /// <summary> Creates a new instance using the associated <paramref name="file"/>. </summary>
        /// <param name="file">File system file</param>
        /// <exception cref="ArgumentNullException">If <paramref name="file"/> is NULL</exception>
        public FileSystemFile(FileInfo file) : base(file) {
            // Currently nothing to do here
        }

        /// <summary> Returns the nature of the artefact. </summary>
        public override string Nature 
            => "FileSystem.File";

        /// <inheritdoc />
        public override IWorkspaceArtefact Parent 
            => _parent ?? (_parent = new FileSystemWorkspaceFolder(FileSystemFile.Directory));
    }
}