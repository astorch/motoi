using System;
using System.Linq;
using log4net;
using motoi.extensions;
using motoi.extensions.core;
using motoi.platform.resources.model.preference;
using motoi.plugins.model;
using Xcite.Csharp.generics;

namespace motoi.platform.resources.runtime.preference {
    /// <summary>
    /// Provides access to the preference stores.
    /// </summary>
    public class PreferenceStoreManager  {
        private const string PreferenceStoreManagerExtensionPointId = "org.motoi.resources.preferenceStoreManager";
        private static IPreferenceStoreManager iPreferenceStoreManager;

        /// <summary>
        /// Returns the currently used <see cref="IPreferenceStoreManager"/>.
        /// </summary>
        /// <returns>Currently used <see cref="IPreferenceStoreManager"/></returns>
        public static IPreferenceStoreManager GetInstance() {
            return iPreferenceStoreManager ?? (iPreferenceStoreManager = CreatePreferenceStoreManager());
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
            ILog logWriter = LogManager.GetLogger(typeof(PreferenceStoreManager));
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
            logWriter.DebugFormat("Plug-in '{0}' provides a custom preference store manager implementation with highest priority.", 
                providingBundle);

            // Grab the type
            string className = maxPriorityElement["class"];
            if (string.IsNullOrEmpty(className)) {
                logWriter.ErrorFormat("Plug-in '{0}' provides a custom preference store manager implementation with an empty " +
                                      "class attribute. Using default implementation.", providingBundle);
                return GetDefaultPreferenceStoreManager();
            }

            // Allocate
            try {
                Type instanceType = TypeLoader.TypeForName(providingBundle, className);
                IPreferenceStoreManager preferenceStoreManager = instanceType.NewInstance<IPreferenceStoreManager>();
                logWriter.InfoFormat("Preference store manager of type '{0}' successfully installed", preferenceStoreManager.GetType());
                return preferenceStoreManager;
            } catch (Exception ex) {
                logWriter.ErrorFormat("Error on installing preference store manager from plug-in '{0}' (Class '{1}'). Reason: {2}",
                    providingBundle, className, ex);
                return GetDefaultPreferenceStoreManager();
            }
        }
    }
}