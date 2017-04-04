using System;

namespace motoi.platform.ui.factories {
    /// <summary>
    /// Provides convenience methods to create instances of <see cref="IViewPart"/> or 
    /// <see cref="IWidget"/>.
    /// </summary>
    public static class UIFactory {
        /// <summary>
        /// Convenience method to create a type-safe widget of the given type.
        /// </summary>
        /// <typeparam name="TWidget">Type of the widget</typeparam>
        /// <returns>Instance or null</returns>
        public static TWidget NewWidget<TWidget>(IWidgetCompound composite) where TWidget : class, IWidget {
            if (composite == null) throw new ArgumentNullException("composite");
            return FactoryProvider.Instance.GetWidgetFactory().CreateInstance<TWidget>(composite);
        }

        /// <summary>
        /// Convenience method to create a type-safe shell off the given type.
        /// </summary>
        /// <typeparam name="TShell">Type of the shell</typeparam>
        /// <returns>Instance or null</returns>
        public static TShell NewShell<TShell>() where TShell : class, IShell {
            return FactoryProvider.Instance.GetShellFactory().CreateInstance<TShell>();
        }

        /// <summary>
        /// Convenience method to create a type-safe UI service of the given type.
        /// </summary>
        /// <typeparam name="TService">Type of the service</typeparam>
        /// <returns>Instance or NULL</returns>
        public static TService NewService<TService>() where TService : class, IUIService {
            return FactoryProvider.Instance.GetUIServiceFactory().GetService<TService>();
        }
    }
}