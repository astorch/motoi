using System.Collections.Generic;
using System.IO;
using System.Linq;
using motoi.plugins;
using NLog;
using xcite.collections;

namespace motoi.extensions {
    /// <summary> Provides a service that handles Extension Points. </summary>
    public class ExtensionService {

        /// <summary> Backing variable for the service. </summary>
        private static ExtensionService _service;

        /// <summary> Returns the instance of this service. </summary>
        public static ExtensionService Instance 
            => _service ?? (_service = new ExtensionService());

        /// <summary> Defines the name of an extension file. </summary>
        private const string ExtensionFileName = "extensions.xml";

        /// <summary> Log instance. </summary>
        private readonly Logger iLogger = LogManager.GetCurrentClassLogger();

        private readonly ExtensionPointMap iExtensionPointMap = new ExtensionPointMap();
        private readonly IDictionary<IBundle, IList<IConfigurationElement>> iBundleToConfigurationElementMap = new Dictionary<IBundle, IList<IConfigurationElement>>(10);

        private bool _started;
        private bool _stopped;

        /// <summary> Private constructor. </summary>
        private ExtensionService() {
            Start();
        }

        /// <summary> Starts the Extension Service. </summary>
        public void Start() {
            if (_started) return;

            iLogger.Debug("Starting Extension Service");

            IList<IPluginInfo> providedPlugins = PluginService.Instance.ProvidedPlugins;
            providedPlugins.ForEach(plugin => {
                Stream stream = plugin.Bundle.GetResourceAsStream(ExtensionFileName);
                if (stream == null) return;

                // Create a parser and parse the stream
                ExtensionFileParser parser = ExtensionFileParser.GetInstance(stream);
                ExtensionPointMap map = parser.Parse();

                // Map bundle to Configuration Elements
                IList<IConfigurationElement> cfgElements = new List<IConfigurationElement>(map.Count);
                map.ForEach(x => cfgElements.AddAll(x.Value));
                iBundleToConfigurationElementMap.Add(plugin.Bundle, cfgElements);

                // Merge the new elements to the existing ones
                iExtensionPointMap.Merge(map);
            });
            
            _started = true;
            iLogger.Debug("Extension Service started");
        }

        /// <summary>
        /// Stops the Extension Service.
        /// </summary>
        public void Stop() {
            if (_stopped) return;

            iLogger.Debug("Stopping Extension Service");

            iBundleToConfigurationElementMap.Clear();
            iExtensionPointMap.Clear();
            _service = null;

            _stopped = true;
            iLogger.Debug("Extension Service stopped");
        }

        /// <summary>
        /// Returns all registered configuration elements that associated with the given id.
        /// </summary>
        /// <param name="id">Id of the extension point</param>
        /// <returns>Array of all registered configuration elements</returns>
        public IConfigurationElement[] GetConfigurationElements(string id) {
            return iExtensionPointMap.GetConfigurationElements(id);
        }

        /// <summary>
        /// Returns the bundle which provides the given configuration element.
        /// </summary>
        /// <param name="configurationElement">Configuration Element</param>
        /// <returns>Bundle or null</returns>
        public IBundle GetProvidingBundle(IConfigurationElement configurationElement) {
            IBundle bundle = iBundleToConfigurationElementMap.FirstOrDefault(x => x.Value.Contains(configurationElement)).Key;
            return bundle;
        }
    }
}