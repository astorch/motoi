using System;
using System.Linq;
using System.Reflection;

namespace Xcite.Csharp.lang {
    /// <summary>
    /// Provides various extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeMethodExtensions {
        /// <summary>
        /// Returns all public properties of this <paramref name="type"/>. If the given type is 
        /// an interface, the properties of the parent interfaces will be selected, too.
        /// </summary>
        /// <param name="type">Type which public properties to look up</param>
        /// <returns>All public properties of the given type</returns>
        public static PropertyInfo[] GetPublicProperties(this Type type) {
            if (!type.IsInterface) return type.GetProperties();

            return new[] { type }
                   .Concat(type.GetInterfaces())
                   .SelectMany(i => i.GetProperties()).ToArray();
        }
    }
}