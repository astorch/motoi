using System.Collections.Generic;
using System.IO;
using System.Linq;
using motoi.plugins;
using xcite.logging;

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
        private readonly ILog iLogger = LogManager.GetLog(typeof(ExtensionService));

        private readonly ExtensionPointMap _extensionPointMap = new ExtensionPointMap();
        private readonly IDictionary<IBundle, IList<IConfigurationElement>> _bundleToConfigurationElementMap = new Dictionary<IBundle, IList<IConfigurationElement>>(10);

        private bool _started;
        private bool _stopped;

        /// <inheritdoc />
        private ExtensionService() {
            Start();
        }

        /// <summary> Starts the Extension Service. </summary>
        public void Start() {
            if (_started) return;

            iLogger.Debug("Starting extension service");

            IList<IPluginInfo> providedPlugins = PluginService.Instance.ProvidedPlugins;
            for (int i = -1; ++i != providedPlugins.Count;) {
                IPluginInfo plugin = providedPlugins[i];
                
                Stream stream = plugin.Bundle.GetResourceAsStream(ExtensionFileName);
                if (stream == null) continue;

                // Create a parser and parse the stream
                ExtensionFileParser parser = ExtensionFileParser.GetInstance(stream);
                ExtensionPointMap map = parser.Parse();

                // Map bundle to Configuration Elements
                IList<IConfigurationElement> cfgElements = new List<IConfigurationElement>(map.Count);

                using (IEnumerator<KeyValuePair<string, IList<IConfigurationElement>>> itr = map.GetEnumerator()) {
                    while (itr.MoveNext()) {
                        KeyValuePair<string, IList<IConfigurationElement>> entry = itr.Current;
                        IList<IConfigurationElement> cfgElementSet = entry.Value;

                        for (int j = -1; ++j != cfgElementSet.Count;) {
                            IConfigurationElement cfgEl = cfgElementSet[j];
                            cfgElements.Add(cfgEl);
                        }
                    }
                }
                
                _bundleToConfigurationElementMap.Add(plugin.Bundle, cfgElements);

                // Merge the new elements to the existing ones
                _extensionPointMap.Merge(map);
            }

            _started = true;
            iLogger.Debug("Extension service started");
        }

        /// <summary> Stops the Extension Service. </summary>
        public void Stop() {
            if (_stopped) return;

            iLogger.Debug("Stopping extension service");

            _bundleToConfigurationElementMap.Clear();
            _extensionPointMap.Clear();
            _service = null;

            _stopped = true;
            iLogger.Debug("Extension service stopped");
        }

        /// <summary> Returns all registered configuration elements that associated with the given id. </summary>
        /// <param name="id">Id of the extension point</param>
        /// <returns>Array of all registered configuration elements</returns>
        public IConfigurationElement[] GetConfigurationElements(string id) {
            return _extensionPointMap.GetConfigurationElements(id);
        }

        /// <summary> Returns the bundle which provides the given configuration element. </summary>
        /// <param name="configurationElement">Configuration Element</param>
        /// <returns>Bundle or null</returns>
        public IBundle GetProvidingBundle(IConfigurationElement configurationElement) {
            IBundle bundle = _bundleToConfigurationElementMap.FirstOrDefault(x => x.Value.Contains(configurationElement)).Key;
            return bundle;
        }
    }
}