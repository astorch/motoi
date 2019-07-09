using System.ComponentModel;
using motoi.platform.ui.bindings;

namespace motoi.platform.ui.toolbars {
    /// <summary> Defines base tool bar control. </summary>
    public interface IToolBarControl : INotifyPropertyChanged, IDataBindingSupport {
        /// <summary>
        /// Returns TRUE if the control is enabled or does set it.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Returns the current width of the control or does set it.
        /// </summary>
        int Width { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IToolBarControl"/>
    /// that is used by data binding operations.
    /// </summary>
    /// <typeparam name="TControl">Type of parent control</typeparam>
    public class PToolBarControl<TControl> : BindableObject<TControl> where TControl : IToolBarControl {
        /// <summary>IsEnabled property meta data</summary>
        public static readonly IBindableProperty<bool> IsEnabledProperty = CreatePropertyInfo(nameof(IToolBarControl.IsEnabled), true);
    }
}