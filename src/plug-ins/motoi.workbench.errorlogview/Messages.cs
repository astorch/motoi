using motoi.platform.nls;

namespace motoi.workbench.errorlogview {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 649

    /// <summary> Provides the messages of the plug-in. </summary>
    internal class Messages : NLS<Messages> {
        static Messages() {
            LoadMessages();
        }

        public static readonly string Extensions_ErrorLogView_Label;

        public static readonly string ErrorLogDataView_Name;

        public static readonly string ErrorLogDataView_ColumnMessage_Name;
        public static readonly string ErrorLogDataView_ColumnPlugin_Name;
        public static readonly string ErrorLogDataView_ColumnDate_Name;
    }
}
