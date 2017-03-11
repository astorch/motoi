using System.Collections.Generic;
using motoi.platform.ui.controls;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines the properties of a dialog window.
    /// </summary>
    public interface IDialogWindow : IWindow {
        /// <summary>
        /// Returns a collection of buttons of the dialog window.
        /// </summary>
        IList<IButton> Buttons { get; }

        /// <summary>
        /// Creates the dialogs and makes it visible to the user. If the flag 
        /// <paramref name="modal"/> is TRUE the dialog has a modal behaviour.
        /// </summary>
        /// <param name="modal">TRUE or FALSE</param>
        void Show(bool modal);
    }
}