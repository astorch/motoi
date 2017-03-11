using System.IO;

namespace motoi.platform.resources.model {
    /// <summary>
    /// Defines a workspace
    /// </summary>
    public interface IWorkspace : IWorkspaceArtefactContainer {
        /// <summary>
        /// Returns an <see cref="IWorkspaceFile"/> reference for the given <paramref name="fileInfo"/>. If the file info 
        /// is NULL or points to a file that does not exist, NULL is returned. Additionally, NULL is returned when the file 
        /// exists outside the workspace.
        /// </summary>
        /// <param name="fileInfo">File info</param>
        /// <returns>Instance of <see cref="IWorkspaceFile"/> or NULL</returns>
        IWorkspaceFile GetWorkspaceFile(FileInfo fileInfo);

        /// <summary>
        /// Returns an <see cref="IWorkspaceFolder"/> reference for the given <paramref name="directoryInfo"/>. If the directory info 
        /// is NULL or points to a folder that does not exists, NULL is returned. Additionally, NULL is returned when the directory 
        /// exists outside the workspace.
        /// </summary>
        /// <param name="directoryInfo">Directory info</param>
        /// <returns>Instance of <see cref="IWorkspaceFolder"/> or NULL</returns>
        IWorkspaceFolder GetWorkspaceFolder(DirectoryInfo directoryInfo);
    }
}