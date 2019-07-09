using motoi.platform.ui.actions;
using motoi.platform.ui.widgets;

namespace motoi.platform.ui.shells {
    /// <summary> Defines the properties of a dialog window. </summary>
    public interface IDialogWindow : IWindow {
        /// <summary>
        /// Creates the dialogs and makes it visible to the user. If the flag 
        /// <paramref name="modal"/> is TRUE the dialog has a modal behaviour.
        /// </summary>
        /// <param name="modal">TRUE or FALSE</param>
        void Show(bool modal);

        /// <summary> Adds a button to dialog button area. </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="actionHandler">Handle of the action to be performed when the button is clicked</param>
        /// <returns>Handle of the button</returns>
        IButton AddButton(string label, IActionHandler actionHandler);

        /// <summary>
        /// Adds a button to the dialog button area. Additionally, the button can be set as 
        /// default button of the dialog.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="actionHandler">Handle of the action to be performed when the button is clicked</param>
        /// <param name="isDefault">If TRUE the button is set as default button of the dialog</param>
        /// <returns>Handle of the button</returns>
        IButton AddButton(string label, IActionHandler actionHandler, bool isDefault);
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IDialogWindow"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PDialogWindow : PDialogWindowControl<IDialogWindow> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IDialogWindow"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PDialogWindowControl<TDialogWindow> : PWindowControl<TDialogWindow> where TDialogWindow : class, IDialogWindow {
        
    }
}