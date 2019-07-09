using System;
using System.Reflection;

namespace motoi.platform.ui.bindings {
    /// <summary> Describes a bindable property. </summary>
    public interface IBindableProperty {
        /// <summary> Returns the name of the bindable property. </summary>
        string Name { get; }

        /// <summary> Returns the type of the control that provides the property. </summary>
        Type ControlType { get; }

        /// <summary> Returns the property info of the property. </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary> Returns TRUE if the property advises two-way-binding by default. </summary>
        bool BindsTwoWayByDefault { get; }

        /// <summary> Returns the trigger mode when the binding source is being updated. </summary>
        EDataBindingSourceUpdateTrigger DefaultUpdateSourceUpdateTrigger { get; }

        /// <summary> Returns the default property value. </summary>
        object DefaultValue { get; }
    }
}