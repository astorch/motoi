namespace Xcite.Csharp.lang {
    /// <summary>
    /// Defines an extended C# base object class.
    /// </summary>
    public class XObject {
        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() {
            string toString = string.Format("{0}@{1}", GetHashCode().ToString("X4"), GetType().FullName);
            return toString;
        }
    }
}