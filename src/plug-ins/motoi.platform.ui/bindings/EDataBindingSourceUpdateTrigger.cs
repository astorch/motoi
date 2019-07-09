using xcite.csharp;

namespace motoi.platform.ui.bindings {
    /// <summary> Defines kinds of trigger when the binding source is updated. </summary>
    public class EDataBindingSourceUpdateTrigger : XEnum<EDataBindingSourceUpdateTrigger> {
        /// <summary> Indicates to use a default update trigger. This depends on the bound property. </summary>
        public static readonly EDataBindingSourceUpdateTrigger Default = new EDataBindingSourceUpdateTrigger("Default");

        /// <summary> Updates the binding source when the UpdateSource() method is invoked. </summary>
        public static readonly EDataBindingSourceUpdateTrigger Explicit = new EDataBindingSourceUpdateTrigger("Explicit");

        /// <summary> Updates the binding source when the binding target loses focus. </summary>
        public static readonly EDataBindingSourceUpdateTrigger LostFocus = new EDataBindingSourceUpdateTrigger("LostFocus");

        /// <summary> Updates the binding source when the binding target property changes. </summary>
        public static readonly EDataBindingSourceUpdateTrigger PropertyChanged = new EDataBindingSourceUpdateTrigger("PropertyChanged");

        /// <summary> Updates the binding source when the binding target property changes but not every time. This reduces frequent invocations. </summary>
        public static readonly EDataBindingSourceUpdateTrigger Delayed = new EDataBindingSourceUpdateTrigger("Delayed");
        
        /// <summary> Creates a new instance. </summary>
        /// <param name="name">Unique identifier</param>
        private EDataBindingSourceUpdateTrigger(string name) : base(name) {
            // Currently nothing to do here
        }
    }
}