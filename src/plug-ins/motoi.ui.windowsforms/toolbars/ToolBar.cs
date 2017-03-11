using System.Drawing;
using System.IO;
using System.Windows.Forms;
using motoi.platform.ui.actions;
using motoi.platform.ui.images;
using motoi.platform.ui.toolbars;

namespace motoi.ui.windowsforms.toolbars {
    /// <summary>
    /// Provides an implementation of <see cref="IToolBar"/>.
    /// </summary>
    public class ToolBar : ToolStrip, IToolBar {
        /// <summary>
        /// Adds a button to the tool bar with the given <paramref name="label"/>. If the button is clicked, the given 
        /// <paramref name="actionHandler"/> is invoked. This button has no image.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="actionHandler">Action handler</param>
        /// <param name="toolTipText">Additional tooltip text. When NULL <paramref name="label"/> is used as tooltip text</param>
        public void AddButton(string label, IActionHandler actionHandler, string toolTipText = null) {
            AddButton(label, (ImageDescriptor)null, actionHandler, toolTipText);
        }

        /// <summary>
        /// Adds a button to the tool bar with the given <paramref name="label"/> and the given <paramref name="image"/>. 
        /// If the button is clicked, the given  <paramref name="actionHandler"/> is invoked.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="image">Image to use for the button</param>
        /// <param name="actionHandler">Action handler</param>
        /// <param name="toolTipText">Additional tooltip text. When NULL <paramref name="label"/> is used as tooltip text</param>
        public void AddButton(string label, ImageDescriptor image, IActionHandler actionHandler, string toolTipText = null) {
            AddButton(label, (image == null ? null : image.ImageStream), actionHandler, toolTipText);
        }

        /// <summary>
        /// Adds a button to the tool bar with the given <paramref name="label"/> and the given <paramref name="image"/>. 
        /// If the button is clicked, the given  <paramref name="actionHandler"/> is invoked.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="image">Stream of the image to use for the button</param>
        /// <param name="actionHandler">Action handler</param>
        /// <param name="toolTipText">Additional tooltip text. When NULL <paramref name="label"/> is used as tooltip text</param>
        public void AddButton(string label, Stream image, IActionHandler actionHandler, string toolTipText = null) {
            ToolStripButton toolStripButton = new ToolStripButton();
            actionHandler.PropertyChanged += (sender, args) => toolStripButton.Enabled = actionHandler.IsEnabled;

            if (image != null) {
                using (image) {
                    toolStripButton.Image = new Bitmap(image);
                }
            }

            toolStripButton.Text = label;
            toolStripButton.AutoToolTip = false;
            toolStripButton.ToolTipText = toolTipText ?? label;
            toolStripButton.Click += (sender, args) => actionHandler.Run();
            toolStripButton.Enabled = actionHandler.IsEnabled;

            Items.Add(toolStripButton);
        }

        public IToolBarComboBox AddComboBox() {
            ToolBarComboBox toolBarComboBox = new ToolBarComboBox();
            Items.Add(toolBarComboBox);
            return toolBarComboBox;
        }
    }
}