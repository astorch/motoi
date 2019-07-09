using System;
using System.Linq;
using motoi.extensions;
using motoi.plugins;
using xcite.csharp;
using xcite.logging;

namespace motoi.platform.ui.factories {
    /// <summary> Provides access to the registered UI Factories. </summary>
    public class FactoryProvider : GenericSingleton<FactoryProvider> {
        /// <summary> Extension Point id. </summary>
        private const string FactoryExtensionPointId = "org.motoi.ui.provider";

        private IUIProvider _uiProvider;
        private IShellFactory _shellFactory;
        private IWidgetFactory _widgetFactory;
        private IUIServiceFactory _uiServiceFactory;
        private IApplicationController _applicationController;

        /// <summary>
        /// Returns the controller of the UI platform application.
        /// The instance is created the first time it's required.
        /// </summary>
        /// <seealso cref="IApplicationController"/>
        /// <returns>UI platform application controller</returns>
        public IApplicationController GetApplicationController() {
            return _applicationController ?? (_applicationController = _uiProvider.GetApplicationController());
        }

        /// <summary>
        /// Returns the factory that is used to create UI services.
        /// /// The factory is created the first time it's required.
        /// </summary>
        /// <returns>Factory for creating services</returns>
        public IUIServiceFactory GetUIServiceFactory() {
            return _uiServiceFactory ?? (_uiServiceFactory = _uiProvider.GetUIServiceFactory());
        }

        /// <summary>
        /// Returns the factory that is used to create shells. 
        /// The factory is created the first time it's required.
        /// </summary>
        /// <seealso cref="IShell"/>
        /// <returns>Factory for creating shells</returns>
        public IShellFactory GetShellFactory() {
            return _shellFactory ?? (_shellFactory = _uiProvider.GetShellFactory());
        }

        /// <summary>
        /// Returns the factory that is used to create widgets (or compounds). 
        /// The factory is created the first time it's required.
        /// </summary>
        /// <seealso cref="IWidget"/>
        /// <seealso cref="IWidgetCompound"/>
        /// <returns>Factory for creating widgets or compounds</returns>
        public IWidgetFactory GetWidgetFactory() {
            return _widgetFactory ?? (_widgetFactory = _uiProvider.GetWidgetFactory());
        }

        /// <inheritdoc />
        protected override void OnInitialize() {
            ILog logWriter = LogManager.GetLog(typeof(FactoryProvider));
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(FactoryExtensionPointId);
            if (configurationElements.Length == 0) throw new InvalidOperationException("There is no registered UI provider!");

            if (configurationElements.Length > 1) {
                string[] viewPartFactories = configurationElements.Select(x => x.Id).ToArray();
                string enumeration = string.Join(", ", viewPartFactories);
                logWriter.Warning($"There is more than one UI provider registered: {enumeration}");
                logWriter.Warning($"Taking first one ({configurationElements[0].Id})");
                configurationElements = new[] {configurationElements[0]};
            }

            IConfigurationElement element = configurationElements[0];
            string id = element["id"];
            string className = element["class"];

            if (string.IsNullOrEmpty(className)) {
                string msg = $"Class attribute of UI provider contribution '{id}' is NULL or empty";
                logWriter.Fatal(msg);
                throw new InvalidOperationException(msg);
            }

            try {
                IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(element);
                Type uiProviderType = TypeLoader.TypeForName(providingBundle, className);

                _uiProvider = uiProviderType.NewInstance<IUIProvider>();
            } catch (Exception ex) {
                string msg = $"Error on creating UI provider instance of '{className}' contributed by '{id}'";
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