using xcite.csharp;

namespace motoi.platform.ui.shells {
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