using System.ComponentModel;

namespace motoi.platform.ui.actions {
    /// <summary> Defines properties of an action handler. </summary>
    public interface IActionHandler : INotifyPropertyChanged {
        /// <summary> Returns true if the handler is enabled or does set it. </summary>
        bool IsEnabled { get; }

        /// <summary> Tells the handler to invoke his action. </summary>
        void Run();
    }
}