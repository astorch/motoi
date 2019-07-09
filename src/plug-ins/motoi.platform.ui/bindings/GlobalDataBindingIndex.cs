using System;
using System.Collections.Generic;
using System.Linq;
using xcite.csharp;

namespace motoi.platform.ui.bindings {
    /// <summary> Implements a global data binding cache that provides all data binding functionality. </summary>
    class GlobalDataBindingIndex : GenericSingleton<GlobalDataBindingIndex> {

        private readonly Dictionary<Tuple<IDataBindingSupport, IBindableProperty>, object> _globalValueCache = new Dictionary<Tuple<IDataBindingSupport, IBindableProperty>, object>();
        private readonly Dictionary<Tuple<IDataBindingSupport, IBindableProperty>, DataBinding> _globalDataBindingCache = new Dictionary<Tuple<IDataBindingSupport, IBindableProperty>, DataBinding>(); 

        /// <summary>
        /// Sets the given <paramref name="value"/> as current property value of the bound <paramref name="property"/> for 
        /// the given binding target <paramref name="obj"/>. Returns TRUE if the (new) given value is not equal to the current one. 
        /// Only in this case the new value is set.
        /// </summary>
        /// <param name="obj">Data binding target</param>
        /// <param name="property">Data binding property</param>
        /// <param name="value">New property value</param>
        /// <returns>TRUE if the new value has been set</returns>
        public bool SetValue<TValue>(IDataBindingSupport obj, IBindableProperty<TValue> property, TValue value) {
            Tuple<IDataBindingSupport, IBindableProperty> key = Tuple.Create(obj, (IBindableProperty)property);
            
            lock (_globalValueCache) {
                // Only update real new values
                _globalValueCache.TryGetValue(key, out object currentValue);
                if (Equals(currentValue, value)) return false;

                // Update the new value
                _globalValueCache[key] = value;
                return true;
            }
        }

        /// <summary>
        /// Returns the current value of the given <paramref name="property"/>
        /// of the given data binding target <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">Data binding target</param>
        /// <param name="property">Data binding property</param>
        /// <returns>Current property value</returns>
        public TValue GetValue<TValue>(IDataBindingSupport obj, IBindableProperty<TValue> property) {
            Tuple<IDataBindingSupport, IBindableProperty> key = Tuple.Create(obj, (IBindableProperty)property);

            object result;
            lock (_globalValueCache) {
                _globalValueCache.TryGetValue(key, out result);
            }
            return (TValue)result;
        }

        /// <summary>
        /// Returns the data binding for the given data binding target <paramref name="obj"/> for the given 
        /// bound property <paramref name="property"/>. Returns NULL if there is no data binding.
        /// </summary>
        /// <param name="obj">Data binding target</param>
        /// <param name="property">Data binding property</param>
        /// <returns>Data binding or NULL</returns>
        public DataBinding GetDataBinding<TValue>(IDataBindingSupport obj, IBindableProperty<TValue> property) {
            Tuple<IDataBindingSupport, IBindableProperty> key = Tuple.Create(obj, (IBindableProperty)property);
            lock (_globalDataBindingCache) {
                _globalDataBindingCache.TryGetValue(key, out DataBinding dataBinding);
                return dataBinding;
            }
        }

        /// <summary>
        /// Adds the given <paramref name="dataBinding"/> to the internal index for
        /// the given data binding target <paramref name="obj"/> 
        /// with the given bound property <paramref name="property"/>.
        /// </summary>
        /// <param name="obj">Data binding target</param>
        /// <param name="property">Data binding property</param>
        /// <param name="dataBinding">Data binding to index</param>
        public void AddToIndex<TValue>(IDataBindingSupport obj, IBindableProperty<TValue> property, DataBinding dataBinding) {
            Tuple<IDataBindingSupport, IBindableProperty> key = Tuple.Create(obj, (IBindableProperty)property);
            lock (_globalDataBindingCache) {
                _globalDataBindingCache[key] = dataBinding;
            }
        }

        /// <summary>
        /// Removes all data bindings for the given data binding
        /// target <paramref name="obj"/> from the index. 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveFromIndex(IDataBindingSupport obj) {
            // Remove cached values
            lock (_globalValueCache) {
                Tuple<IDataBindingSupport, IBindableProperty>[] keysToRemove = _globalValueCache.Keys.Where(k => Equals(obj, k.Item1)).ToArray();
                for (short i = -1; ++i != keysToRemove.Length;) {
                    Tuple<IDataBindingSupport, IBindableProperty> key = keysToRemove[i];
                    _globalValueCache.Remove(key);
                }
            }

            lock (_globalDataBindingCache) {
                Tuple<IDataBindingSupport, IBindableProperty>[] keysToRemove = _globalDataBindingCache.Keys.Where(k => Equals(obj, k.Item1)).ToArray();
                for (short i = -1; ++i != keysToRemove.Length;) {
                    Tuple<IDataBindingSupport, IBindableProperty> key = keysToRemove[i];
                    DataBinding dataBinding = _globalDataBindingCache[key];
                    dataBinding.Dispose();
                    _globalDataBindingCache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/>
        /// has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            lock (_globalValueCache) {
                _globalValueCache.Clear();
            }

            lock (_globalDataBindingCache) {
                _globalDataBindingCache.Clear();
            }
        }

        /// <summary> Will be called directly after this instance has been created. </summary>
        protected override void OnInitialize() {
            // Currently nothing to do here
        }
    }
}