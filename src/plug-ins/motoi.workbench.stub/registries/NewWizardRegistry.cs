using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using motoi.extensions;
using motoi.platform.nls;
using motoi.plugins;
using motoi.platform.ui.images;
using motoi.workbench.model;
using xcite.csharp;
using xcite.logging;

namespace motoi.workbench.stub.registries {
    /// <summary> Implements a registry that manages all New Wizard contributions. </summary>
    class NewWizardRegistry : GenericSingleton<NewWizardRegistry> {

        /// <summary> Extension point id. </summary>
        private const string ExtensionPointId = "org.motoi.workbench.stub.wizards.newWizard";

        private static readonly ILog _log = LogManager.GetLog(typeof(NewWizardRegistry));

        /// <summary> Collection of resolved contributions. </summary>
        private readonly List<Contribution> _contributions = new List<Contribution>(30);

        /// <summary> Returns all registered contributions. </summary>
        public Contribution[] Contributions 
            => _contributions.ToArray();

        /// <inheritdoc />
        protected override void OnInitialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointId);

            IConfigurationElement[] categories = configurationElements.Where(el => el.Prefix == "category").ToArray();
            IConfigurationElement[] wizards = configurationElements.Where(el => el.Prefix == "wizard").ToArray();
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
                _contributions.Add(contribution);
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
                    _log.Error($"Error on creating wizard of type '{className}'.", ex);
                    continue;
                }

                ImageDescriptor imageDescriptor = null;
                if (!string.IsNullOrEmpty(imagePath)) {
                    try {
                        imageDescriptor = ImageDescriptor.Create($"image.{id}", wizardImpl.GetType().Assembly, imagePath);
                    } catch (Exception ex) {
                        _log.Error($"Error on resolving image of wizard '{id}'.", ex);
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

                _contributions.Add(contribution);
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}
