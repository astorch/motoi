using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using motoi.extensions;
using motoi.platform.nls;
using motoi.plugins;
using motoi.platform.ui.images;
using motoi.workbench.model;
using NLog;
using xcite.collections;
using xcite.csharp;

namespace motoi.workbench.stub.registries {
    /// <summary> Implements a registry that manages all New Wizard contributions. </summary>
    class NewWizardRegistry : GenericSingleton<NewWizardRegistry> {

        /// <summary> Extension point id. </summary>
        private const string ExtensionPointId = "org.motoi.workbench.stub.wizards.newWizard";

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary> Collection of resolved contributions. </summary>
        private readonly LinearList<Contribution> iContributions = new LinearList<Contribution>();

        /// <summary> Returns all registered contributions. </summary>
        public Contribution[] Contributions 
            => iContributions.ToArray();

        /// <inheritdoc />
        protected override void OnInitialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointId);

            IConfigurationElement[] categories = Enumerable.ToArray(configurationElements.Where(el => el.Prefix == "category"));
            IConfigurationElement[] wizards = Enumerable.ToArray(configurationElements.Where(el => el.Prefix == "wizard"));
            IDictionary<string, CategoryContribution> idToCategoryMap = new Dictionary<string, CategoryContribution>(categories.Length);

            // Processing categories
            for (int i = -1; ++i < categories.Length; ) {
                IConfigurationElement category = categories[i];
                string id = category["id"];
                string label = category["label"];

                // NLS support
                if (label.StartsWith("%")){
                    string nlsKey = label.Substring(1);
                    string assemblyName = ExtensionService.Instance.GetProvidingBundle(category).Name;
                    // We cannot use the AppDomain here, because the assembly must not have been loaded yet
                    Assembly assembly = Assembly.Load(assemblyName); // TODO Check if this may be an issue
                    string localizationId = NLS.GetLocalizationId(assembly);
                    label = NLS.GetText(localizationId, nlsKey);
                }

                CategoryContribution contribution = new CategoryContribution { Id = id, Label = label };
                idToCategoryMap.Add(id, contribution);
                iContributions.Add(contribution);
            }

            // Processing wizards
            for (int i = -1; ++i < wizards.Length; ) {
                IConfigurationElement wizard = wizards[i];
                string id = wizard["id"];
                string category = wizard["category"];
                string label = wizard["label"];
                string className = wizard["class"];
                string imagePath = wizard["image"];

                if (string.IsNullOrEmpty(className)) continue;

                IWizard wizardImpl;

                try {
                    IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(wizard);
                    Type wizardType = TypeLoader.TypeForName(providingBundle, className);
                    wizardImpl = wizardType.NewInstance<IWizard>();
                } catch (Exception ex) {
                    _log.Error(ex, $"Error on creating wizard of type '{className}'.");
                    continue;
                }

                ImageDescriptor imageDescriptor = null;
                if (!string.IsNullOrEmpty(imagePath)) {
                    try {
                        imageDescriptor = ImageDescriptor.Create(string.Format("image.{0}", id), wizardImpl.GetType().Assembly, imagePath);
                    } catch (Exception ex) {
                        _log.Error(ex, $"Error on resolving image of wizard '{id}'.");
                    }
                }

                // NLS support
                label = NLS.Localize(label, wizardImpl);

                WizardContribution contribution = new WizardContribution { Id = id, Label = label, Category = category, Wizard = wizardImpl, Image = imageDescriptor};

                // Is it a categorized item?
                if (!string.IsNullOrEmpty(category)) {
                    if (idToCategoryMap.TryGetValue(category, out CategoryContribution categoryContr)) {
                        categoryContr.Wizards.Add(contribution);
                        continue;
                    }
                }

                iContributions.Add(contribution);
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}
