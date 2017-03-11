using System;
using Xcite.Csharp.generics;

namespace Xcite.Csharp.exceptions {
    /// <summary>
    /// Provides methods to throw exceptions according to the Xcite exception pattern 
    /// more easily. The example shows the typical usage.
    /// </summary>
    /// <typeparam name="TErrorReason">Type of error reason to handle</typeparam>
    /// <typeparam name="TException">Type of exceptions to throw</typeparam>
    /// <code>
    /// public class Throw : ThrowBase&lt;MyErrorReason, MyException&gt; {
    ///  // No relevant content
    /// }
    /// public class OperatingObject {
    ///     public void DoSomething() {
    ///         Throw.Error(MyErrorReason.Unkown);
    ///     }
    /// }
    /// </code>
    public class ThrowBase<TErrorReason, TException>
        where TErrorReason : EErrorReason
        where TException : Xception<TErrorReason> {

        /// <summary>
        /// Throws a new exception of the defined type <typeparamref name="TException"/> 
        /// with the given <paramref name="errorReason"/>.
        /// </summary>
        /// <param name="errorReason">Error reason</param>
        public static void Error(TErrorReason errorReason) {
            throw typeof(TException).NewInstance<TException>(errorReason);
        }

        /// <summary>
        /// Throws a new exception of the defined type <typeparamref name="TException"/> 
        /// with the given <paramref name="errorReason"/> and <paramref name="message"/>.
        /// </summary>
        /// <param name="errorReason">Error reason</param>
        /// <param name="message">Error message</param>
        public static void Error(TErrorReason errorReason, string message) {
            throw typeof(TException).NewInstance<TException>(errorReason, message);
        }

        /// <summary>
        /// Throws a new exception of the defined type <typeparamref name="TException"/> 
        /// with the given <paramref name="errorReason"/>, <paramref name="message"/> and 
        /// <paramref name="preceedingException"/>. 
        /// </summary>
        /// <param name="errorReason">Error reason</param>
        /// <param name="message">Error message</param>
        /// <param name="preceedingException">Preceeding exception</param>
        public static void Error(TErrorReason errorReason, string message, Exception preceedingException) {
            throw typeof(TException).NewInstance<TException>(errorReason, message, preceedingException);
        }
    }
}