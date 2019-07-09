using System;

namespace motoi.platform.ui.bindings {
    /// <summary> Provides methods to apply bindings to UI controls. </summary>
    public static class DataBindingOperator {
        /// <summary>
        /// Applies the given <paramref name="dataBinding"/> to the given <paramref name="uiElement"/> for the 
        /// given <paramref name="property"/>.
        /// </summary>
        /// <param name="uiElement">UI Element that is used as target of the binding</param>
        /// <param name="property">Property that is being bound</param>
        /// <param name="dataBinding">Data binding</param>
        /// <exception cref="ArgumentNullException">If any parameter is NULL</exception>
        public static void Apply<TValue>(IDataBindingSupport uiElement, IBindableProperty<TValue> property, DataBinding dataBinding) {
            if (uiElement == null) throw new ArgumentNullException(nameof(uiElement));
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (dataBinding == null) throw new ArgumentNullException(nameof(dataBinding));
            
            dataBinding.Connect(uiElement, property);
            GlobalDataBindingIndex.Instance.AddToIndex(uiElement, property, dataBinding);
        } 
    }
}