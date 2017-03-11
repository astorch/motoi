using System;
using System.Collections.Generic;
using System.IO;
using motoi.platform.resources.model;

namespace motoi.platform.resources.runtime.filesystem {
    /// <summary>
    /// Provides an implementation of <see cref="IWorkspaceFolder"/>.
    /// </summary>
    class FileSystemWorkspaceFolder : AbstractFileSystemWorkspaceArtefactContainer, IWorkspaceFolder {
        private IWorkspaceArtefact iParent;

        /// <summary>
        /// Creates a new instance using the given <paramref name="folderInfo"/>.
        /// </summary>
        /// <param name="folderInfo">Directory info</param>
        /// <exception cref="ArgumentNullException">If the given directory reference is NULL</exception>
        public FileSystemWorkspaceFolder(DirectoryInfo folderInfo) : base(folderInfo) {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        public override string Nature { get { return "FileSystem.Folder"; } }

        /// <inheritdoc />
        public override IWorkspaceArtefact Parent {
            get { return iParent ?? (iParent = new FileSystemWorkspaceFolder(FileSystemDirectory.Parent)); }
        }

        /// <inheritdoc />
        protected override void PerformRefresh(ERefreshBehavior refreshBehavior, DirectoryInfo fileSystemDirectory, List<IWorkspaceArtefact> containerArtefacts) {
            containerArtefacts.Clear();

            // TODO Implement support of refresh behavior

            // Directories
            DirectoryInfo[] subDirectories = fileSystemDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = -1; ++i != subDirectories.Length; ) {
                DirectoryInfo subDirectory = subDirectories[i];
                FileSystemWorkspaceFolder fsWorkspaceFolder = new FileSystemWorkspaceFolder(subDirectory);
                fsWorkspaceFolder.Refresh();
                containerArtefacts.Add(fsWorkspaceFolder);
            }

            // Files
            FileInfo[] files = fileSystemDirectory.GetFiles("*", SearchOption.TopDirectoryOnly);
            for (int i = -1; ++i != files.Length; ) {
                FileInfo file = files[i];
                FileSystemFile fsFile = new FileSystemFile(file);
                containerArtefacts.Add(fsFile);
            }
        }
    }
}