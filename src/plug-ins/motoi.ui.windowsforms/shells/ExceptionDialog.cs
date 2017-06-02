using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using motoi.platform.ui.actions;
using motoi.platform.ui.shells;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="IExceptionDialog"/>.
    /// </summary>
    public class ExceptionDialog : MessageDialog, IExceptionDialog {
        private static readonly string ForwardLabelText = "Details " + '\u00BB';  // »
        private static readonly string BackwardLabelText = "Details " + '\u00AB'; // «

        private TableLayoutPanel fParentContentPanel;
        private TableLayoutPanel fDetailsPanel;
        private ListBox fDetailsListBox;
        private RichTextBox fDetailsTextBox;
        private Size fCollapsedClientSize;
        private IButton fDetailsButton;
        private bool fShowDetails;

        /// <inheritdoc />
        public ExceptionDialog() {
            InitializeComponents();
        }

        #region IExceptionDialog

        /// <inheritdoc />
        Exception IExceptionDialog.Exception {
            get { return PExceptionDialog.GetModelValue(this, PExceptionDialog.ExceptionProperty); }
            set {
                PExceptionDialog.SetModelValue(this, PExceptionDialog.ExceptionProperty, value);
                OnExceptionChanged(value);
            }
        }

        #endregion

        /// <summary>
        /// Is invoked when <see cref="IExceptionDialog.Exception"/> has been changed.
        /// </summary>
        /// <param name="value">New value</param>
        private void OnExceptionChanged(Exception value) {
            if (value == null) return;
            ((IExceptionDialog)this).Text = value.Message;

            List<ExceptionInfo> exceptionStack = new List<ExceptionInfo>(5);
            exceptionStack.Add(ExceptionInfo.From(value));

            Exception innerException = value.InnerException;
            while (innerException != null) {
                exceptionStack.Add(ExceptionInfo.From(innerException));
                innerException = innerException.InnerException;
            }

            IEnumerable<string> messages = exceptionStack.Select(ex => ex.Message);
            string aggregatedMessage = string.Join(Environment.NewLine, messages);

            IEnumerable<string> stacktrace = exceptionStack.Select(ex => ex.Stacktrace);
            string aggregatedStacktrace = string.Join(Environment.NewLine, stacktrace);

            ExceptionInfo allMessageInfo = new ExceptionInfo("All messages", aggregatedMessage, aggregatedStacktrace,
                value.TargetSite.Name, value.Source);
            exceptionStack.Insert(0, allMessageInfo);
            fDetailsListBox.DataSource = exceptionStack;
            fDetailsListBox.DisplayMember = "Type";

            fDetailsListBox.SelectedValueChanged += (sender, args) => {
                ExceptionInfo exInfo = (ExceptionInfo) ((ListBox) sender).SelectedItem;
                string rtfText = exInfo.ToRtfString();
                fDetailsTextBox.Rtf = rtfText;
            };

            fDetailsListBox.SelectionMode = SelectionMode.One;
            fDetailsListBox.SelectedItem = exceptionStack[0];
        }

        /// <inheritdoc />
        protected override void AddMessageDialogButtons(EMessageDialogResult[] resultSet) {
            base.AddMessageDialogButtons(resultSet);
            fDetailsButton = ((IDialogWindow) this).AddButton(ForwardLabelText, new ActionHandlerDelegate(OnDetailsButtonClicked));
        }


        /// <summary> Is invoked when the button to show or hide the details has been clicked. </summary>
        private void OnDetailsButtonClicked() {
            // Toggle flag
            fShowDetails = !fShowDetails;

            SuspendLayout();

            // Expand or collapse dialog window
            if (fShowDetails) {
                fDetailsButton.Text = BackwardLabelText;

                fParentContentPanel.RowCount += 1;
                fParentContentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

                fParentContentPanel.Controls.Add(fDetailsPanel, 0, 2);
                fParentContentPanel.SetColumnSpan(fDetailsPanel, 2);

                fCollapsedClientSize = ClientSize;
                ClientSize = new Size(1200, 600);
            } else {
                fDetailsButton.Text = ForwardLabelText;

                fParentContentPanel.RowCount -= 1;
                fParentContentPanel.RowStyles.RemoveAt(fParentContentPanel.RowStyles.Count - 1);

                int index = fParentContentPanel.Controls.GetChildIndex(fDetailsPanel);
                fParentContentPanel.Controls.RemoveAt(index);

                ClientSize = fCollapsedClientSize;
            }
            
            ResumeLayout(true);
        }

        /// <inheritdoc />
        protected override Control CreateContentControl(Panel contentContainer) {
            CreateControls();

            Control ctrl = base.CreateContentControl(contentContainer);
            fParentContentPanel = (TableLayoutPanel) contentContainer.Controls[contentContainer.Controls.Count - 1];
            
            return ctrl;
        }

        /// <inheritdoc />
        protected override void OnWindowClosed() {
            fDetailsPanel.Controls.Clear();
            fDetailsPanel = null;
            fDetailsListBox = null;
            fDetailsTextBox = null;

            base.OnWindowClosed();
        }

        /// <summary> Creates the controls used by this component. </summary>
        private void CreateControls() {
            fDetailsPanel = new TableLayoutPanel {ColumnCount = 2, RowCount = 1, Dock = DockStyle.Fill, AutoSize = true};

            fDetailsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            fDetailsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            fDetailsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80f));

            fDetailsListBox = new ListBox {Dock =  DockStyle.Fill, AutoSize = true, IntegralHeight = false};
            fDetailsPanel.Controls.Add(fDetailsListBox, 0, 0);
            
            fDetailsTextBox = new RichTextBox {
                ReadOnly = true,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSize = true
            };
            fDetailsPanel.Controls.Add(fDetailsTextBox, 1, 0);
        }

        /// <summary> Performs an initialization of the used components. </summary>
        private void InitializeComponents() {
            AutoSize = true;
        }

        /// <summary> Provides information about an exception. </summary>
        class ExceptionInfo {
            /// <summary>
            /// Creates a new instance with the given arguments.
            /// </summary>
            /// <param name="type">Exception type</param>
            /// <param name="message">Exception message</param>
            /// <param name="stacktrace">Exception stacktrace</param>
            /// <param name="targetSite">Exception target site</param>
            /// <param name="source">Exception source</param>
            public ExceptionInfo(string type, string message, string stacktrace, string targetSite, string source) {
                Type = type;
                Message = message;
                Stacktrace = stacktrace;
                TargetSite = targetSite;
                Source = source;
            }

            /// <summary> Returns the exception type. </summary>
            public string Type { get; private set; }

            /// <summary> Returns the exception message. </summary>
            public string Message { get; private set; }

            /// <summary> Returns the stacktrace of the exception. </summary>
            public string Stacktrace { get; private set; }

            /// <summary> Returns the target site of the exception. </summary>
            public string TargetSite{ get; private set; }

            /// <summary> Returns the source of the exception. </summary>
            public string Source { get; private set; }

            /// <summary>
            /// Creates an instance of <see cref="ExceptionInfo"/> based on the informationen 
            /// of the given <paramref name="ex"/>.
            /// </summary>
            /// <param name="ex">Exception that provides information</param>
            /// <returns>New instance of <see cref="ExceptionInfo"/></returns>
            public static ExceptionInfo From(Exception ex) {
                return new ExceptionInfo(ex.GetType().FullName, ex.Message,
                    ex.StackTrace, (ex.TargetSite != null ? ex.TargetSite.Name : string.Empty),
                    ex.Source);
            }

            /// <summary>
            /// Returns a RTF formatted string with all exception information.
            /// </summary>
            /// <returns>RTF formatted string with exception information</returns>
            public string ToRtfString() {
                return new StringBuilder()
                    .Append(@"{\rtf1\pc ")
                    .Append($@"\b {Type}\b0 \par \par ")
                    .Append(@"\b Message:\b0 \par ")
                    .Append(Message.Replace("\r\n", @" \line ")).Append(@" \par \par ")
                    .Append(@"\b Stacktrace:\b0 \par ")
                    .Append(Stacktrace.Replace("\r\n", @" \line ")).Append(@" \par \par ")
                    .Append(@"\b Target Site:\b0 \par")
                    .Append(TargetSite).Append(@" \par \par ")
                    .Append(@"\b Source:\b0 \par")
                    .Append(Source)
                    .ToString();
            }
        }
    }
}