namespace motoi.platform.ui.bindings {
    /// <summary>
    /// Defines a bindable property that announces the
    /// expected value type of the binding source.
    /// </summary>
    /// <typeparam name="TValue">Type of value being bound</typeparam>
    public interface IBindableProperty<out TValue> : IBindableProperty {
        /// <summary> Returns the default property value. </summary>
        new TValue DefaultValue { get; }
    }
}