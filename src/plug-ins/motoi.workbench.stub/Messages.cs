using motoi.platform.nls;

namespace motoi.workbench.stub {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 1591

    /// <summary> Provides the messages of the plug-in. </summary>
    public class Messages : NLS<Messages> {

        static Messages() {
            LoadMessages();
        }

        public static readonly string Extensions_Menu_File_Name;
        public static readonly string Extensions_Menu_File_New;
        public static readonly string Extensions_Menu_File_SaveAs;
        public static readonly string Extensions_Menu_File_Save;
        public static readonly string Extensions_Menu_File_Exit;

        public static readonly string Extensions_Menu_Window_Name;
        public static readonly string Extensions_Menu_Window_ShowViews;

        public static readonly string NewWizard_Title;
        public static readonly string NewWizardOpeningPage_Title;
        public static readonly string NewWizardOpeningPage_Description;

        public static readonly string WindowMenuShowViewsMenuHandler_DialogTitle;
        public static readonly string WindowMenuShowViewsMenuHandler_DialogDescription;
        public static readonly string WindowMenuShowViewsMenuHandler_DialogButtonOk;
        public static readonly string WindowMenuShowViewsMenuHandler_DialogButtonCancel;

    }
}