using System;

namespace motoi.platform.ui.actions {
    /// <summary>
    /// Implements an <see cref="IActionHandler"/> that invokes given delegates when it 
    /// is being called by the framework.
    /// </summary>
    public class ActionHandlerDelegate : AbstractActionHandler {
        private static readonly Func<bool> TrueFunc = () => true;
        private readonly Action _action;
        private readonly Func<bool> _isEnabledFunc;

        /// <summary>
        /// Creates a new instance with the given <paramref name="action"/>. The created action 
        /// is always enabled.
        /// </summary>
        /// <param name="action">Delegate to execute - must not be NULL</param>
        /// <exception cref="ArgumentNullException"/>
        public ActionHandlerDelegate(Action action) : this(action, TrueFunc) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="action"/>. The created action 
        /// is enabled if the given <paramref name="isEnabledFunc"/> returns TRUE.
        /// </summary>
        /// <param name="action">Delegate to execute - must not be NULL</param>
        /// <param name="isEnabledFunc">Delegate to check IsEnabled state - must not be NULL</param>
        /// <exception cref="ArgumentNullException"/>
        public ActionHandlerDelegate(Action action, Func<bool> isEnabledFunc) {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _isEnabledFunc = isEnabledFunc ?? throw new ArgumentNullException(nameof(isEnabledFunc));
        }

        /// <summary>
        /// Returns true if the handler is enabled or does set it.
        /// </summary>
        public override bool IsEnabled {
            get {
                bool funcResult = _isEnabledFunc();
                return funcResult;
            }
            set { base.IsEnabled = value; }
        }

        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public override void Run() {
            _action();
        }
    }
}