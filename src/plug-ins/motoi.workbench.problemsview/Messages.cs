﻿using motoi.platform.nls;

namespace motoi.workbench.problemsview {
    // ReSharper disable UnassignedReadonlyField
    // ReSharper disable InconsistentNaming
    #pragma warning disable 1591

    /// <summary> Provides the messages of the plug-in. </summary>
    public class Messages : NLS<Messages> {
        static Messages() {
            LoadMessages();
        }

        public static readonly string ProblemsDataView_Name;
        public static readonly string ProblemsDataView_ColumnDescription_Name;
        public static readonly string ProblemsDataView_ColumnFile_Name;
        public static readonly string ProblemsDataView_ColumnLine_Name;
        public static readonly string ProblemsDataView_ColumnColumn_Name;

        public static readonly string ProblemsViewItemType_Error;
        public static readonly string ProblemsViewItemType_Warning;
        public static readonly string ProblemsViewItemType_Info;
    }
}