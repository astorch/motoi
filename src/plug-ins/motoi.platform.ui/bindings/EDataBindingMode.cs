using Xcite.Csharp.lang;

namespace motoi.platform.ui.bindings {
    /// <summary>
    /// Defines kinds of binding modes and resulting update behaviors.
    /// </summary>
    public class EDataBindingMode : XEnum<EDataBindingMode> {
        /// <summary>
        /// Indicates to use the property default binding mode. This depends of the bound property.
        /// </summary>
        public static readonly EDataBindingMode Default = new EDataBindingMode("Default");

        /// <summary>
        /// Updates the binding target when the binding source changes.
        /// </summary>
        public static readonly EDataBindingMode OneWay = new EDataBindingMode("OneWay");

        /// <summary>
        /// Updates the binding source when the binding target changes.
        /// </summary>
        public static readonly EDataBindingMode OneWayToSource = new EDataBindingMode("OneWayToSource");

        /// <summary>
        /// Updates the binding target when the binding source changes and vice versa.
        /// </summary>
        public static readonly EDataBindingMode TwoWay = new EDataBindingMode("TwoWay");

        /// <summary>
        /// Updates the binding target only one time when the binding is applied to the target.
        /// </summary>
        public static readonly EDataBindingMode OneTime = new EDataBindingMode("OneTime");

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">Unique identifier</param>
        private EDataBindingMode(string name) : base(name) {
            // Currently nothing to do here
        }
    }
}