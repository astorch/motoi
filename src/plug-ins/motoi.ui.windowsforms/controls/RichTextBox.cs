using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using FastColoredTextBoxNS;
using motoi.platform.ui;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;
using Xcite.Collections;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="IRichTextBox"/> by extending the <see cref="FastColoredTextBox"/>.
    /// </summary>
    public class RichTextBox : FastColoredTextBox, IRichTextBox {
        private WavyLineStyle iErrorLineStyle;
        private WavyLineStyle iWarningLineStyle;
        private WavyLineStyle iInfoLineStyle;
        private Bitmap iErrorBitmap;
        private Bitmap iWarningBitmap;
        private Bitmap iInfoBitmap;

        private readonly Dictionary<int, IRichTextSyntaxHint> iSyntaxHints = new Dictionary<int, IRichTextSyntaxHint>(5);

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RichTextBox() {
            InitializeComponent();
        }

        /// <summary>
        /// Performs an initialization of the used components.
        /// </summary>
        private void InitializeComponent() {
            Language = Language.Custom;
            Font = new Font("Consolas", 9.5f, FontStyle.Regular); //new Font(FontFamily.GenericMonospace, 9.5f, FontStyle.Regular);
            LeftPadding = 15;
            CurrentLineColor = Color.Gold;
        }

        #region IRichTextBox

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.VisibilityProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            }
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.EnabledProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <summary>
        /// Returns the text content of the text box or does set it.
        /// </summary>
        string ITextBox.Text {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.TextProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.TextProperty, value);
                Text = value;
            }
        }

        /// <summary>
        /// Returns the curent cursor index of the text box or does set it.
        /// </summary>
        int ITextBox.CursorIndex {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.CursorIndexProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.CursorIndexProperty, value);
                SelectionStart = value;
                DoSelectionVisible();
            }
        }

        /// <summary>
        /// Returns the currently used Rich Text Model or does set it.
        /// </summary>
        IRichTextModel IRichTextBox.RichTextModel {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.RichTextModelProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.RichTextModelProperty, value);
                OnRichTextModelChanged(value);
            }
        }

        /// <summary>
        /// Returns the currently used Rich Text Syntax validator or does set it.
        /// </summary>
        IRichTextSyntaxValidator IRichTextBox.RichTextSyntaxValidator {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.RichTextSyntaxValidatorProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.RichTextSyntaxValidatorProperty, value);
                OnRichTextSyntaxValidatorChanged(value);
            }
        }

        /// <summary>
        /// Returns TRUE if the text box is read only and therefore the text cannot be edited.
        /// </summary>
        bool ITextBox.ReadOnly {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.ReadOnlyProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.ReadOnlyProperty, value);
                ReadOnly = value;
            }
        }

        #endregion

        /// <summary> Fires SelectionChanged event </summary>
        public override void OnSelectionChanged() {
            PTextBox.SetModelValue(this, PRichTextBox.CursorIndexProperty, SelectionStart, EBindingSourceUpdateReason.PropertyChanged);
            base.OnSelectionChanged();
        }

        /// <summary> Fires TextChanged event </summary>
        protected override void OnTextChanged(TextChangedEventArgs args) {
            PTextBox.SetModelValue(this, PRichTextBox.TextProperty, Text, EBindingSourceUpdateReason.PropertyChanged);
            base.OnTextChanged(args);
        }

        /// <summary> Fires SelectedChangedDelayed event </summary>
        /// <param name="changedRange"></param>
        public override void OnTextChangedDelayed(Range changedRange) {
            ProcessSyntaxValidation(changedRange);
            base.OnTextChangedDelayed(changedRange); 
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLostFocus(EventArgs e) {
            PTextBox.SetModelValue(this, PRichTextBox.TextProperty, Text, EBindingSourceUpdateReason.LostFocus);
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Is invoked when a line is painted.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnPaintLine(PaintLineEventArgs e) {
            base.OnPaintLine(e);
            if (iSyntaxHints.Count == 0) return;

            int lineIndex = e.LineIndex;
            IRichTextSyntaxHint hint;
            if (!iSyntaxHints.TryGetValue(lineIndex, out hint)) return;

            Image image = null;
            if (hint.Type == ERichTextSyntaxHintType.Error)
                image = iErrorBitmap;

            if (hint.Type == ERichTextSyntaxHintType.Info)
                image = iInfoBitmap;

            if (hint.Type == ERichTextSyntaxHintType.Warning)
                image = iWarningBitmap;

            if (image == null) return;

            e.Graphics.DrawImage(image, 0, e.LineRect.Top + 4, 15, 15);
        }

        /// <summary>
        /// Tells the instance that the <see cref="IRichTextBox.RichTextModel"/> property has been changed.
        /// </summary>
        /// <param name="richTextModel">New model value</param>
        protected virtual void OnRichTextModelChanged(IRichTextModel richTextModel) {
            if (richTextModel == null) return;

            byte[] xmlStyleData = richTextModel.XmlStyleData;
            if (xmlStyleData != null && xmlStyleData.Length != 0) {
                XmlDocument xmlDocument = new XmlDocument();
                using (BufferedStream bufferedStream = new BufferedStream(new MemoryStream(xmlStyleData))) {
                    xmlDocument.Load(bufferedStream);
                }
                
                string styleName = richTextModel.StyleName;
                SyntaxHighlighter.AddXmlDescription(styleName, xmlDocument);
                DescriptionFile = styleName;

                // Force refresh
                SyntaxHighlighter.HighlightSyntax(styleName, Range);
            }

            IEnumerable<string> autoCompleteItems = richTextModel.AutoCompleteItems;
            if (autoCompleteItems != null) {
                // Set up auto complete menu
                AutocompleteMenu autocompleteMenu = new AutocompleteMenu(this);
                autocompleteMenu.SearchPattern = richTextModel.AutoCompleteSearchPattern;
                autocompleteMenu.MinFragmentLength = richTextModel.AutoCompleteMinFragmentLength;
                autocompleteMenu.AppearInterval = richTextModel.AutoCompleteAppearInterval;
                autocompleteMenu.AllowTabKey = richTextModel.AutoCompleteAllowTab;

                // Create auto complete items
                List<AutocompleteItem> autoCompleteItemCollection = new List<AutocompleteItem>(200);
                autoCompleteItems.ForEach(item => autoCompleteItemCollection.Add(new AutocompleteItem(item)));
                autocompleteMenu.Items.SetAutocompleteItems(autoCompleteItemCollection);

                // Support observable collection
                IObservableCollection<string> observableCollection = autoCompleteItems as IObservableCollection<string>;
                if (observableCollection != null) {
                    observableCollection.AddCollectionListener(new CollectionListenerImpl(autoCompleteItemCollection));
                }

                autocompleteMenu.MinimumSize = new Size(240, 0);
                autocompleteMenu.Selected += (sender, args) => richTextModel.OnItemInserted(args.Item.Text, SelectionStart - args.Item.Text.Length); // TODO Unregister event
            }
        }

        /// <summary>
        /// Tells the instance that the <see cref="IRichTextBox.RichTextSyntaxValidator"/> property has been changed.
        /// </summary>
        /// <param name="validator">New validator</param>
        protected virtual void OnRichTextSyntaxValidatorChanged(IRichTextSyntaxValidator validator) {
            if (validator == null) return;

            // Lazy initialize resources
            if (iErrorLineStyle == null) {
                SyntaxHighlighter.AddResilientStyle(iErrorLineStyle = new WavyLineStyle(255, Color.Red));
                SyntaxHighlighter.AddResilientStyle(iWarningLineStyle = new WavyLineStyle(255, Color.Yellow));
                SyntaxHighlighter.AddResilientStyle(iInfoLineStyle = new WavyLineStyle(255, Color.Green));

                iErrorBitmap = LoadBitmap("Error-32.png");
                iWarningBitmap = LoadBitmap("Warning-32.png");
                iInfoBitmap = LoadBitmap("Info-32.png");
            }
        }

        /// <summary>
        /// Tells the instance to validate the text referenced by the given <paramref name="changedRange"/>.
        /// </summary>
        /// <param name="changedRange">Range of changed text</param>
        protected virtual void ProcessSyntaxValidation(Range changedRange) {
            IRichTextSyntaxValidator syntaxValidator = ((IRichTextBox) this).RichTextSyntaxValidator;
            if (syntaxValidator == null) return;
            
            // Clean all styles
            Range.ClearStyle(iErrorLineStyle);
            iSyntaxHints.Clear();

            // Check for syntax errors again
            string text = Range.Text;
            IEnumerable<IRichTextSyntaxHint> hints = syntaxValidator.Validate(text);
            Place start = new Place(0, 0);

            using (IEnumerator<IRichTextSyntaxHint> itr = hints.GetEnumerator()) {
                while (itr.MoveNext()) {
                    IRichTextSyntaxHint hint = itr.Current;
                    iSyntaxHints.Add(hint.BeginLine, hint);

                    Range hintRange = GetRange(new Place(hint.BeginColumn, hint.BeginLine), new Place(hint.EndColumn, hint.EndLine));
                    while (string.IsNullOrEmpty(hintRange.Text) && hintRange.Start > start) {
                        hintRange.GoLeft(true);
                    }

                    Style style = null;

                    if (hint.Type == ERichTextSyntaxHintType.Error)
                        style = iErrorLineStyle;

                    if (hint.Type == ERichTextSyntaxHintType.Warning)
                        style = iWarningLineStyle;

                    if (hint.Type == ERichTextSyntaxHintType.Info)
                        style = iInfoLineStyle;

                    if (style == null) continue;

                    hintRange.SetStyle(style);
                }
            }
        }

        /// <summary>
        /// Loads an image with the given <paramref name="imageName"/> from the current assembly. It is assumed 
        /// that the image is stored as embedded resource under the path 'resources.images'.
        /// </summary>
        /// <param name="imageName">Name of the image to load</param>
        /// <returns>Loaded bitmap</returns>
        private Bitmap LoadBitmap(string imageName) {
            Assembly assembly = GetType().Assembly;
            string assemblyName = assembly.GetName().Name;
            string rsxPath = string.Format("{0}.resources.images.{1}", assemblyName, imageName);
            using (Stream stream = assembly.GetManifestResourceStream(rsxPath)) {
                if (stream == null) throw new InvalidOperationException(string.Format("Could not open stream to '{0}'", rsxPath));
                using (BufferedStream bufferedStream = new BufferedStream(stream)) {
                    return new Bitmap(bufferedStream);
                }
            }
        }

        /// <summary>
        /// Provides an anonymous implementation of <see cref="ICollectionListener{TItem}"/>.
        /// </summary>
        class CollectionListenerImpl : ICollectionListener<string> {
            private readonly List<AutocompleteItem> iAutocompleteItems;

            /// <summary>
            /// Creates a new instance that updates the given <paramref name="autocompleteItems"/> collection.
            /// </summary>
            /// <param name="autocompleteItems">Collection to update</param>
            public CollectionListenerImpl(List<AutocompleteItem> autocompleteItems) {
                iAutocompleteItems = autocompleteItems;
            }

            /// <summary>
            /// Is invoked when the given <paramref name="item"/> has been added to the given <paramref name="itemCollection"/>.
            /// </summary>
            /// <param name="itemCollection">Modified item collection</param>
            /// <param name="item">Added item</param>
            public void OnItemAdded(IObservableCollection<string> itemCollection, string item) {
                UpdateCollection(itemCollection);
            }

            /// <summary>
            /// Is invoked when the given <paramref name="item"/> has been removed from the given <paramref name="itemCollection"/>.
            /// </summary>
            /// <param name="itemCollection">Modified item collection</param>
            /// <param name="item">Removed item</param>
            public void OnItemRemoved(IObservableCollection<string> itemCollection, string item) {
                UpdateCollection(itemCollection);
            }

            /// <summary>
            /// Is invoked when the given <paramref name="itemCollection"/> has been cleared.
            /// </summary>
            /// <param name="itemCollection">Modified item collection</param>
            public void OnCleared(IObservableCollection<string> itemCollection) {
                iAutocompleteItems.Clear();
            }

            /// <summary>
            /// Performs an update of the autocomplete item collection.
            /// </summary>
            /// <param name="itemCollection"></param>
            private void UpdateCollection(IObservableCollection<string> itemCollection) {
                iAutocompleteItems.Clear();
                itemCollection.OrderByDescending(_ => _).ForEach(_ => iAutocompleteItems.Add(new AutocompleteItem(_)));
            }
        }
    }
}