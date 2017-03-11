using motoi.platform.resources.model.editors;

namespace motoi.workbench.model {
    /// <summary>
    /// Defines the properties of an editor.
    /// </summary>
    public interface IEditor : ISaveableWorkbenchPart {
        /// <summary>
        /// Returns name of the current editor tab text.
        /// </summary>
        string EditorTabText { get; }

        /// <summary>
        /// Tells the editor to use the given <paramref name="editorInput"/>.
        /// </summary>
        /// <param name="editorInput">Editor input</param>
        void SetEditorInput(IEditorInput editorInput);
    }
}