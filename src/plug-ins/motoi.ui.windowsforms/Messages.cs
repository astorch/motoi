using motoi.platform.nls;

namespace motoi.ui.windowsforms {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 649

    /// <summary> Provides the messages of the plug-in. </summary>
    internal class Messages : NLS<Messages> {
        static Messages() {
            LoadMessages();
        }

        public static readonly string MessageDialogWindow_Button_Ok;
        public static readonly string MessageDialogWindow_Button_Cancel;
        public static readonly string MessageDialogWindow_Button_No;
        public static readonly string MessageDialogWindow_Button_Yes;
    }
}