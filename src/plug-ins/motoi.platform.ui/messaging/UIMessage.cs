using System;

namespace motoi.platform.ui.messaging {
    /// <summary>
    /// Describes a message that is dispatched to
    /// UI elements using the <see cref="UIMessageDispatcher"/>.
    /// </summary>
    public class UIMessage : EventArgs {
        /// <summary> Targeted UI element of the message. </summary>
        public object UIElement { get; set; }

        /// <summary> Action id </summary>
        public ushort Action { get; set; }

        /// <summary> Action arguments </summary>
        public object[] Arguments { get; set; }
    }
}