using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using motoi.extensions;
using motoi.extensions.core;
using motoi.platform.commons;
using motoi.plugins.model;
using xcite.csharp;

namespace motoi.platform.nls {
    /// <summary> Implements the registry for NLS contributions. </summary>
    public class NLSRegistry : GenericSingleton<NLSRegistry> {
        private const string ExtensionPointId = "org.motoi.platform.localization";
        private readonly ILog fLogWriter = LogManager.GetLogger(typeof(NLSRegistry));
        private readonly Dictionary<string, LocalizationContribution> fRegistry = new Dictionary<string, LocalizationContribution>(97);

        /// <summary>
        /// Returns the provider type of the specified <paramref name="localizationId"/>. If no provider type 
        /// is known, NULL is returned. If <paramref name="localizationId"/> is NULL or empty, NULL is returned, too.
        /// </summary>
        /// <param name="localizationId">Localization id</param>
        /// <returns>Provider type or NULL</returns>
        public Type GetProviderType(string localizationId) {
            if (string.IsNullOrEmpty(localizationId)) return null;

            LocalizationContribution localizationContribution;
            if (fRegistry.TryGetValue(localizationId, out localizationContribution)) return localizationContribution.ProviderType;
            return null;
        }

        /// <summary>
        /// Returns the resource file for the specified <paramref name="localizationId"/> according to the 
        /// specified <paramref name="language"/>. If <paramref name="language"/> is NULL or <see cref="string.Empty"/> 
        /// the default resource file is returned, if one exists. Otherwise, the file is looked up using the following 
        /// scheme: "{registered_resource_path}_{language}.txt". The {registered_resource_path} must have been provided by 
        /// an extension point. <br/>
        /// If <paramref name="localizationId"/> is NULL or empty, NULL is returned. If none resource file has been found, 
        /// NULL is returned, too.
        /// </summary>
        /// <param name="localizationId">Localization id</param>
        /// <param name="language">Language</param>
        /// <returns>Resource file or NULL</returns>
        public string GetResourceFile(string localizationId, string language) {
            if (string.IsNullOrEmpty(localizationId)) return null;
            language = language ?? string.Empty;

            IEnumerable<LocalizationContribution> locContrSet = fRegistry.Values.Where(c => c.LocalizationId == localizationId);
            using (IEnumerator<LocalizationContribution> locContrSetItr = locContrSet.GetEnumerator()) {
                while (locContrSetItr.MoveNext()) {
                    LocalizationContribution contr = locContrSetItr.Current;

                    using (IEnumerator<ResourcePathReference> contrItr = contr.Sources.GetEnumerator()) {
                        while (contrItr.MoveNext()) {
                            ResourcePathReference fileContr = contrItr.Current;
                            Assembly assembly = fileContr.Assembly;
                            string resourcePath = fileContr.ResourcePath;
                            string lnk = string.IsNullOrEmpty(language) ? string.Empty : "_";
                            string nlsResourceFile = $"{resourcePath}{lnk}{language}.txt";
    
                            using (Stream stream = ResourceLoader.OpenStream(assembly, nlsResourceFile)) {
                                if (stream == null) continue;
                                using (StreamReader streamReader = new StreamReader(stream)) {
                                    return streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <inheritdoc />
        protected override void OnInitialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointId);
            for (int i = -1; ++i != configurationElements.Length;) {
                IConfigurationElement element = configurationElements[i];
                IBundle bundle = ExtensionService.Instance.GetProvidingBundle(element);

                string id = element.Id;
                string providerType = element["provider"];
                string resourcePath = element["resourcePath"];
                if (string.IsNullOrEmpty(id)) {
                    fLogWriter.ErrorFormat("Bundle '{0}' declares a localization contribution with no id", bundle);
                    continue;
                }

                if (string.IsNullOrEmpty(providerType)) {
                    fLogWriter.ErrorFormat("Bundle '{0}' declares the localization contribution '{1}' with no provider type", bundle, id);
                    continue;
                }

                if (string.IsNullOrEmpty(resourcePath)) {
                    fLogWriter.ErrorFormat("Bundle '{0}' declares the localization contribution '{1}' with an empty resource path", bundle, id);
                    continue;
                }

                LocalizationContribution localizationContribution;
                if (!fRegistry.TryGetValue(id, out localizationContribution)) {
                    Type provider;
                    try {
                        provider = TypeLoader.TypeForName(bundle, providerType);
                    } catch (Exception ex) {
                        fLogWriter.Error($"Error on resolving type of '{providerType}' provided by bundle '{bundle}' for localization contribution '{id}'", ex);
                        continue;
                    }

                    localizationContribution = new LocalizationContribution(id, provider);
                    fRegistry.Add(id, localizationContribution);
                }

                ResourcePathReference resourcePathReference = new ResourcePathReference(localizationContribution.ProviderType.Assembly, resourcePath);
                localizationContribution.Sources.Add(resourcePathReference);
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            fRegistry.Clear();
        }

        /// <summary> Describes a localization contribution. </summary>
        class LocalizationContribution {
            /// <summary>
            /// Creates a new instance with the given values.
            /// </summary>
            /// <param name="localizationId">Localization id</param>
            /// <param name="providerType">Provider type</param>
            /// <exception cref="ArgumentNullException">If any argument is NULL or empty</exception>
            public LocalizationContribution(string localizationId, Type providerType) {
                if (string.IsNullOrEmpty(localizationId)) throw new ArgumentNullException(nameof(localizationId));
                ProviderType = providerType ?? throw new ArgumentNullException(nameof(providerType));
                LocalizationId = localizationId;
                Sources = new List<ResourcePathReference>(3);
            }

            /// <summary> Returns the localization id. </summary>
            public string LocalizationId { get; private set; }

            /// <summary> Returns the provider type. </summary>
            public Type ProviderType { get; private set; }

            /// <summary> Returns the set of resource path references. </summary>
            public IList<ResourcePathReference> Sources { get; private set; }
        }

        /// <summary> Describes a resource path reference. </summary>
        class ResourcePathReference {
            /// <summary>
            /// Creates a new instance with the given values.
            /// </summary>
            /// <param name="assembly">Assembly the provides the resources</param>
            /// <param name="resourcePath">Path of the resources within the assembly</param>
            public ResourcePathReference(Assembly assembly, string resourcePath) {
                if (string.IsNullOrEmpty(resourcePath)) throw new ArgumentNullException(nameof(resourcePath));
                Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
                ResourcePath = resourcePath;
            }

            /// <summary> Returns the assembly that provides the resources. </summary>
            public Assembly Assembly { get; private set; }

            /// <summary> Returns the resource path. </summary>
            public string ResourcePath { get; private set; }
        }
    }
}