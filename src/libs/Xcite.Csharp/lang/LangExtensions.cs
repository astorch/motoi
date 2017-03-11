using System;

namespace Xcite.Csharp.lang {
    /// <summary>
    /// Provides various language associated method extensions.
    /// </summary>
    public static class LangExtensions {
        /// <summary>
        /// Returns a copy of the given func, that memorizes the calculated value after the first invocation. 
        /// If the function is invoked again, the memorized value is returned.
        /// </summary>
        /// <typeparam name="TValue">Type of the function result</typeparam>
        /// <param name="_func">Original function</param>
        /// <returns>Functions that uses an internal memory</returns>
        public static Func<TValue> Memorize<TValue>(this Func<TValue> _func) {
            bool hasValue = false;
            TValue result = default(TValue);
            Func<TValue> memFunc = () => {
                if (hasValue) return result;
                hasValue = true;
                return (result = _func());
            };
            return memFunc;
        } 
    }
}