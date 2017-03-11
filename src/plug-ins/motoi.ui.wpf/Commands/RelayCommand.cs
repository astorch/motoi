using System;
using System.Windows.Input;

namespace Motoi.UI.WPF.Commands
{
    /// <summary>
    /// Provides an implementation of <see cref="ICommand"/> to use commands 
    /// to invoke delegates.
    /// </summary>
    public class RelayCommand : ICommand {

        private readonly Action<object> iAction;
        private readonly Func<object, bool> iCanExecuteCallback;
        private bool iCanExecute = true;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="action">Delegate that will be invoked</param>
        /// <param name="canExecute">Delegate that can decide when the command is executable</param>
        public RelayCommand(Action<object> action, Func<object, bool> canExecute = null){
            iAction = action;
            iCanExecuteCallback = canExecute;
        }

        /// <summary>
        /// Invokes the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) {
            if (iAction != null)
                iAction(parameter);
        }

        /// <summary>
        /// Checks if the command is executable.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True or false</returns>
        public bool CanExecute(object parameter) {
            if (iCanExecuteCallback != null)
                return iCanExecuteCallback(parameter);

            return iCanExecute;
        }

        /// <summary>
        /// Can execute changed event handler.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Dispatcher method to notify all listeners about 
        /// a state change.
        /// </summary>
        protected virtual void OnCanExecuteChanged() {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}