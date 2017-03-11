using Xcite.Csharp.lang;

namespace motoi.platform.ui.bindings {
    /// <summary>
    /// Defines kinds of binding source update reasons.
    /// </summary>
    public class EBindingSourceUpdateReason : XEnum<EBindingSourceUpdateReason> {
        /// <summary>
        /// Update must be processed due to a property change.
        /// </summary>
        public static readonly EBindingSourceUpdateReason PropertyChanged = new EBindingSourceUpdateReason("PropertyChanged");

        /// <summary>
        /// Update must be processed due to a lost focus event.
        /// </summary>
        public static readonly EBindingSourceUpdateReason LostFocus = new EBindingSourceUpdateReason("LostFocus");

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">Unique identifier</param>
        private EBindingSourceUpdateReason(string name) : base(name) {
            // Currently nothing to do here
        }
    }
}