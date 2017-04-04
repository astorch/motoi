namespace motoi.platform.ui.factories {
    /// <summary>
    /// Defines a factory for widgets.
    /// </summary>
    public interface IWidgetFactory {
        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TWidget">Type of the widget to create</typeparam>
        /// <param name="widgetCompound">Parent element that contains this widget</param>
        /// <returns>Newly created instance of the given widget type</returns>
        TWidget CreateInstance<TWidget>(IWidgetCompound widgetCompound) where TWidget : class, IWidget;
    }
}