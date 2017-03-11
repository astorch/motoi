using motoi.platform.resources.model.editors;
using motoi.platform.ui.toolbars;
using motoi.workbench.model;
using Xcite.Csharp.lang;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IEditor"/> that is based on <see cref="AbstractSaveableWorkbenchPart"/>.
    /// </summary>
    public abstract class AbstractEditor : AbstractSaveableWorkbenchPart, IEditor {
        private string iEditorTabText;

        /// <summary>
        /// Returns name of the current editor tab text.
        /// </summary>
        public virtual string EditorTabText {
            get { return iEditorTabText; }
            protected set {
                if (iEditorTabText == value) return;
                iEditorTabText = value;
                DispatchPropertyChanged(Name.Of(() => EditorTabText));
            }
        }

        /// <summary>
        /// Tells the editor to add additional tool bar controls if desired.
        /// </summary>
        /// <param name="toolBar">Tool bar to configure</param>
        public abstract void ConfigureToolBar(IToolBar toolBar);

        /// <summary>
        /// Tells the editor to use the given <paramref name="editorInput"/>.
        /// </summary>
        /// <param name="editorInput">Editor input</param>
        public abstract void SetEditorInput(IEditorInput editorInput);
    }
}