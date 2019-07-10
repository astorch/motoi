namespace motoi.workbench.model {
    /// <summary>
    /// Extends <see cref="IEditor"/> and defines additional properties and methods 
    /// used for text editors.
    /// </summary>
    public interface ITextEditor : IEditor {
        /// <summary> Returns the text that is edited or does set it. </summary>
        string EditorText { get; set; }
    }
}