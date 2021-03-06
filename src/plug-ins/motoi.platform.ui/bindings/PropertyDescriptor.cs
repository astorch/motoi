﻿using System;
using System.Linq;
using System.Reflection;
using xcite.csharp;

namespace motoi.platform.ui.bindings {
    /// <summary> Provides methods to create instances of <see cref="IBindableProperty"/>. </summary>
    public static class PropertyDescriptor {
        /// <summary> Creates an instance of <see cref="IBindableProperty"/> based of the given parameters. </summary>
        /// <param name="controlType">Type of the control that provides the property</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="defaultValue">Advised the default (initial) value</param>
        /// <param name="bindsTwoByDefault">If TRUE the property advised to support two-way-binding at default</param>
        /// <param name="defaultSourceUpdateTrigger">Advised source update trigger for the property. Must not be <see cref="EDataBindingSourceUpdateTrigger.Default"/>.</param>
        /// <returns>Newly created instance of <see cref="IBindableProperty"/></returns>
        /// <exception cref="ArgumentNullException">If any parameter is NULL</exception>
        /// <exception cref="ArgumentException">If <paramref name="defaultSourceUpdateTrigger"/> is <see cref="EDataBindingSourceUpdateTrigger.Default"/></exception>
        public static IBindableProperty<TValue> CreateInfo<TValue>(Type controlType, string propertyName, TValue defaultValue, bool bindsTwoByDefault = false, EDataBindingSourceUpdateTrigger defaultSourceUpdateTrigger = null) {
            if (controlType == null) throw new ArgumentNullException(nameof(controlType));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            if (defaultSourceUpdateTrigger == EDataBindingSourceUpdateTrigger.Default) throw new ArgumentException($"Default source update trigger cannot be '{EDataBindingSourceUpdateTrigger.Default}'!");

            PropertyInfo[] controlTypeProperties = controlType.GetPublicProperties();
            PropertyInfo propertyInfo = controlTypeProperties.First(property => property.Name == propertyName);

            return new BindablePropertyImpl<TValue>(controlType, propertyName, propertyInfo) {
                BindsTwoWayByDefault = bindsTwoByDefault,
                DefaultUpdateSourceUpdateTrigger = defaultSourceUpdateTrigger ?? EDataBindingSourceUpdateTrigger.PropertyChanged,
                DefaultValue = defaultValue
            };
        }

        /// <summary> Provides an anonymous implementation of <see cref="IBindableProperty"/>. </summary>
        class BindablePropertyImpl<TValue> : IBindableProperty<TValue> {
            /// <summary> Creates a new instance based on the given parameters. </summary>
            /// <param name="controlType">Type of the control that provides the property</param>
            /// <param name="name">Name of the property</param>
            /// <param name="propertyInfo">Property info of the property</param>
            public BindablePropertyImpl(Type controlType, string name, PropertyInfo propertyInfo) {
                ControlType = controlType;
                Name = name;
                PropertyInfo = propertyInfo;
                DefaultValue = default;
            }
            
            /// <inheritdoc />
            public EDataBindingSourceUpdateTrigger DefaultUpdateSourceUpdateTrigger { get; set; }
            
            /// <inheritdoc />
            public bool BindsTwoWayByDefault { get; set; }
            
            /// <inheritdoc />
            public TValue DefaultValue { get; set; }

            /// <inheritdoc />
            object IBindableProperty.DefaultValue 
                => DefaultValue;

            /// <inheritdoc />
            public string Name { get; }
            
            /// <inheritdoc />
            public Type ControlType { get; }

            /// <inheritdoc />
            public PropertyInfo PropertyInfo { get; }
            
            /// <inheritdoc />
            public override string ToString() {
                return $"BindableProperty '{Name}' ({PropertyInfo})";
            }
        }
    }
}