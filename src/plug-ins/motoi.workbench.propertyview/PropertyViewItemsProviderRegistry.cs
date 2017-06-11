using System;
using System.Collections.Generic;
using log4net;
using motoi.extensions;
using motoi.extensions.core;
using motoi.plugins.model;
using xcite.csharp;

namespace motoi.workbench.propertyview {
    /// <summary>
    /// Provides a registry of contributed implementations of <see cref="IPropertyViewItemsProvider"/>.
    /// </summary>
    public class PropertyViewItemsProviderRegistry : GenericSingleton<PropertyViewItemsProviderRegistry> {
        /// <summary> Extension point id. </summary>
        private const string ExtensionPointId = "org.motoi.workbench.propertyview.provider";

        private readonly ILog iLogWriter = LogManager.GetLogger(typeof(PropertyViewItemsProviderRegistry));

        private Dictionary<string, IPropertyViewItemsProvider> iRegisteredProviders;

        /// <summary>
        /// Returns the provider that has been registered for the given <paramref name="fileExtension"/>. If there 
        /// is no provider, NULL is returned.
        /// </summary>
        /// <param name="fileExtension">File extension a provider may associated with</param>
        /// <returns>An instance of <see cref="IPropertyViewItemsProvider"/> or NULL</returns>
        public IPropertyViewItemsProvider GetProvider(string fileExtension) {
            IPropertyViewItemsProvider provider;
            if (iRegisteredProviders.TryGetValue(fileExtension, out provider)) return provider;
            return null;
        }

        /// <inheritdoc />
        protected override void OnInitialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointId);
            iLogWriter.InfoFormat("{0} registered provider found", configurationElements.Length);

            iRegisteredProviders = new Dictionary<string, IPropertyViewItemsProvider>(configurationElements.Length);

            for (int i = -1; ++i != configurationElements.Length;) {
                IConfigurationElement configurationElement = configurationElements[i];
                IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(configurationElement);
                string fileExtension = configurationElement["fileExtension"];
                string cls = configurationElement["class"];

                if (string.IsNullOrEmpty(fileExtension)) {
                    iLogWriter.ErrorFormat("Bundle '{0}' provides a property view items provider with an empty file extension", providingBundle);
                    continue;
                }

                if (string.IsNullOrEmpty(cls)) {
                    iLogWriter.ErrorFormat("Bundle '{0}' provides a property view items provider with an empty class reference", providingBundle);
                    continue;
                }


                IPropertyViewItemsProvider provider;
                try {
                    Type providerType = TypeLoader.TypeForName(providingBundle, cls);
                    provider = providerType.NewInstance<IPropertyViewItemsProvider>();
                } catch (Exception ex) {
                    iLogWriter.ErrorFormat("Error on creating instance of '{0}' provided by bundle '{1}'. Reason: {2}", 
                        cls, providingBundle, ex);
                    continue;
                }

                try {
                    iRegisteredProviders.Add(fileExtension, provider);
                    iLogWriter.InfoFormat("Provider '{0}' ({1}) successfully registered for file extension '{2}'",
                        provider.GetType(), providingBundle, fileExtension);
                } catch (Exception ex) {
                    iLogWriter.ErrorFormat("Error on registering '{0}' ({1}) as provider for file extension '{2}'. Reason: {3}",
                        provider.GetType(), providingBundle, fileExtension, ex);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            iRegisteredProviders.Clear();
            iRegisteredProviders = null;
        }
    }
}