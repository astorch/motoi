using System.Windows.Forms;

namespace motoi.ui.windowsforms.utils {
    /// <summary> Provides various extension methods for <see cref="Control"/>. </summary>
    public static class ControlExtensionMethods {
        /// <summary>
        /// Creates a new instance of <see cref="LayoutSuspensionToken"/> that is 
        /// assigned with the given <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control that is assigned with the layout suspension token</param>
        /// <returns>New instance of <see cref="LayoutSuspensionToken"/></returns>
        /// <seealso cref="LayoutSuspensionToken"/>
        public static LayoutSuspensionToken DeferLayout(this Control control) {
            return new LayoutSuspensionToken(control);
        }
    }
}