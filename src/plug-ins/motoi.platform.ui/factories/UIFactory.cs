using System;

namespace motoi.platform.ui.factories {
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
            TWidget widget = FactoryProvider.Instance.GetWidgetFactory().CreateInstance<TWidget>(composite);
            return widget;
        }

        /// <summary>
        /// Convenience method to create a type safe shell off the given type.
        /// </summary>
        /// <typeparam name="TShell">Type of the shell</typeparam>
        /// <returns>Instance or null</returns>
        public static TShell NewShell<TShell>() where TShell : class, IShell {
            TShell viewPart = FactoryProvider.Instance.GetShellFactory().CreateInstance<TShell>();
            return viewPart;
        }
    }
}