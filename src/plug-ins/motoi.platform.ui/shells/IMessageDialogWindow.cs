﻿using motoi.platform.ui.bindings;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines a message dialog.
    /// </summary>
    public interface IMessageDialogWindow : IDialogWindow {
        /// <summary>
        /// Returns the dialog type or does set it.
        /// </summary>
        EMessageDialogType DialogType { get; set; }

        /// <summary>
        /// Returns the set of dialog results that are available to the user.
        /// </summary>
        EMessageDialogResult[] DialogResultSet { get; set; }

        /// <summary>
        /// Returns the dialog header or does set it.
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// Returns the dialog text or does set it.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Creates the dialog and makes it visible to the user.
        /// </summary>
        /// <returns>Dialog close result</returns>
        new EMessageDialogResult Show();
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IMessageDialogWindow"/> that is used by data binding operations.
    /// </summary>
    public class PMessageDialogWindow : PDialogWindowControl<IMessageDialogWindow> {
        /// <summary> Header property meta data </summary>
        public static readonly IBindableProperty<string> HeaderProperty = CreatePropertyInfo(_ => _.Header, string.Empty);

        /// <summary> Text property meta data </summary>
        public static readonly IBindableProperty<string> TextProperty = CreatePropertyInfo(_ => _.Text, string.Empty);
    }
}