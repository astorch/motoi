using System;
using System.ComponentModel;
using System.Reflection;
using xcite.csharp;
using xcite.logging;

namespace motoi.platform.ui.bindings {
    /// <summary> Describes a data binding. </summary>
    public class DataBinding : XObject {
        private static readonly ILog _log = LogManager.GetLog(typeof(DataBinding));

        /// <summary> Creates a new instance using the given path. </summary>
        /// <param name="source">Binding source object - must not be NULL</param>
        /// <param name="path">Path to the property to bind</param>
        public DataBinding(object source, string path) {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Path = !string.IsNullOrEmpty(path) ? path : throw new ArgumentNullException(nameof(path)); 
            SourcePropertyInfo = DataBindingUtil.ResolvePropertyInfo(source, path);

            BindingMode = EDataBindingMode.Default;
            BindingSourceUpdateTrigger = EDataBindingSourceUpdateTrigger.Default;
            
            if (SourcePropertyInfo == null) throw new DataBindingException($"The given source object of type '{source.GetType()}' does not have a property under the given path '{path}'");
        }

        /// <summary> Returns the data binding mode or does set it. </summary>
        public EDataBindingMode BindingMode { get; set; }

        /// <summary> Returns the data binding update trigger. </summary>
        public EDataBindingSourceUpdateTrigger BindingSourceUpdateTrigger { get; set; }

        /// <summary> Returns the path to the bound source property. </summary>
        public string Path { get; }

        /// <summary> Returns the source of the binding or does set it. </summary>
        public object Source { get; set; }

        /// <summary> Returns the info of the source property. </summary>
        public PropertyInfo SourcePropertyInfo { get; }

        /// <summary> Returns the target of the binding or does set it. </summary>
        public object Target { get; private set; }

        /// <summary> Returns the info of the target property. </summary>
        public PropertyInfo TargetPropertyInfo { get; private set; }

        /// <summary> Returns TRUE if the binding supports to update the source. </summary>
        private bool ShallUpdateSource { get; set; }

        /// <summary>
        /// Connects this instance to the given data binding <paramref name="target"/> for the <paramref name="property"/>.
        /// </summary>
        /// <param name="target">Data binding target to connect</param>
        /// <param name="property">Target property to bind</param>
        public void Connect<TValue>(IDataBindingSupport target, IBindableProperty<TValue> property) {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            TargetPropertyInfo = (property ?? throw new ArgumentNullException(nameof(property))).PropertyInfo;

            // Apply default value to the target
            ApplyDefaultPropertyValue(target, property);

            // Selected binding mode
            EDataBindingMode bindingMode = BindingMode;
            if (bindingMode == EDataBindingMode.Default)
                bindingMode = (property.BindsTwoWayByDefault ? EDataBindingMode.TwoWay : EDataBindingMode.OneWay);

            // Set property
            ShallUpdateSource = (bindingMode == EDataBindingMode.OneWayToSource || bindingMode == EDataBindingMode.TwoWay);

            // OneWayToSource
            if (bindingMode == EDataBindingMode.OneWayToSource) { 
                // Update source immediately
                ApplyValueToBindingSource(Source, SourcePropertyInfo, Target, property.PropertyInfo);
                return;
            }

            // Update immediately the target
            ApplyValueToBindingTarget(target, property.PropertyInfo, Source, SourcePropertyInfo);
                
            // In this case we're finished here
            if (bindingMode == EDataBindingMode.OneTime) return;

            // OneWay and/or TwoWay

            // Add update listener to the source (if supported)
            if (Source is INotifyPropertyChanged propertyChangedDispatcher) {
                propertyChangedDispatcher.PropertyChanged += OnSourcePropertyChanged;
            }
        }

        /// <summary> Tells the data binding to update the source value. </summary>
        /// <param name="sender">Method invocator</param>
        /// <param name="property">Property to update</param>
        /// <param name="value">New value</param>
        public void UpdateSource<TValue>(object sender, IBindableProperty<TValue> property, object value) {
            if (!ShallUpdateSource) return;

            ApplyValueToBindingSource(Source, SourcePropertyInfo, Target, property.PropertyInfo);
        }

        /// <summary>
        /// Tells the instance that it is going to be disposed
        /// and shall clean up its resources before.
        /// </summary>
        public void Dispose() {
            // Remove handler
            if (Source is INotifyPropertyChanged propertyChangedDispatcher)
                propertyChangedDispatcher.PropertyChanged -= OnSourcePropertyChanged;
        }

        /// <summary> Applies the current value of the binding target to the binding source. </summary>
        /// <param name="source">Data binding source</param>
        /// <param name="sourcePropertyInfo">Data binding source property</param>
        /// <param name="target">Data binding target</param>
        /// <param name="targetPropertyInfo">Data binding target property</param>
        private void ApplyValueToBindingSource(object source, PropertyInfo sourcePropertyInfo, object target, PropertyInfo targetPropertyInfo) {
            object targetValue = null;
            try {
                if (target == null) throw new ArgumentNullException(nameof(target));
                if (targetPropertyInfo == null) throw new ArgumentNullException(nameof(targetPropertyInfo));
                if (source == null) throw new ArgumentNullException(nameof(source));
                if (sourcePropertyInfo == null) throw new ArgumentNullException(nameof(sourcePropertyInfo));
                
                targetValue = targetPropertyInfo.GetValue(target, null);
                ApplyValueChange(source, sourcePropertyInfo, targetValue);
            } catch (Exception ex) {
                _log.Error(
                    "Error on applying target value to binding source. " + 
                   $"target: '{target}', target property: '{targetPropertyInfo}', " +
                   $"source: '{source}, source property: '{sourcePropertyInfo}', source property value: '{targetValue ?? "NULL"}'.",
                    ex
                );
            }
        }

        /// <summary> Applies the current value of the binding source to the binding target. </summary>
        /// <param name="target">Data binding target</param>
        /// <param name="targetPropertyInfo">Data binding target property</param>
        /// <param name="source">Data binding source</param>
        /// <param name="sourcePropertyInfo">Data binding source property</param>
        private void ApplyValueToBindingTarget(object target, PropertyInfo targetPropertyInfo, object source, PropertyInfo sourcePropertyInfo) {
            object sourceValue = null;
            try {
                if (target == null) throw new ArgumentNullException(nameof(target));
                if (targetPropertyInfo == null) throw new ArgumentNullException(nameof(targetPropertyInfo));
                if (source == null) throw new ArgumentNullException(nameof(source));
                if (sourcePropertyInfo == null) throw new ArgumentNullException(nameof(sourcePropertyInfo));

                sourceValue = sourcePropertyInfo.GetValue(source, null);
                ApplyValueChange(target, targetPropertyInfo, sourceValue);
            } catch (Exception ex) {
                _log.Error(
                    "Error on applying source value to binding target. " + 
                   $"target: '{target}', target property: '{targetPropertyInfo}', " +
                   $"source: '{source}, source property: '{sourcePropertyInfo}', source property value: '{sourceValue ?? "NULL"}'. ",
                    ex
                );
            }
        }

        /// <summary>
        /// Applies the given <paramref name="value"/> to the given property (<paramref name="propertyInfo"/>) 
        /// of the given target object (<paramref name="obj"/>).
        /// </summary>
        /// <param name="obj">Object the property value is applied to</param>
        /// <param name="propertyInfo">Property info</param>
        /// <param name="value">Value</param>
        private void ApplyValueChange(object obj, PropertyInfo propertyInfo, object value) {
            try {
                if (obj == null) throw new ArgumentNullException(nameof(obj));
                if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
                
                // TODO Support type-safe
                // TODO Support conversion

                // Don't update if the values are equal
//                object currentValue = propertyInfo.GetValue(obj, null);
//                if (Equals(currentValue, value)) return;

                // Update the property value
                propertyInfo.SetValue(obj, value, null);
            } catch (Exception ex) {
                _log.Error($"Errror on performing binding update. Object is '{obj}', property is '{propertyInfo}' and value is '{value ?? "NULL"}'", ex);
            }
        }

        /// <summary>
        /// Applies the default value of the given <paramref name="property"/> to the given binding target <paramref name="target"/>.
        /// </summary>
        /// <param name="target">Data binding target</param>
        /// <param name="property">Data binding property</param>
        private void ApplyDefaultPropertyValue<TValue>(IDataBindingSupport target, IBindableProperty<TValue> property) {
            try {
                if (target == null) throw new ArgumentNullException(nameof(target));
                if (property == null) throw new ArgumentNullException(nameof(property));

                ApplyValueChange(target, property.PropertyInfo, property.DefaultValue);
            } catch (Exception ex) {
                _log.Error("Error on applying default property value to the binding target.", ex);
            }
        }

        /// <summary> Is invoked when the data binding source dispatches an property changed event. </summary>
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName != SourcePropertyInfo.Name) return;
            ApplyValueToBindingTarget(Target, TargetPropertyInfo, Source, SourcePropertyInfo);
        }
    }

    /// <summary> Provides some tooling methods to support binding operations. </summary>
    static class DataBindingUtil {
        /// <summary>
        /// Resolves and returns the <see cref="PropertyInfo"/> of the <paramref name="obj"/> property that is target 
        /// by the given <paramref name="path"/>.
        /// </summary>
        /// <param name="obj">Object that provides the property to look up</param>
        /// <param name="path">Path to the property to look up</param>
        /// <returns>Resolved <see cref="PropertyInfo"/> or NULL</returns>
        public static PropertyInfo ResolvePropertyInfo(object obj, string path) {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            
            string[] pathFragments = path.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            object navigationObject = obj;
            for (short i = -1; ++i != pathFragments.Length;) {
                string pathFragment = pathFragments[i];
                PropertyInfo property = navigationObject.GetType().GetProperty(pathFragment);
                if (i + 1 == pathFragments.Length) return property;
                navigationObject = property.GetValue(obj, null);
            }

            return null;
        }
    }
}