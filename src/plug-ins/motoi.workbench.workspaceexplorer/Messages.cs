using motoi.platform.nls;

namespace motoi.workbench.workspaceexplorer {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 649

    /// <summary> Provides the messages of the plug-in. </summary>
    internal class Messages : NLS<Messages> {
        static Messages() {
            LoadMessages();
        }

        public static readonly string Extensions_WorkspaceExploreryView_Label;

        public static readonly string WorkspaceDataView_Name;
    }
}