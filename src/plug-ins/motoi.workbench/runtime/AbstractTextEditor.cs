using System.IO;
using motoi.platform.resources.model.editors;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;
using motoi.workbench.model;
using Xcite.Csharp.lang;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="ITextEditor"/> that is based on <see cref="AbstractEditor"/>.
    /// </summary>
    public abstract class AbstractTextEditor : AbstractEditor, ITextEditor {
        private string iEditorText;

        /// <summary>
        /// Returns the text that is edited or does set it. Note that the setter won't raise 
        /// a property changed event.
        /// </summary>
        public virtual string EditorText {
            get { return iEditorText; }
            set {
                iEditorText = value;
                IsDirty = true;
            }
        }

        /// <summary>
        /// Returns the editor input.
        /// </summary>
        public virtual IEditorInput EditorInput { get; private set; }

        /// <summary>
        /// Tells the editor to use the given <paramref name="editorInput"/>.
        /// </summary>
        /// <param name="editorInput">Editor input</param>
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
            DispatchPropertyChanged(Name.Of(() => EditorText));
            EditorTabText = input.Name;
            IsDirty = false;
        }

        /// <summary>
        /// Returns the currently used rich text box to display the text.
        /// </summary>
        protected IRichTextBox RichTextBox { get; private set; }

        /// <summary>
        /// Tells the instance to create its content using the given widget factory.
        /// </summary>
        /// <param name="gridComposite">Panel to place the content widgets of the editor</param>
        public override void CreateContents(IGridComposite gridComposite) {
            gridComposite.GridColumns = 1;
            gridComposite.GridRows = 1;
            IRichTextBox richTextBox = WidgetFactory.CreateInstance<IRichTextBox>(gridComposite);
            gridComposite.AddWidget(richTextBox);
            RichTextBox = richTextBox;

            DataBindingOperator.Apply(richTextBox, PRichTextBox.TextProperty, new DataBinding(this, Name.Of(() => EditorText)));
        }
    }
}