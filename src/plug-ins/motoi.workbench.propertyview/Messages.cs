using motoi.platform.nls;

namespace motoi.workbench.propertyview {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 1591

    /// <summary> Provides the messages of the plug-in. </summary>
    public class Messages : NLS<Messages> {
        static Messages() {
            LoadMessages();
        }

        public static readonly string Extensions_PropertyView_Label;

        public static readonly string PropertyDataView_Name;
    }
}