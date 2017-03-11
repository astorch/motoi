using System;
using motoi.platform.ui;
using motoi.platform.ui.bindings;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Provides an WinForms specific implementation of <see cref="IDataBindingFactory"/>.
    /// </summary>
    public class DataBindingFactory : IDataBindingFactory {
        /// <summary>
        /// Tells the factory to create a binding to the given <paramref name="control"/> 
        /// </summary>
        /// <param name="control">Control that is beeing used as binding target</param>
        /// <param name="property">Property of the control that shall be bound</param>
        /// <param name="dataBinding">Data binding meta data</param>
        public void Apply(object control, IBindableProperty property, DataBinding dataBinding) {
            throw new NotSupportedException();
            IDataBindingSupport dbs = control as IDataBindingSupport;
            if (dbs == null) throw new InvalidOperationException();
            dataBinding.Connect(dbs, property);
        }
    }
}