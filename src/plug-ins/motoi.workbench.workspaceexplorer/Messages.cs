using motoi.platform.nls;

namespace motoi.workbench.workspaceexplorer {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 1591

    /// <summary> Provides the messages of the plug-in. </summary>
    public class Messages : NLS<Messages> {
        static Messages() {
            LoadMessages();
        }

        public static readonly string Extensions_WorkspaceExploreryView_Label;
    }
}