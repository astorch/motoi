using System.IO;
using motoi.platform.ui.actions;
using motoi.platform.ui.images;

namespace motoi.platform.ui.toolbars {
    /// <summary>
    /// Describes a tool bar.
    /// </summary>
    public interface IToolBar {
        /// <summary>
        /// Adds a button to the tool bar with the given <paramref name="label"/>. If the button is clicked, the given 
        /// <paramref name="actionHandler"/> is invoked. This button has no image.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="actionHandler">Action handler</param>
        /// <param name="toolTipText">Additional tooltip text. When NULL <paramref name="label"/> is used as tooltip text</param>
        void AddButton(string label, IActionHandler actionHandler, string toolTipText = null);

        /// <summary>
        /// Adds a button to the tool bar with the given <paramref name="label"/> and the given <paramref name="image"/>. 
        /// If the button is clicked, the given  <paramref name="actionHandler"/> is invoked.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="image">Image to use for the button</param>
        /// <param name="actionHandler">Action handler</param>
        /// <param name="toolTipText">Additional tooltip text. When NULL <paramref name="label"/> is used as tooltip text</param>
        void AddButton(string label, ImageDescriptor image, IActionHandler actionHandler, string toolTipText = null);

        /// <summary>
        /// Adds a button to the tool bar with the given <paramref name="label"/> and the given <paramref name="image"/>. 
        /// If the button is clicked, the given  <paramref name="actionHandler"/> is invoked.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="image">Stream of the image to use for the button</param>
        /// <param name="actionHandler">Action handler</param>
        /// <param name="toolTipText">Additional tooltip text. When NULL <paramref name="label"/> is used as tooltip text</param>
        void AddButton(string label, Stream image, IActionHandler actionHandler, string toolTipText = null);

        IToolBarComboBox AddComboBox();
    }
}