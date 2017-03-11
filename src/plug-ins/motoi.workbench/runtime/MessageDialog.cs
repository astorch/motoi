using motoi.platform.ui;
using motoi.platform.ui.shells;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides convenience methods to easily show message dialogs.
    /// </summary>
    public static class MessageDialog {
        /// <summary>
        /// Shows a message dialog of the given <paramref name="dialogType"/> with the given arguments.
        /// </summary>
        /// <param name="dialogType">Type of the dialog</param>
        /// <param name="title">Dialog title</param>
        /// <param name="header">Dialog header</param>
        /// <param name="text">Dialog text</param>
        /// <param name="resultSet">Set of possible dialog results</param>
        /// <returns>Selected dialog result</returns>
        public static EMessageDialogResult Show(EMessageDialogType dialogType, string title, string header, string text, EMessageDialogResult[] resultSet) {
            IMessageDialogWindow msgDlg = UIFactory.NewViewPart<IMessageDialogWindow>();
            msgDlg.DialogType = dialogType;
            msgDlg.WindowTitle = title;
            msgDlg.Header = header;
            msgDlg.Text = text;
            msgDlg.DialogResultSet = resultSet ?? new[] { EMessageDialogResult.Ok };
            
            return msgDlg.Show();
        }

        /// <summary>
        /// Shows a question message dialog with the given arguments.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="header">Dialog header</param>
        /// <param name="text">Dialog text</param>
        /// <param name="resultSet">Set of possible dialog results</param>
        /// <returns>Selected dialog result</returns>
        public static EMessageDialogResult ShowQuestion(string title, string header, string text, EMessageDialogResult[] resultSet) {
            return Show(EMessageDialogType.Question, title, header, text, resultSet);
        }

        /// <summary>
        /// Shows an error message dialog with the given arguments.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="header">Dialog header</param>
        /// <param name="text">Dialog text</param>
        public static void ShowError(string title, string header, string text) {
            Show(EMessageDialogType.Error, title, header, text, new[] {EMessageDialogResult.Ok});
        }
    }
}