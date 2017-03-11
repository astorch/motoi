using Xcite.Csharp.lang;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines a message dialog.
    /// </summary>
    public interface IMessageDialogWindow : IDialogWindow {
        /// <summary>
        /// Returns the dialog type or does set it.
        /// </summary>
        EMessageDialogType DialogType { get; set; }

        /// <summary>
        /// Returns the set of dialog results that are available to the user.
        /// </summary>
        EMessageDialogResult[] DialogResultSet { get; set; }

        /// <summary>
        /// Returns the dialog header or does set it.
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// Returns the dialog text or does set it.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Creates the dialog and makes it visible to the user.
        /// </summary>
        /// <returns>Dialog close result</returns>
        new EMessageDialogResult Show();
    }

    /// <summary>
    /// Defines kinds of message dialog results.
    /// </summary>
    public class EMessageDialogResult : XEnum<EMessageDialogResult> {
        /// <summary>
        /// Indicates the ok result.
        /// </summary>
        public static readonly EMessageDialogResult Ok = new EMessageDialogResult("Ok");

        /// <summary>
        /// Indicates the cancel result.
        /// </summary>
        public static readonly EMessageDialogResult Cancel = new EMessageDialogResult("Cancel");

        /// <summary>
        /// Indicates the yes result.
        /// </summary>
        public static readonly EMessageDialogResult Yes = new EMessageDialogResult("Yes");

        /// <summary>
        /// Indicates the no result.
        /// </summary>
        public static readonly EMessageDialogResult No = new EMessageDialogResult("No");

        /// <summary>
        /// Indicates a none result.
        /// </summary>
        public static readonly EMessageDialogResult None = new EMessageDialogResult("None");

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="uniqueReference">Unique reference to identify this instance</param>
        private EMessageDialogResult(string uniqueReference) : base(uniqueReference) {
            // Currently nothing to do here
        }
    }

    /// <summary>
    /// Defines kinds of message dialog types.
    /// </summary>
    public class EMessageDialogType : XEnum<EMessageDialogType> {
        /// <summary>
        /// Indicates an info dialog.
        /// </summary>
        public static readonly EMessageDialogType Info = new EMessageDialogType("Info");

        /// <summary>
        /// Indicates a warning dialog.
        /// </summary>
        public static readonly EMessageDialogType Warning = new EMessageDialogType("Warning");

        /// <summary>
        /// Indicates an error dialog.
        /// </summary>
        public static readonly EMessageDialogType Error = new EMessageDialogType("Error");

        /// <summary>
        /// Indicates a hint dialog.
        /// </summary>
        public static readonly EMessageDialogType Hint = new EMessageDialogType("Hint");

        /// <summary>
        /// Indicates a question dialog.
        /// </summary>
        public static readonly EMessageDialogType Question = new EMessageDialogType("Question");

        /// <summary>
        /// Indicates a custom dialog.
        /// </summary>
        public static readonly EMessageDialogType Custom = new EMessageDialogType("Custom");

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="uniqueReference">Unique reference to identify this instance</param>
        private EMessageDialogType(string uniqueReference) : base(uniqueReference) {
            // Nothing to do here
        }
    }
}