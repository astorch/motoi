using System;
using System.Linq;
using System.Reflection;
using Xcite.Csharp.assertions;
using Xcite.Csharp.lang;

namespace motoi.platform.ui.bindings {
    /// <summary>
    /// Provides methods to create instances of <see cref="IBindableProperty"/>.
    /// </summary>
    public static class PropertyDescriptor {
        /// <summary>
        /// Creates an instance of <see cref="IBindableProperty"/> based of the given parameters.
        /// </summary>
        /// <param name="controlType">Type of the control that provides the property</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="defaultValue">Advised the default (initial) value</param>
        /// <param name="bindsTwoByDefault">If TRUE the property advised to support two-way-binding at default</param>
        /// <param name="defaultSourceUpdateTrigger">Advised source update trigger for the property. Must not be <see cref="EDataBindingSourceUpdateTrigger.Default"/>.</param>
        /// <returns>Newly created instance of <see cref="IBindableProperty"/></returns>
        /// <exception cref="ArgumentNullException">If any parameter is NULL</exception>
        /// <exception cref="ArgumentException">If <paramref name="defaultSourceUpdateTrigger"/> is <see cref="EDataBindingSourceUpdateTrigger.Default"/></exception>
        public static IBindableProperty CreateInfo<TValue>(Type controlType, string propertyName, TValue defaultValue, bool bindsTwoByDefault = false, EDataBindingSourceUpdateTrigger defaultSourceUpdateTrigger = null) {
            Assert.NotNull(() => controlType);
            Assert.NotNullOrEmpty(() => propertyName);

            if (defaultSourceUpdateTrigger == EDataBindingSourceUpdateTrigger.Default) throw new ArgumentException(string.Format("Default source update trigger cannot be '{0}'!", EDataBindingSourceUpdateTrigger.Default));

            PropertyInfo[] controlTypeProperties = controlType.GetPublicProperties();
            PropertyInfo propertyInfo = controlTypeProperties.First(property => property.Name == propertyName);

            return new BindablePropertyImpl(controlType, propertyName, propertyInfo) {
                BindsTwoWayByDefault = bindsTwoByDefault,
                DefaultUpdateSourceUpdateTrigger = defaultSourceUpdateTrigger ?? EDataBindingSourceUpdateTrigger.PropertyChanged,
                DefaultValue = defaultValue
            };
        }

        /// <summary>
        /// Provides an anonymous implementation of <see cref="IBindableProperty"/>.
        /// </summary>
        class BindablePropertyImpl : IBindableProperty {
            /// <summary>
            /// Creates a new instance based on the given parameters.
            /// </summary>
            /// <param name="controlType">Type of the control that provides the property</param>
            /// <param name="name">Name of the property</param>
            /// <param name="propertyInfo">Property info of the property</param>
            public BindablePropertyImpl(Type controlType, string name, PropertyInfo propertyInfo) {
                ControlType = controlType;
                Name = name;
                PropertyInfo = propertyInfo;
            }

            /// <summary>
            /// Returns the trigger mode when the binding source is being updated.
            /// </summary>
            public EDataBindingSourceUpdateTrigger DefaultUpdateSourceUpdateTrigger { get; set; }

            /// <summary>
            /// Returns TRUE if the property advises two-way-binding by default.
            /// </summary>
            public bool BindsTwoWayByDefault { get; set; }

            /// <summary>
            /// Returns the default property value.
            /// </summary>
            public object DefaultValue { get; set; }

            /// <summary>
            /// Returns the name of the bindable property.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Returns the type of the control that provides the property.
            /// </summary>
            public Type ControlType { get; private set; }

            /// <summary>
            /// Returns the property info of the property.
            /// </summary>
            public PropertyInfo PropertyInfo { get; private set; }

            /// <summary>
            /// Returns a string representation of the object.
            /// </summary>
            /// <returns>String representation of the object</returns>
            public override string ToString() {
                string result = string.Format("BindableProperty '{0}' ({1})", Name, PropertyInfo);
                return result;
            }
        }
    }
}