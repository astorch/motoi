using motoi.platform.nls;

namespace motoi.workbench {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 1591

    /// <summary> Provides the messages of the plug-in. </summary>
    public class Messages : NLS<Messages> {
        static Messages() {
            LoadMessages();
        }

        public static readonly string AbstractPerspective_CanCloseEditor_Dialog_Title;
        public static readonly string AbstractPerspective_CanCloseEditor_Dialog_Header;
        public static readonly string AbstractPerspective_CanCloseEditor_Dialog_Text;

        public static readonly string AbstractWizard_Button_Cancel;
        public static readonly string AbstractWizard_Button_Finish;
        public static readonly string AbstractWizard_Button_Next;
        public static readonly string AbstractWizard_Button_Back;
    }
}