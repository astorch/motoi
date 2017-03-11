using System;
using System.Linq;
using motoi.extensions;
using motoi.extensions.core;
using motoi.plugins.model;
using Xcite.Csharp.generics;

namespace motoi.platform.ui {
    /// <summary>
    /// Provides access to the registered UI Factories.
    /// </summary>
    public static class FactoryProvider {
        /// <summary>
        /// Extension Point id.
        /// </summary>
        private const string FactoryExtensionPointId = "org.motoi.ui.provider";

        private static IViewPartFactory iViewPartFactory;
        private static IWidgetFactory iWidgetFactory;
        private static IDataBindingFactory iDataBindingFactory;

        /// <summary>
        /// Returns the factory that shall be used to create view parts.
        /// </summary>
        /// <returns>Factory for creating View Parts</returns>
        /// <exception cref="NullReferenceException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IViewPartFactory GetViewPartFactory() {
            if (iViewPartFactory != null) return iViewPartFactory;
            Initialize();
            return iViewPartFactory;
        }

        /// <summary>
        /// Returns the factory that shall be used to create widgets.
        /// </summary>
        /// <returns>Factory for creating widgets</returns>
        /// <exception cref="NullReferenceException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IWidgetFactory GetWidgetFactory() {
            if (iWidgetFactory != null) return iWidgetFactory;
            Initialize();
            return iWidgetFactory;
        }

        /// <summary>
        /// Returns the factory that shall be used to create data bindings.
        /// </summary>
        /// <returns></returns>
        public static IDataBindingFactory GetDataBindingFactory() {
            if (iDataBindingFactory != null) return iDataBindingFactory;
            Initialize();
            return iDataBindingFactory;
        }

        /// <summary>
        /// Initializes and creates the factories.
        /// </summary>
        private static void Initialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(FactoryExtensionPointId);

            if (configurationElements.Length == 0) throw new NullReferenceException("There is no registered ViewPartFactory!");

            if (configurationElements.Length > 1) {
                string[] viewPartFactories = configurationElements.Select(x => x.Id).ToArray();
                string enumeration = string.Join(", ", viewPartFactories);
                throw new InvalidOperationException(string.Format("There is more than one ViewPartFactory registered! {0}", enumeration));
            }

            IConfigurationElement element = configurationElements[0];
            string id = element["id"];
            string viewPartFactoryClassName = element["viewPartFactory"];
            string widgetFactoryClassName = element["widgetFactory"];
            string dataBindingFactoryClassName = element["dataBindingFactory"];

            if (string.IsNullOrEmpty(viewPartFactoryClassName)) throw new NullReferenceException(string.Format("Class attribute of ViewPartFactory definition is null or empty! Id: '{0}'", id));
            if (string.IsNullOrEmpty(widgetFactoryClassName)) throw new NullReferenceException(string.Format("Class attribute of WidgetFactory definition is null or empty! Id: '{0}'", id));
            if (string.IsNullOrEmpty(dataBindingFactoryClassName)) throw new NullReferenceException(string.Format("Class attribute of DataBindingFactory definition is null or empty! Id: '{0}'", id));

            IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(element);
            Type viewPartFactoryType = TypeLoader.TypeForName(providingBundle, viewPartFactoryClassName);
            Type widgetFactoryType = TypeLoader.TypeForName(providingBundle, widgetFactoryClassName);
            Type dataBindingFactoryType = TypeLoader.TypeForName(providingBundle, dataBindingFactoryClassName);

            iViewPartFactory = viewPartFactoryType.NewInstance<IViewPartFactory>();
            iWidgetFactory = widgetFactoryType.NewInstance<IWidgetFactory>();
            iDataBindingFactory = dataBindingFactoryType.NewInstance<IDataBindingFactory>();
        }
    }
}