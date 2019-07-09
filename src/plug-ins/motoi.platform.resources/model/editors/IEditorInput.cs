using System.IO;

namespace motoi.platform.resources.model.editors {
    /// <summary> Defines the input of an editor. </summary>
    public interface IEditorInput : IWorkspaceFile {
        /// <summary> Opens a stream to the editor input data using read mode. </summary>
        /// <returns>Stream to read</returns>
        Stream OpenRead();

        /// <summary> Opens a stream to the editor input data using write mode. </summary>
        /// <returns>Stream to write</returns>
        Stream OpenWrite();
    }
}