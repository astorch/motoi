using System;
using motoi.platform.ui.messaging;

namespace motoi.ui.windowsforms.messaging {
    /// <summary>
    /// Defines event data of the <see cref="UIMessageDispatchListener.MessageReceived"/> event.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs {
        /// <summary>
        /// Returns TRUE if the message has been handled and can be dropped.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Original UI message.
        /// </summary>
        public UIMessage UIMessage { get; set; }
    }
}