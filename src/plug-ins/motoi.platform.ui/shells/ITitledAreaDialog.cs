using System;
using motoi.platform.ui.controls;

namespace motoi.platform.ui.shells {
    /// <summary>
    /// Defines the properties of a Titled Area Dialog.
    /// </summary>
    public interface ITitledAreaDialog : IDialogWindow {
        /// <summary>
        /// Returns the title of the dialog or does set it.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Returns the description text of the dialog or does set it.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Adds a button to dialog button area.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="action">Action to be performed when the button is clicked</param>
        /// <returns>Handle for the button</returns>
        IButton AddButton(string label, Action action);
    }
}