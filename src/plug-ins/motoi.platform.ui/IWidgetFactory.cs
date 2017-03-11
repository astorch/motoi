using motoi.platform.ui.widgets;

namespace motoi.platform.ui {
    /// <summary>
    /// Defines the interface of a Widget Factory.
    /// </summary>
    public interface IWidgetFactory {
        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TWidget">Type of the widget to create</typeparam>
        /// <param name="composite">Parent element that contains this widget</param>
        /// <returns>Newly created instance of the given widget type</returns>
        TWidget CreateInstance<TWidget>(IViewPartComposite composite) where TWidget : class, IWidget;
    }
}