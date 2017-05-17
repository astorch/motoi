using System;
using System.Drawing;
using System.IO;
using System.Xml;
using FastColoredTextBoxNS;
using motoi.platform.ui;
using motoi.platform.ui.bindings;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="IRichTextBox"/> by extending the <see cref="FastColoredTextBox"/>.
    /// </summary>
    public class RichTextBox : FastColoredTextBox, IRichTextBox {

        /// <inheritdoc />
        public RichTextBox() {
            InitializeComponent();
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

        /// <inheritdoc />
        string ITextBox.Text {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.TextProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.TextProperty, value);
                Text = value;
            }
        }

        /// <inheritdoc />
        int ITextBox.CursorIndex {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.CursorIndexProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.CursorIndexProperty, value);
                SelectionStart = value;
                DoSelectionVisible();
            }
        }

        /// <inheritdoc />
        bool ITextBox.ReadOnly {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.ReadOnlyProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.ReadOnlyProperty, value);
                ReadOnly = value;
            }
        }

        /// <inheritdoc />
        StyleDefinition IRichTextBox.StyleDefinition {
            get { return PRichTextBox.GetModelValue(this, PRichTextBox.StyleDefinitionProperty); }
            set {
                PRichTextBox.SetModelValue(this, PRichTextBox.StyleDefinitionProperty, value);
                OnStyleDefinitionChanged(value);
            }
        }

        #endregion

        /// <summary>
        /// Is invoked when the style definition has been changed.
        /// </summary>
        /// <param name="styleDefinition">New style definition</param>
        private void OnStyleDefinitionChanged(StyleDefinition styleDefinition) {
            if (styleDefinition == null) return;

            string styleName = styleDefinition.StyleName;
            byte[] styleData = styleDefinition.StyleData;
            if (styleData == null || styleData.Length == 0) return;

            // Convert style data to XML document
            XmlDocument xmlDocument = new XmlDocument();
            using (BufferedStream bufferedStream = new BufferedStream(new MemoryStream(styleData))) {
                xmlDocument.Load(bufferedStream);
            }

            // Register and apply style
            SyntaxHighlighter.AddXmlDescription(styleName, xmlDocument);
            DescriptionFile = styleName;

            // Force refresh
            SyntaxHighlighter.HighlightSyntax(styleName, Range);
        }

        /// <inheritdoc />
        public override void OnSelectionChanged() {
            PRichTextBox.SetModelValue(this, PRichTextBox.CursorIndexProperty, SelectionStart, EBindingSourceUpdateReason.PropertyChanged);
            base.OnSelectionChanged();
        }

        /// <inheritdoc />
        protected override void OnTextChanged(TextChangedEventArgs args) {
            PRichTextBox.SetModelValue(this, PRichTextBox.TextProperty, Text, EBindingSourceUpdateReason.PropertyChanged);
            base.OnTextChanged(args);
        }

        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e) {
            PRichTextBox.SetModelValue(this, PRichTextBox.TextProperty, Text, EBindingSourceUpdateReason.LostFocus);
            base.OnLostFocus(e);
        }

        /// <summary> Performs an initialization of the used components. </summary>
        private void InitializeComponent() {
            Language = Language.Custom;
            Font = new Font("Consolas", 9.5f, FontStyle.Regular); //new Font(FontFamily.GenericMonospace, 9.5f, FontStyle.Regular);
            LeftPadding = 15;
            CurrentLineColor = Color.Gold;
        }
    }
}