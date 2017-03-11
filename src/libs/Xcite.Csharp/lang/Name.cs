using System;
using System.Linq.Expressions;
using Xcite.Csharp.expressions;

namespace Xcite.Csharp.lang {
    /// <summary>
    /// Provides methods to resolve names of lambda expressions that reference a member.
    /// </summary>
    public static class Name {
        /// <summary>
        /// Returns the name of the property that is referenced by the given expression.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyRef">Expression referencing a property</param>
        /// <returns>Name of the property</returns>
        public static string Of<TProperty>(Expression<Func<TProperty>> propertyRef) {
            return propertyRef.GetPropertyName();
        }

        /// <summary>
        /// Returns the name of a property that is owned by a given object type. The property is 
        /// referenced by the given expression.
        /// </summary>
        /// <typeparam name="TObject">Type of object that owns the property</typeparam>
        /// <typeparam name="TProperty">Type of the property</typeparam>
        /// <param name="propertyRef">Expression referencing a property</param>
        /// <returns>Name of the property</returns>
        public static string Of<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyRef) {
            return propertyRef.GetPropertyName();
        }
    }
}