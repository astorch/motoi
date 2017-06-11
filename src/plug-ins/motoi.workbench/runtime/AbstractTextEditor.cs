using System.IO;
using motoi.platform.resources.model.editors;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;
using motoi.workbench.model;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="ITextEditor"/> that is based on <see cref="AbstractEditor"/>.
    /// </summary>
    public abstract class AbstractTextEditor : AbstractEditor, ITextEditor {
        private string iEditorText;

        /// <inheritdoc />
        public virtual string EditorText {
            get { return iEditorText; }
            set {
                iEditorText = value;
                IsDirty = true;
            }
        }

        /// <summary> Returns the editor input. </summary>
        public virtual IEditorInput EditorInput { get; private set; }

        /// <inheritdoc />
        public override void SetEditorInput(IEditorInput editorInput) {
            IEditorInput input = editorInput;

            string fileContent;
            using (BufferedStream bufferedStream = new BufferedStream(input.OpenRead())) {
                using (StreamReader streamReader = new StreamReader(bufferedStream)) {
                    fileContent = streamReader.ReadToEnd();
                }
            }

            EditorInput = input;
            EditorText = fileContent;
            DispatchPropertyChanged(nameof(EditorText));
            EditorTabText = input.Name;
            IsDirty = false;
        }

        /// <summary> Returns the currently used rich text box to display the text. </summary>
        protected IRichTextBox TextBox { get; private set; }

        /// <inheritdoc />
        public override void CreateContents(IGridPanel gridComposite) {
            gridComposite.GridColumns = 1;
            gridComposite.GridRows = 1;
            IContentAssistTextBox richTextBox = WidgetFactory.CreateInstance<IContentAssistTextBox>(gridComposite);
            gridComposite.AddWidget(richTextBox);
            TextBox = richTextBox;

            DataBindingOperator.Apply(richTextBox, PRichTextBox.TextProperty, new DataBinding(this, nameof(EditorText)));
        }
    }
}