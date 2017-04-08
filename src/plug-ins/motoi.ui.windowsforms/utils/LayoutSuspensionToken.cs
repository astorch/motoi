using System;
using System.Windows.Forms;

namespace motoi.ui.windowsforms.utils {
    /// <summary>
    /// Implements a disposable token that suspends the layout of a assigned control 
    /// until the token is being disposed. Then the layout of the assigned control 
    /// will be resumed.
    /// </summary>
    public class LayoutSuspensionToken : IDisposable {
        private Control iControl;

        /// <summary>
        /// Creates a new instance that suspends the layout of the given <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control which layout is going to be suspended</param>
        public LayoutSuspensionToken(Control control) {
            if (control == null) throw new ArgumentNullException("control");
            iControl = control;
            iControl.SuspendLayout();
        }

        /// <inheritdoc />
        public void Dispose() {
            iControl.ResumeLayout(true);
            iControl = null;
        }
    }
}