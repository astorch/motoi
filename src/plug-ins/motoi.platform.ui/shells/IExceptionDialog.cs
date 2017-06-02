using System;
using motoi.platform.ui.bindings;

namespace motoi.platform.ui.shells {
    /// <summary> Defines an exception dialog. </summary>
    public interface IExceptionDialog : IMessageDialog {
        /// <summary> Returns the exception to show or does set it. </summary>
        Exception Exception { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IExceptionDialog"/> that is used by data binding operations.
    /// </summary>
    public class PExceptionDialog : PMessageDialogControl<IExceptionDialog> {
        /// <summary> Exception property meta data </summary>
        public static readonly IBindableProperty<Exception> ExceptionProperty = CreatePropertyInfo(nameof(IExceptionDialog.Exception), (Exception)null);
    }
}