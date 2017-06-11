using motoi.platform.resources.model.editors;
using motoi.workbench.model;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IEditor"/> that is based on <see cref="AbstractSaveableWorkbenchPart"/>.
    /// </summary>
    public abstract class AbstractEditor : AbstractSaveableWorkbenchPart, IEditor {
        private string iEditorTabText;

        /// <inheritdoc />
        public virtual string EditorTabText {
            get { return iEditorTabText; }
            protected set {
                if (iEditorTabText == value) return;
                iEditorTabText = value;
                DispatchPropertyChanged(nameof(EditorTabText));
            }
        }

        /// <inheritdoc />
        public abstract void SetEditorInput(IEditorInput editorInput);
    }
}