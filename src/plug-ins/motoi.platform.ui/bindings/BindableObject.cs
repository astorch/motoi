using System;
using System.Linq.Expressions;
using Xcite.Csharp.assertions;
using Xcite.Csharp.expressions;

namespace motoi.platform.ui.bindings {
    /// <summary>
    /// Implements a base class to defined bindable objects. All objects that may support binding operations 
    /// must inherit from this one.
    /// </summary>
    /// <typeparam name="TControl">Concrete bindable object type</typeparam>
    public abstract class BindableObject<TControl> where TControl : IDataBindingSupport {
        /// <summary>
        /// Creates a property meta data descriptor for the public property with the given <paramref name="propertyName"/> of the associated 
        /// <typeparamref name="TControl"/> type.
        /// </summary>
        /// <typeparam name="TValue">Type of property value</typeparam>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="defaultValue">Default property value</param>
        /// <param name="bindsTwoWayByDefault">If TRUE the property supports a two-way-binding per default</param>
        /// <param name="defaultSourceUpdateTrigger">Sets the default source update trigger</param>
        /// <returns>Created property meta data descriptor</returns>
        protected static IBindableProperty CreatePropertyInfo<TValue>(string propertyName, TValue defaultValue, bool bindsTwoWayByDefault = false, 
            EDataBindingSourceUpdateTrigger defaultSourceUpdateTrigger = null) {
            return PropertyDescriptor.CreateInfo(typeof(TControl), propertyName, defaultValue, bindsTwoWayByDefault, defaultSourceUpdateTrigger);
        }

        /// <summary>
        /// Creates a property meta data descriptor for the public property with the given <paramref name="propertyRef"/> of the associated 
        /// <typeparamref name="TControl"/> type.
        /// </summary>
        /// <typeparam name="TValue">Type of property value</typeparam>
        /// <param name="propertyRef">Reference of the property</param>
        /// <param name="defaultValue">Default property value</param>
        /// <param name="bindsTwoWayByDefault">If TRUE the property supports a two-way-binding per default</param>
        /// <param name="defaultSourceUpdateTrigger">Sets the default source update trigger</param>
        /// <returns>Created property meta data descriptor</returns>
        protected static IBindableProperty CreatePropertyInfo<TValue>(Expression<Func<TControl, TValue>> propertyRef, TValue defaultValue, bool bindsTwoWayByDefault = false,
            EDataBindingSourceUpdateTrigger defaultSourceUpdateTrigger = null) {
            string propertyName = propertyRef.GetPropertyName();
            return CreatePropertyInfo(propertyName, defaultValue, bindsTwoWayByDefault, defaultSourceUpdateTrigger);
        }

        /// <summary>
        /// Returns the current model value for the given data binding target <paramref name="obj"/> and the given property <paramref name="property"/>;
        /// </summary>
        /// <typeparam name="TValue">Type of the expected model value</typeparam>
        /// <param name="obj">Data binding target</param>
        /// <param name="property">Data binding property</param>
        /// <returns>Current model value</returns>
        public static TValue GetModelValue<TValue>(TControl obj, IBindableProperty property) {
            return (TValue) GlobalDataBindingIndex.Instance.GetValue(obj, property);
        }

        /// <summary>
        /// Sets the current model value for the given data binding target <paramref name="obj"/> and the given <paramref name="property"/> 
        /// to the given <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the model value</typeparam>
        /// <param name="obj">Data binding target</param>
        /// <param name="property">Data binding property</param>
        /// <param name="value">New value to set</param>
        public static void SetModelValue<TValue>(TControl obj, IBindableProperty property, TValue value) { // Called from model
            GlobalDataBindingIndex.Instance.SetValue(obj, property, value);
        }

        /// <summary>
        /// Sets the current model value for the given data binding target <paramref name="obj"/> and the given <paramref name="property"/> 
        /// to the given <paramref name="value"/>. Additionally an <paramref name="updateReason"/> has to be given!
        /// </summary>
        /// <typeparam name="TValue">Type of the model value</typeparam>
        /// <param name="obj">Data binding target </param>
        /// <param name="property">Data binding property</param>
        /// <param name="value">New value to set</param>
        /// <param name="updateReason">Update reason - must not be NULL</param>
        /// <exception cref="ArgumentNullException">If <paramref name="updateReason"/> is NULL</exception>
        public static void SetModelValue<TValue>(TControl obj, IBindableProperty property, TValue value, // Called from control
            EBindingSourceUpdateReason updateReason) {
            Assert.NotNull(() => updateReason);
            DataBinding dataBinding = GlobalDataBindingIndex.Instance.GetDataBinding(obj, property);

            // If there is no data binding there is no model to update
            if (dataBinding == null) return; // TODO Check if correct

            // If set to default the default setting of the property is used
            EDataBindingSourceUpdateTrigger updateSourceTrigger = dataBinding.BindingSourceUpdateTrigger;
            if (updateSourceTrigger == EDataBindingSourceUpdateTrigger.Default)
                updateSourceTrigger = property.DefaultUpdateSourceUpdateTrigger;

            // Check if reason fits to set trigger
            if (updateReason == EBindingSourceUpdateReason.PropertyChanged && updateSourceTrigger != EDataBindingSourceUpdateTrigger.PropertyChanged) return;
            if (updateReason == EBindingSourceUpdateReason.LostFocus && updateSourceTrigger != EDataBindingSourceUpdateTrigger.LostFocus) return;

            // Update the value
            if (!GlobalDataBindingIndex.Instance.SetValue(obj, property, value)) return;

            // Update the source
            dataBinding.UpdateSource(obj, property, value);
        }

        /// <summary>
        /// Notifies the framework that the given object is being disposed.
        /// </summary>
        /// <param name="obj">Object that is being disposed</param>
        public static void DisposeInstance(TControl obj) {
            GlobalDataBindingIndex.Instance.RemoveFromIndex(obj);
        }
    }
}