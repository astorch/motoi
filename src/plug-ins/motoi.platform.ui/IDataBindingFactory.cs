using motoi.platform.ui.bindings;

namespace motoi.platform.ui {
    /// <summary>
    /// Defines a factory to create UI framework specific data bindings.
    /// </summary>
    public interface IDataBindingFactory {
        /// <summary>
        /// Tells the factory to create a binding to the given <paramref name="control"/> 
        /// </summary>
        /// <param name="control">Control that is beeing used as binding target</param>
        /// <param name="property">Property of the control that shall be bound</param>
        /// <param name="dataBinding">Data binding meta data</param>
        void Apply(object control, IBindableProperty property, DataBinding dataBinding);
    }
}