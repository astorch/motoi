using System;
using System.Collections;
using System.Reflection;

namespace motoi.platform.ui.data {
    /// <summary>
    /// Provides a default implementation of <see cref="IContentProvider"/> that casts
    /// the given input element to an array.
    /// </summary>
    public class ArrayContentProvider : IContentProvider {
        /// <summary> Returns a default instance. </summary>
        public static readonly IContentProvider Instance = new ArrayContentProvider();

        /// <summary> Return all elements that derive from the given input element. </summary>
        /// <param name="input">Input element</param>
        /// <returns></returns>
        public object[] GetElements(object input) {
            if (input == null) return new object[0];

            // Should handle most of the input
            if (input is IEnumerable enumerable) return CopyToArray(enumerable);

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
        /// Copies all elements of the given <paramref name="enumerable"/>
        /// into a new array.
        /// </summary>
        private object[] CopyToArray(IEnumerable enumerable) {
            ArrayList objList = new ArrayList(100);

            IEnumerator itr = enumerable.GetEnumerator();
            try {
                while (itr.MoveNext()) {
                    objList.Add(itr.Current);
                }
            } finally {
                (itr as IDisposable)?.Dispose();
            }

            return objList.ToArray();
        }

        /// <summary> Provides an array object converter. </summary>
        class ObjectConverter {
            private readonly Type _objectType;

            /// <summary> Creates a new instance for the given <paramref name="objectType"/> </summary>
            /// <param name="objectType">Type of object to convert</param>
            public ObjectConverter(Type objectType) {
                _objectType = objectType;
            }

            /// <summary> Converts the given object (array) into a real object array. </summary>
            /// <param name="input">Object to convert</param>
            /// <returns>Converted real object array</returns>
            public object[] Convert(object input) {
                MethodInfo convertMethod = GetType().GetMethod(nameof(ArrayConvert));
                MethodInfo paramConvertMethod = convertMethod.MakeGenericMethod(_objectType);
                return (object[]) paramConvertMethod.Invoke(this,new []{input});
            }

            public object[] ArrayConvert<T>(T[] input) {
                object[] rawObjects = new object[input.Length];
                Array.Copy(input, rawObjects, input.Length);
                return rawObjects;
            }
        }
    }
}