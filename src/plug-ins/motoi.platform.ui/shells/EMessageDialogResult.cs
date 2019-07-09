using xcite.csharp;

namespace motoi.platform.ui.shells {
    /// <summary> Defines kinds of message dialog results. </summary>
    public class EMessageDialogResult : XEnum<EMessageDialogResult> {
        /// <summary> Indicates the ok result. </summary>
        public static readonly EMessageDialogResult Ok = new EMessageDialogResult("Ok");

        /// <summary> Indicates the cancel result. </summary>
        public static readonly EMessageDialogResult Cancel = new EMessageDialogResult("Cancel");

        /// <summary> Indicates the yes result. </summary>
        public static readonly EMessageDialogResult Yes = new EMessageDialogResult("Yes");

        /// <summary> Indicates the no result. </summary>
        public static readonly EMessageDialogResult No = new EMessageDialogResult("No");

        /// <summary> Indicates a none result. </summary>
        public static readonly EMessageDialogResult None = new EMessageDialogResult("None");

        /// <summary> Indicates a close result. </summary>
        public static readonly EMessageDialogResult Close = new EMessageDialogResult("Close");

        /// <summary> Protected constructor. </summary>
        /// <param name="uniqueReference">Unique reference to identify this instance</param>
        private EMessageDialogResult(string uniqueReference) : base(uniqueReference) {
            // Currently nothing to do here
        }
    }
}