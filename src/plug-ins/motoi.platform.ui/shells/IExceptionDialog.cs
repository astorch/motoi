namespace motoi.platform.ui.shells {
    /// <summary> Defines an exception dialog. </summary>
    public interface IExceptionDialog : IMessageDialog {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IExceptionDialog"/> that is used by data binding operations.
    /// </summary>
    public class PExceptionDialog : PMessageDialogControl<IExceptionDialog> {
        
    }
}