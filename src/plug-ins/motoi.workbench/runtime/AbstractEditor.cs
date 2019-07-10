using motoi.platform.resources.model.editors;
using motoi.workbench.model;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IEditor"/>
    /// that is based on <see cref="AbstractSaveableWorkbenchPart"/>.
    /// </summary>
    public abstract class AbstractEditor : AbstractSaveableWorkbenchPart, IEditor {
        private string _editorTabText;

        /// <inheritdoc />
        public virtual string EditorTabText {
            get { return _editorTabText; }
            protected set {
                if (_editorTabText == value) return;
                _editorTabText = value;
                DispatchPropertyChanged(nameof(EditorTabText));
            }
        }

        /// <inheritdoc />
        public abstract void SetEditorInput(IEditorInput editorInput);
    }
}