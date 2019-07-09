using System.Collections.Generic;
using System.IO;
using motoi.platform.resources.model;

namespace motoi.platform.resources.runtime.filesystem {
    /// <summary> Provides an implementation of <see cref="IWorkspace"/>. </summary>
    class FileSystemWorkspace : FileSystemWorkspaceFolder, IWorkspace {
        /// <summary> Creates a new instance for the given <paramref name="workspaceDirectoryInfo"/>. </summary>
        /// <param name="workspaceDirectoryInfo">Workspace directory info</param>
        public FileSystemWorkspace(DirectoryInfo workspaceDirectoryInfo) : base(workspaceDirectoryInfo) {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        public override string Nature 
            => "FileSystem.Workspace";

        /// <inheritdoc />
        public override IWorkspaceArtefact Parent 
            => null;

        /// <inheritdoc />
        public virtual IWorkspaceFile GetWorkspaceFile(FileInfo fileInfo) {
            if (fileInfo == null) return null;
            if (!fileInfo.Exists) return null;

            DirectoryInfo fileDirectory = fileInfo.Directory;
            IWorkspaceFolder workspaceFolder = GetWorkspaceFolder(fileDirectory);
            if (workspaceFolder == null) return null;

            string fileName = fileInfo.Name;
            IWorkspaceArtefact folderArtefact = workspaceFolder.GetArtefact(fileName);
            return folderArtefact as IWorkspaceFile;
        }

        /// <inheritdoc />
        public virtual IWorkspaceFolder GetWorkspaceFolder(DirectoryInfo directoryInfo) {
            if (directoryInfo == null) return null;
            if (!directoryInfo.Exists) return null;

            string dirPath = directoryInfo.FullName;
            using (IEnumerator<IWorkspaceFolder> folderItr = FlatHierarchy<IWorkspaceFolder>().GetEnumerator()) {
                while (folderItr.MoveNext()) {
                    IWorkspaceFolder folder = folderItr.Current;
                    if (folder == null) continue;
                    
                    string folderPath = folder.Location.LocalPath;
                    if (folderPath == dirPath) return folder;
                }
            }

            return null;
        }
    }
}