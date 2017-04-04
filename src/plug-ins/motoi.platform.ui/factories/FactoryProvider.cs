using System;
using System.Linq;
using log4net;
using motoi.extensions;
using motoi.extensions.core;
using motoi.plugins.model;
using Xcite.Csharp.generics;

namespace motoi.platform.ui.factories {
    /// <summary>
    /// Provides access to the registered UI Factories.
    /// </summary>
    public class FactoryProvider : GenericSingleton<FactoryProvider> {
        /// <summary>
        /// Extension Point id.
        /// </summary>
        private const string FactoryExtensionPointId = "org.motoi.ui.provider";

        private IUIProvider iUIProvider;
        private IShellFactory iShellFactory;
        private IWidgetFactory iWidgetFactory;
        private IUIServiceFactory iUIServiceFactory;
        private IApplicationController iApplicationController;

        /// <summary>
        /// Returns the controller of the UI platform application.
        /// The instance is created the first time it's required.
        /// </summary>
        /// <seealso cref="IApplicationController"/>
        /// <returns>UI platform application controller</returns>
        public IApplicationController GetApplicationController() {
            return iApplicationController ?? (iApplicationController = iUIProvider.GetApplicationController());
        }

        /// <summary>
        /// Returns the factory that is used to create UI services.
        /// /// The factory is created the first time it's required.
        /// </summary>
        /// <returns>Factory for creating services</returns>
        public IUIServiceFactory GetUIServiceFactory() {
            return iUIServiceFactory ?? (iUIServiceFactory = iUIProvider.GetUIServiceFactory());
        }

        /// <summary>
        /// Returns the factory that is used to create shells. 
        /// The factory is created the first time it's required.
        /// </summary>
        /// <seealso cref="IShell"/>
        /// <returns>Factory for creating shells</returns>
        public IShellFactory GetShellFactory() {
            return iShellFactory ?? (iShellFactory = iUIProvider.GetShellFactory());
        }

        /// <summary>
        /// Returns the factory that is used to create widgets (or compounds). 
        /// The factory is created the first time it's required.
        /// </summary>
        /// <seealso cref="IWidget"/>
        /// <seealso cref="IWidgetCompound"/>
        /// <returns>Factory for creating widgets or compounds</returns>
        public IWidgetFactory GetWidgetFactory() {
            return iWidgetFactory ?? (iWidgetFactory = iUIProvider.GetWidgetFactory());
        }

        /// <inheritdoc />
        protected override void OnInitialize() {
            ILog logWriter = LogManager.GetLogger(typeof(FactoryProvider));
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(FactoryExtensionPointId);
            if (configurationElements.Length == 0) throw new NullReferenceException("There is no registered UI provider!");

            if (configurationElements.Length > 1) {
                string[] viewPartFactories = configurationElements.Select(x => x.Id).ToArray();
                string enumeration = string.Join(", ", viewPartFactories);
                logWriter.Warn(string.Format("There is more than one UI provider registered: {0}", enumeration));
                logWriter.WarnFormat("Taking first one ({0})", configurationElements[0].Id);
                configurationElements = new[] {configurationElements[0]};
            }

            IConfigurationElement element = configurationElements[0];
            string id = element["id"];
            string className = element["class"];

            if (string.IsNullOrEmpty(className)) {
                string msg = string.Format("Class attribute of UI provider contribution '{0}' is NULL or empty", id);
                logWriter.Fatal(msg);
                throw new InvalidOperationException(msg);
            }

            try {
                IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(element);
                Type uiProviderType = TypeLoader.TypeForName(providingBundle, className);

                iUIProvider = uiProviderType.NewInstance<IUIProvider>();
            } catch (Exception ex) {
                string msg = string.Format("Error on creating UI provider instance of '{0}' contributed by '{1}'", className, id);
                logWriter.Fatal(msg);
                throw new InvalidOperationException(msg, ex);
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}