using System;
using motoi.plugins;

namespace motoi.extensions {
    /// <summary> Provides common methods to load classes that has been provided by plug-ins. </summary>
    public static class TypeLoader {
        /// <summary> Returns the class with the given name that has been provided by the given bundle. </summary>
        /// <param name="bundle">Bundle which provides the class</param>
        /// <param name="typeName">Name of the desired type</param>
        /// <returns>Type or null</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static Type TypeForName(IBundle bundle, string typeName) {
            string bundleName = bundle.Name;
            string assemblyQualifiedName = string.Format("{0}.{1},{0}", bundleName, typeName);
            
            Type type = Type.GetType(assemblyQualifiedName);
            if (type == null) throw new InvalidOperationException($"There is no known type for '{assemblyQualifiedName}'");
            
            return type;
        }
    }
}