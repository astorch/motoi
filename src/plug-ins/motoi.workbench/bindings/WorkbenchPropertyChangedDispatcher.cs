using System;
using System.ComponentModel;
using motoi.platform.ui.bindings;

namespace motoi.workbench.bindings {
    /// <summary>
    /// Extends <see cref="PropertyChangedDispatcher"/> to support sending notifications to the UI 
    /// thread asynchronously.
    /// </summary>
    public class WorkbenchPropertyChangedDispatcher : PropertyChangedDispatcher {
        /// <summary>
        /// Tells the instance to dispatch the given <paramref name="eventArgs"/> to the given <paramref name="propertyChangedEventHandler"/>. 
        /// </summary>
        /// <param name="propertyChangedEventHandler">Event handler to to notify</param>
        /// <param name="eventArgs">Event arguments</param>
        protected override void OnDispatchPropertyChanged(PropertyChangedEventHandler propertyChangedEventHandler,
            PropertyChangedEventArgs eventArgs) {
            
            Action uiJob = () => base.OnDispatchPropertyChanged(propertyChangedEventHandler, eventArgs);
            PlatformUI.Instance.Invoker.InvokeAsync(uiJob);
        }
    }
}