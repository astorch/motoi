using System;
using System.Linq;
using motoi.extensions;
using motoi.platform.resources.model.preference;
using motoi.plugins;
using NLog;
using xcite.csharp;

namespace motoi.platform.resources.runtime.preference {
    /// <summary>
    /// Provides access to the preference stores.
    /// </summary>
    public class PreferenceStoreManager  {
        private const string PreferenceStoreManagerExtensionPointId = "org.motoi.resources.preferenceStoreManager";
        private static IPreferenceStoreManager _preferenceStoreManager;

        /// <summary>
        /// Returns the currently used <see cref="IPreferenceStoreManager"/>.
        /// </summary>
        /// <returns>Currently used <see cref="IPreferenceStoreManager"/></returns>
        public static IPreferenceStoreManager GetInstance() {
            return _preferenceStoreManager ?? (_preferenceStoreManager = CreatePreferenceStoreManager());
        }

        /// <summary>
        /// Returns always a new instance of <see cref="FilePreferenceStoreManager"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="FilePreferenceStoreManager"/></returns>
        private static IPreferenceStoreManager GetDefaultPreferenceStoreManager() {
            return new FilePreferenceStoreManager();
        }

        /// <summary>
        /// Creates an instance of <see cref="IPreferenceStoreManager"/>. First, it is looked up if a custom implementation 
        /// is provided by an extension point. If not, a default implementation is returned. Otherwise the contribution with 
        /// the highest priority is taken. There is always just one implementation of <see cref="IPreferenceStoreManager"/>.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IPreferenceStoreManager"/> either provided by an extension point or the 
        /// default implementation.
        /// </returns>
        private static IPreferenceStoreManager CreatePreferenceStoreManager() {
            Logger logWriter = LogManager.GetCurrentClassLogger(typeof(PreferenceStoreManager));
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(PreferenceStoreManagerExtensionPointId);
            
            // Default
            if (configurationElements == null || configurationElements.Length == 0) {
                logWriter.Info("No preference store manager contributed. Using default implementation.");
                return GetDefaultPreferenceStoreManager();
            }
            
            // Max priority
            IConfigurationElement maxPriorityElement = configurationElements.OrderByDescending(element => {
                string priorityValue = element["priority"];
                int priority;
                if (!int.TryParse(priorityValue, out priority)) return 0;
                return priority;
            }).First();

            // Grab the bundle
            IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(maxPriorityElement);
            logWriter.Debug($"Plug-in '{providingBundle}' provides a custom preference store manager implementation with highest priority.");

            // Grab the type
            string className = maxPriorityElement["class"];
            if (string.IsNullOrEmpty(className)) {
                logWriter.Error($"Plug-in '{providingBundle}' provides a custom preference store manager implementation with an empty " +
                                 "class attribute. Using default implementation.");
                return GetDefaultPreferenceStoreManager();
            }

            // Allocate
            try {
                Type instanceType = TypeLoader.TypeForName(providingBundle, className);
                IPreferenceStoreManager preferenceStoreManager = instanceType.NewInstance<IPreferenceStoreManager>();
                logWriter.Info($"Preference store manager of type '{preferenceStoreManager.GetType()}' successfully installed");
                return preferenceStoreManager;
            } catch (Exception ex) {
                logWriter.Error(ex, $"Error on installing preference store manager from plug-in '{providingBundle}' (Class '{className}').");
                return GetDefaultPreferenceStoreManager();
            }
        }
    }
}