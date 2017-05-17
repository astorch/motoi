using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using FastColoredTextBoxNS;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="IContentAssistTextBox"/> by extending the <see cref="RichTextBox"/>.
    /// </summary>
    public class ContentAssistTextBox : RichTextBox, IContentAssistTextBox {
        private WavyLineStyle iErrorLineStyle;
        private WavyLineStyle iWarningLineStyle;
        private WavyLineStyle iInfoLineStyle;
        private Bitmap iErrorBitmap;
        private Bitmap iWarningBitmap;
        private Bitmap iInfoBitmap;

        private readonly Dictionary<int, IContentSyntaxValidationHint> iSyntaxHints = new Dictionary<int, IContentSyntaxValidationHint>(5);

        #region IContentAssistTextBox

        /// <inheritdoc />
        IContentAssistModel IContentAssistTextBox.ContentAssistModel
        {
            get { return PContentAssistTextBox.GetModelValue(this, PContentAssistTextBox.ContentAssistModelProperty); }
            set
            {
                PContentAssistTextBox.SetModelValue(this, PContentAssistTextBox.ContentAssistModelProperty, value);
                OnContentAssistModelChanged(value);
            }
        }

        /// <inheritdoc />
        IContentSyntaxValidator IContentAssistTextBox.ContentSyntaxValidator
        {
            get { return PContentAssistTextBox.GetModelValue(this, PContentAssistTextBox.ContentSyntaxValidatorProperty); }
            set
            {
                PContentAssistTextBox.SetModelValue(this, PContentAssistTextBox.ContentSyntaxValidatorProperty, value);
                OnContentSyntaxValidatorChanged(value);
            }
        }

        #endregion

        /// <summary> Fires SelectedChangedDelayed event </summary>
        /// <param name="changedRange"></param>
        public override void OnTextChangedDelayed(Range changedRange) {
            ProcessSyntaxValidation(changedRange);
            base.OnTextChangedDelayed(changedRange);
        }

        /// <summary>
        /// Is invoked when a line is painted.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnPaintLine(PaintLineEventArgs e) {
            base.OnPaintLine(e);
            if (iSyntaxHints.Count == 0) return;

            int lineIndex = e.LineIndex;
            IContentSyntaxValidationHint hint;
            if (!iSyntaxHints.TryGetValue(lineIndex, out hint)) return;

            Image image = null;
            if (hint.Type == EContentValidationSyntaxHintType.Error)
                image = iErrorBitmap;

            if (hint.Type == EContentValidationSyntaxHintType.Info)
                image = iInfoBitmap;

            if (hint.Type == EContentValidationSyntaxHintType.Warning)
                image = iWarningBitmap;

            if (image == null) return;

            e.Graphics.DrawImage(image, 0, e.LineRect.Top + 4, 15, 15);
        }

        /// <summary>
        /// Tells the instance that the <see cref="IContentAssistTextBox.ContentAssistModel"/> property has been changed.
        /// </summary>
        /// <param name="contentAssistModel">New model value</param>
        protected virtual void OnContentAssistModelChanged(IContentAssistModel contentAssistModel) {
            if (contentAssistModel == null) return;

            // No provider -> no box
            IContentAssistItemProvider itemProvider = contentAssistModel.ItemProvider;
            if (itemProvider == null) return;

            // Set up auto complete menu
            AutocompleteMenu autocompleteMenu = new AutocompleteMenu(this);
            autocompleteMenu.SearchPattern = contentAssistModel.SearchPattern;
            autocompleteMenu.MinFragmentLength = contentAssistModel.MinFragmentLength;
            autocompleteMenu.AppearInterval = contentAssistModel.AppearInterval;
            autocompleteMenu.AllowTabKey = contentAssistModel.AllowTab;
            autocompleteMenu.AlwaysShowTooltip = contentAssistModel.AlwaysShowInfotip;
            autocompleteMenu.ToolTipDuration = contentAssistModel.InfotipDuration;
            
            autocompleteMenu.Items.SetAutocompleteItems(new ContentAssistItemProjection(autocompleteMenu, itemProvider));
            autocompleteMenu.Selected += (sender, args) => contentAssistModel.OnItemInserted((ContentAssistItem) args.Item.Tag, SelectionStart - args.Item.Text.Length); // TODO Unregister event
        }

        /// <summary>
        /// Tells the instance that the <see cref="IContentAssistTextBox.ContentSyntaxValidator"/> property has been changed.
        /// </summary>
        /// <param name="validator">New validator</param>
        protected virtual void OnContentSyntaxValidatorChanged(IContentSyntaxValidator validator) {
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
            IContentSyntaxValidator syntaxValidator = ((IContentAssistTextBox) this).ContentSyntaxValidator;
            if (syntaxValidator == null) return;
            
            // Clean all styles
            Range.ClearStyle(iErrorLineStyle);
            iSyntaxHints.Clear();

            // Check for syntax errors again
            string text = Range.Text;
            IEnumerable<IContentSyntaxValidationHint> hints = syntaxValidator.Validate(text);
            Place start = new Place(0, 0);

            using (IEnumerator<IContentSyntaxValidationHint> itr = hints.GetEnumerator()) {
                while (itr.MoveNext()) {
                    IContentSyntaxValidationHint hint = itr.Current;
                    iSyntaxHints.Add(hint.BeginLine, hint);

                    Range hintRange = GetRange(new Place(hint.BeginColumn, hint.BeginLine), new Place(hint.EndColumn, hint.EndLine));
                    while (string.IsNullOrEmpty(hintRange.Text) && hintRange.Start > start) {
                        hintRange.GoLeft(true);
                    }

                    Style style = null;

                    if (hint.Type == EContentValidationSyntaxHintType.Error)
                        style = iErrorLineStyle;

                    if (hint.Type == EContentValidationSyntaxHintType.Warning)
                        style = iWarningLineStyle;

                    if (hint.Type == EContentValidationSyntaxHintType.Info)
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
        /// Implements an adapter that maps each <see cref="ContentAssistItem"/> provided 
        /// by a <see cref="IContentAssistItemProvider"/> to an <see cref="AutocompleteItem"/> 
        /// used by the underlying <see cref="FastColoredTextBox"/>.
        /// </summary>
        class ContentAssistItemProjection : IEnumerable<AutocompleteItem> {
            private readonly AutocompleteMenu fAutocompleteMenu;
            private readonly IContentAssistItemProvider fContentAssistItemProvider;

            /// <summary>
            /// Creates a new instance that projects each <see cref="ContentAssistItem"/> 
            /// provided by the given <paramref name="contentAssistItemProvider"/> as 
            /// <see cref="AutocompleteItem"/> into the given <paramref name="autocompleteMenu"/>.
            /// </summary>
            /// <param name="autocompleteMenu">Autocomplete menu</param>
            /// <param name="contentAssistItemProvider">Item provider</param>
            /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
            public ContentAssistItemProjection(AutocompleteMenu autocompleteMenu,
                IContentAssistItemProvider contentAssistItemProvider) {
                fAutocompleteMenu = autocompleteMenu ?? throw new ArgumentNullException(nameof(autocompleteMenu));
                fContentAssistItemProvider = contentAssistItemProvider ?? throw new ArgumentNullException(nameof(contentAssistItemProvider));
            }

            /// <inheritdoc />
            public IEnumerator<AutocompleteItem> GetEnumerator() {
                string text = fAutocompleteMenu.Fragment.Text;
                using (IEnumerator<ContentAssistItem> itr =
                    fContentAssistItemProvider.ProvideItems(text).GetEnumerator()) {
                    while (itr.MoveNext()) {
                        ContentAssistItem item = itr.Current;
                        string itemText = item.Text;

                        // Calculate minimum size of the menu
                        // Todo Check if this is an performance issue
                        using (Graphics gr = fAutocompleteMenu.CreateGraphics()) {
                            SizeF size = gr.MeasureString(itemText, fAutocompleteMenu.Font);
                            if (size.Width > fAutocompleteMenu.MinimumSize.Width)
                                fAutocompleteMenu.MinimumSize = new Size((int) size.Width + 10, 0);
                        }

                        // Note: text     = text to replace
                        //       menuText = text as shown in the menu list
                        yield return new AutocompleteItem(itemText, -1, item.ListText ?? itemText, item.InfotipTitle, item.InfotipText) {
                            Tag = item
                        };
                    }
                }
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
    }
}