using motoi.platform.ui.bindings;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines a widget.
    /// </summary>
    public interface IWidget : IViewPart {

    }

    /// <summary>
    /// Provides the property meta data of <see cref="IWidget"/> that is used by data binding operations.
    /// </summary>
    public class PWidget<TControl> : BindableObject<TControl> where TControl : IDataBindingSupport {
        
    }
}