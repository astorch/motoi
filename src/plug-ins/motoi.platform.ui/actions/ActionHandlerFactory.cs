namespace motoi.platform.ui.actions {
    /// <summary>
    /// Implements a factory for creating action handlers.
    /// </summary>
    static class ActionHandlerFactory {
        /// <summary>
        /// Defines an action handler which does nothing.
        /// </summary>
        public static readonly NullActionHandlerImpl NullActionHandler = new NullActionHandlerImpl();

        /// <summary>
        /// Implements an action handler which does nothing.
        /// </summary>
        public class NullActionHandlerImpl : AbstractActionHandler {
            /// <summary>
            /// Returns true if the handler is enabled or does set it.
            /// </summary>
            public override bool IsEnabled { get { return false; } }

            /// <summary>
            /// Tells the handler to invoke his action.
            /// </summary>
            public override void Run() { }
        }
    }
}