using System;
using motoi.platform.ui;
using motoi.platform.ui.widgets;
namespace Motoi.UI.WPF {
    /// <summary>
    /// Provides an implementation of <see cref="IWidgetFactory"/>.
    /// </summary>
    public class WidgetFactory : IWidgetFactory {
        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TWidget">Type of the widget to create</typeparam>
        /// <param name="composite">Parent element that contains this widget</param>
        /// <returns>Newly created instance of the given widget type</returns>
        public TWidget CreateInstance<TWidget>(IViewPartComposite composite) where TWidget : class, IWidget
        {
            Type widgetTpe = typeof (TWidget);
            return null;
        }
    }
}