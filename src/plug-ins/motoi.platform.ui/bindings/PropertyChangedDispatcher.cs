using System;
using System.ComponentModel;
using System.Linq.Expressions;
using xcite.csharp;

namespace motoi.platform.ui.bindings {
    /// <summary>
    /// Provides a basic implementation of <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class PropertyChangedDispatcher : INotifyPropertyChanged {
        /// <summary>
        /// Will be notified when a property has been changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Dispatches the property changed event for a property addressed by the 
        /// given expression.
        /// </summary>
        /// <typeparam name="TValue">Type of the property value</typeparam>
        /// <param name="expression">Expression which addresses a property</param>
        protected void DispatchPropertyChanged<TValue>(Expression<Func<TValue>> expression) {
            string propertyName = expression.GetPropertyName();
            DispatchPropertyChanged(propertyName);
        }

        /// <summary>
        /// Dispatches the property changed event for the given property name.
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        protected void DispatchPropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChangedHandler = PropertyChanged;
            if (propertyChangedHandler == null) return;
            OnDispatchPropertyChanged(propertyChangedHandler, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Tells the instance to dispatch the given <paramref name="eventArgs"/> to the given <paramref name="propertyChangedEventHandler"/>. 
        /// </summary>
        /// <param name="propertyChangedEventHandler">Event handler to to notify</param>
        /// <param name="eventArgs">Event arguments</param>
        protected virtual void OnDispatchPropertyChanged(PropertyChangedEventHandler propertyChangedEventHandler, PropertyChangedEventArgs eventArgs) {
            if (propertyChangedEventHandler == null) return;
            propertyChangedEventHandler(this, eventArgs);
        }
    }
}