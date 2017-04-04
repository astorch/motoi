using System.Collections.Generic;
using motoi.platform.ui;
using motoi.platform.ui.controls;
using motoi.platform.ui.shells;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="IDialogWindow"/>.
    /// </summary>
    public class DialogWindow : Window, IDialogWindow {

        #region IDialogWindow

        /// <summary>
        /// Returns a collection of buttons of the dialog window.
        /// </summary>
        IList<IButton> IDialogWindow.Buttons {
            get { return null; }
        }

        /// <summary>
        /// Creates the dialogs and makes it visible to the user. If the flag 
        /// <paramref name="modal"/> is TRUE the dialog has a modal behaviour.
        /// </summary>
        /// <param name="modal">TRUE or FALSE</param>
        void IDialogWindow.Show(bool modal) {
            if (modal)
                ShowDialog();
            else
                Show();
        }

        /// <summary>
        /// Creates the window and makes it visible to the user.
        /// </summary>
        void IShell.Show() {
            ((IDialogWindow) this).Show(true);
        }

        #endregion
    
    }
}