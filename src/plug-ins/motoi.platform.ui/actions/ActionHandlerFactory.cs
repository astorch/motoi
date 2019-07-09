namespace motoi.platform.ui.actions {
    /// <summary> Implements a factory for creating action handlers. </summary>
    static class ActionHandlerFactory {
        /// <summary> Defines an action handler which does nothing. </summary>
        public static readonly NullActionHandlerImpl NullActionHandler = new NullActionHandlerImpl();

        /// <summary> Implements an action handler which does nothing. </summary>
        public class NullActionHandlerImpl : AbstractActionHandler {
            /// <inheritdoc />
            public override bool IsEnabled 
                => false;


            /// <inheritdoc />
            public override void Run() {
                // Nothing to do here
            }
        }
    }
}