using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Xcite.Collections;

namespace motoi.platform.ui.data {
    /// <summary>
    /// Provides a default implementation of <see cref="IContentProvider"/> that casts
    /// the given input element to an array.
    /// </summary>
    public class ArrayContentProvider : IContentProvider {
        /// <summary>
        /// Returns a default instance.
        /// </summary>
        public static readonly IContentProvider Instance = new ArrayContentProvider();

        /// <summary>
        /// Return all elements that derive from the given input element.
        /// </summary>
        /// <param name="input">Input element</param>
        /// <returns></returns>
        public object[] GetElements(object input) {
            if (input == null) return new object[0];

            // Should handle most of the input
            IEnumerable enumerable = input as IEnumerable;
            if (enumerable != null) return enumerable.ToArray();

            // If it's an array we can convert the elements manually
            Type inputType = input.GetType();
            if (inputType.IsArray) {
                Type inputElementType = inputType.GetElementType();
                return new ObjectConverter(inputElementType).Convert(input);
            }

            // Probably it's a single object
            return new[] {input};
        }

        /// <summary>
        /// Provides an array object converter.
        /// </summary>
        class ObjectConverter {
            private readonly Type iObjectType;

            /// <summary>
            /// Creates a new instance for the given <paramref name="objectType"/>
            /// </summary>
            /// <param name="objectType">Type of object to convert</param>
            public ObjectConverter(Type objectType) {
                iObjectType = objectType;
            }

            /// <summary>
            /// Converts the given object (array) into a real object array.
            /// </summary>
            /// <param name="input">Object to convert</param>
            /// <returns>Converted real object array</returns>
            public object[] Convert(object input) {
                MethodInfo convertMethod = GetType().GetMethod("ArrayConvert");
                MethodInfo paramConvertMethod = convertMethod.MakeGenericMethod(iObjectType);
                return (object[]) paramConvertMethod.Invoke(this,new []{input});
            }

            public object[] ArrayConvert<T>(T[] input) {
                object[] rawObjects = EnumerableExtensions.ToArray(input.Cast<object>());
                return rawObjects;
            }
        }
    }
}