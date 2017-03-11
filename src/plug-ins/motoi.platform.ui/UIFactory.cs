using System;
using motoi.platform.ui.widgets;

namespace motoi.platform.ui {
    /// <summary>
    /// Provides convenience methods to create instances of <see cref="IViewPart"/> or 
    /// <see cref="IWidget"/>.
    /// </summary>
    public static class UIFactory {
        /// <summary>
        /// Convenience method to create a type safe widget of the given type.
        /// </summary>
        /// <typeparam name="TWidget">Type of the widget</typeparam>
        /// <returns>Instance or null</returns>
        public static TWidget NewWidget<TWidget>(IViewPartComposite composite) where TWidget : class, IWidget {
            if (composite == null) throw new ArgumentNullException("composite");
            TWidget widget = FactoryProvider.GetWidgetFactory().CreateInstance<TWidget>(composite);
            return widget;
        }

        /// <summary>
        /// Convenience method to create a type safe view part of the given type.
        /// </summary>
        /// <typeparam name="TViewPart">Type of the view part</typeparam>
        /// <returns>Instance or null</returns>
        public static TViewPart NewViewPart<TViewPart>() where TViewPart : class, IViewPart {
            TViewPart viewPart = FactoryProvider.GetViewPartFactory().CreateInstance<TViewPart>();
            return viewPart;
        }
    }
}