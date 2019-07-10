using System;
using System.Collections.Generic;
using motoi.extensions;
using motoi.plugins;
using xcite.csharp;
using xcite.logging;

namespace motoi.workbench.propertyview {
    /// <summary>
    /// Provides a registry of contributed implementations of <see cref="IPropertyViewItemsProvider"/>.
    /// </summary>
    public class PropertyViewItemsProviderRegistry : GenericSingleton<PropertyViewItemsProviderRegistry> {
        /// <summary> Extension point id. </summary>
        private const string ExtensionPointId = "org.motoi.workbench.propertyview.provider";

        private readonly ILog _log = LogManager.GetLog(typeof(PropertyViewItemsProviderRegistry));

        private Dictionary<string, IPropertyViewItemsProvider> _registeredProviders;

        /// <summary>
        /// Returns the provider that has been registered for the given <paramref name="fileExtension"/>. If there 
        /// is no provider, NULL is returned.
        /// </summary>
        /// <param name="fileExtension">File extension a provider may associated with</param>
        /// <returns>An instance of <see cref="IPropertyViewItemsProvider"/> or NULL</returns>
        public IPropertyViewItemsProvider GetProvider(string fileExtension) {
            if (_registeredProviders.TryGetValue(fileExtension, out IPropertyViewItemsProvider provider)) return provider;
            return null;
        }

        /// <inheritdoc />
        protected override void OnInitialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointId);
            _log.Info($"{configurationElements.Length} registered provider found");

            _registeredProviders = new Dictionary<string, IPropertyViewItemsProvider>(configurationElements.Length);

            for (int i = -1; ++i != configurationElements.Length;) {
                IConfigurationElement configurationElement = configurationElements[i];
                IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(configurationElement);
                string fileExtension = configurationElement["fileExtension"];
                string cls = configurationElement["class"];

                if (string.IsNullOrEmpty(fileExtension)) {
                    _log.Error($"Bundle '{providingBundle}' provides a property view items provider with an empty file extension");
                    continue;
                }

                if (string.IsNullOrEmpty(cls)) {
                    _log.Error($"Bundle '{providingBundle}' provides a property view items provider with an empty class reference");
                    continue;
                }


                IPropertyViewItemsProvider provider;
                try {
                    Type providerType = TypeLoader.TypeForName(providingBundle, cls);
                    provider = providerType.NewInstance<IPropertyViewItemsProvider>();
                } catch (Exception ex) {
                    _log.Error($"Error on creating instance of '{cls}' provided by bundle '{providingBundle}'.", ex);
                    continue;
                }

                try {
                    _registeredProviders.Add(fileExtension, provider);
                    _log.Info($"Provider '{provider.GetType()}' ({providingBundle}) successfully registered for file extension '{fileExtension}'");
                } catch (Exception ex) {
                    _log.Error($"Error on registering '{provider.GetType()}' ({providingBundle}) as provider for file extension '{fileExtension}'.", ex);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            _registeredProviders.Clear();
            _registeredProviders = null;
        }
    }
}