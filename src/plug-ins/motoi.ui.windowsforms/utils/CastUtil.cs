using System;

namespace motoi.ui.windowsforms.utils {
    /// <summary> Provides common methods for cast operations. </summary>
    public static class CastUtil {
        /// <summary>
        /// Casts the given object to the given type.
        /// If the cast is invalid an exception will be thrown.
        /// </summary>
        /// <typeparam name="TOut">Outgoing type</typeparam>
        /// <typeparam name="TIn">Incoming type</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static TOut Cast<TOut, TIn>(this TIn obj) 
            where TOut : class 
            where TIn : class {
            TOut castedObject = obj as TOut;
            if (castedObject == null) throw new InvalidCastException(string.Format("'{0}' cannot be casted to '{1}'!", typeof(TIn), typeof(TOut)));
            return castedObject;
        }

        /// <summary>
        /// Casts the given object to the given type.
        /// If the cast is invalid an exception will be thrown.
        /// </summary>
        /// <typeparam name="TOut">Outgoing type</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static TOut Cast<TOut>(this object obj) where TOut : class {
            return Cast<TOut, object>(obj);
        }
    }
}