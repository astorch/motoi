using motoi.platform.ui.bindings;

namespace motoi.platform.ui.actions {
    /// <summary>
    /// Provides a basic implementation of <see cref="IActionHandler"/>.
    /// </summary>
    public abstract class AbstractActionHandler : PropertyChangedDispatcher, IActionHandler {
        /// <summary>
        /// Backing variable for the enabled flag.
        /// </summary>
        private bool iIsEnabled = true;

        /// <summary>
        /// Returns true if the handler is enabled or does set it.
        /// </summary>
        public virtual bool IsEnabled {
            get { return iIsEnabled; }
            set { 
                iIsEnabled = value;
                DispatchPropertyChanged(() => IsEnabled);
            }
        }

        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public abstract void Run();
    }
}