using motoi.platform.ui.bindings;

namespace motoi.platform.ui.actions {
    /// <summary> Provides a basic implementation of <see cref="IActionHandler"/>. </summary>
    public abstract class AbstractActionHandler : PropertyChangedDispatcher, IActionHandler {
        
        private bool _isEnabled = true;

        /// <inheritdoc />
        public virtual bool IsEnabled {
            get { return _isEnabled; }
            set { 
                _isEnabled = value;
                DispatchPropertyChanged(nameof(IsEnabled));
            }
        }

        /// <inheritdoc />
        public abstract void Run();
    }
}