using Xcite.Csharp.assertions;

namespace motoi.platform.ui.bindings {
    /// <summary>
    /// Provides methods to apply bindings to UI controls.
    /// </summary>
    public static class DataBindingOperator {
        /// <summary>
        /// Applies the given <paramref name="dataBinding"/> to the given <paramref name="uiElement"/> for the 
        /// given <paramref name="property"/>.
        /// </summary>
        /// <param name="uiElement">UI Element that is used as target of the binding</param>
        /// <param name="property">Property that is being bound</param>
        /// <param name="dataBinding">Data binding</param>
        public static void Apply(IDataBindingSupport uiElement, IBindableProperty property, DataBinding dataBinding) {
            Assert.NotNull(() => uiElement);
            Assert.NotNull(() => property);
            Assert.NotNull(() => dataBinding);

            dataBinding.Connect(uiElement, property);
            GlobalDataBindingIndex.Instance.AddToIndex(uiElement, property, dataBinding);
        } 
    }
}