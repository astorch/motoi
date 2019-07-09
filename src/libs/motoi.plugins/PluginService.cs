using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using xcite.csharp;
using xcite.logging;

namespace motoi.plugins {
    /// <summary> Provides a service to handle all installed plug-ins. </summary>
    public class PluginService {
        private static PluginService _instance;

        /// <summary> Returns the instance of this service. </summary>
        public static PluginService Instance 
            => _instance ?? (_instance = new PluginService());

        private readonly ILog _log = LogManager.GetLog(typeof(PluginService));

        private readonly IDictionary<string, Assembly> _nameToAssemblyMap = new Dictionary<string, Assembly>(100);
        private readonly LinkedList<IncludeMapping> _includeMappings = new LinkedList<IncludeMapping>();

        private readonly LinkedList<IPluginInfo> _foundPlugins = new LinkedList<IPluginInfo>();
        private readonly LinkedList<IPluginInfo> _providedPlugins = new LinkedList<IPluginInfo>();
        private readonly LinkedList<IPluginInfo> _activatedPlugins = new LinkedList<IPluginInfo>();

        private const string PluginDirectoryPath = "\\plug-ins\\";

        private bool _started;
        private bool _stopped;

        /// <inheritdoc />
        private PluginService() {
            // AppDomainSetup setup = new AppDomainSetup();
            // AppDomain motoiAppDomain = AppDomain.CreateDomain("Motoi Application Domain", null, setup);
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve += OnTypeResolve;
            Start();
        }

        /// <summary> Event that is raised when the service has been stopped. </summary>
        public event EventHandler Stopped;

        /// <summary> Will be invoked when the app domain cannot resolve a type on its own. </summary>
        private Assembly OnTypeResolve(object sender, ResolveEventArgs args) {
            return null;
        }

        /// <summary> Will be invoked when the app domain cannot resolve an assembly on its own. </summary>
        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args) {
            string assemblyName = args.Name;

            string[] nameSplit = assemblyName.Split(',');
            // Fully qualified name
            if (nameSplit.Length > 1)
                assemblyName = nameSplit[0];

            // Remove trailing '.dll'
            assemblyName = assemblyName.Replace(".dll", string.Empty);

            // Maybe the assembly has already been resolved
            if (_nameToAssemblyMap.TryGetValue(assemblyName, out Assembly assmbly))
                return assmbly;

            Stream assemblyStream;
            // Maybe it's an included assembly?
            string dllName = $"{assemblyName}.dll";
            IncludeMapping inclMap = _includeMappings.FirstOrDefault(x => x.Name == dllName);
            if (inclMap != null) {
                string path = inclMap.BundlePath;
                IBundle bundle = inclMap.Bundle;
                assemblyStream = bundle.GetResourceAsStream(path);
                using (assemblyStream) {
                    return Unwrap(assemblyStream, assemblyName);
                }
            }

            // TODO Maybe a hash algorithmn would be faster
            // Looking for normal plugged assembly
            using (LinkedList<IPluginInfo>.Enumerator enmtor = _providedPlugins.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    IPluginInfo plugin = enmtor.Current;
                    if (plugin == null) continue;
                    
                    IBundle bundle = plugin.Bundle;
                    if (bundle.Name != assemblyName) continue;

                    string lookupName = $"{assemblyName}.dll";
                    assemblyStream = bundle.GetResourceAsStream(lookupName);
                    using (assemblyStream) {
                        return Unwrap(assemblyStream, assemblyName);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Loads and returns the assembly referenced by the given assembly stream. 
        /// The assembly is added to a hash map so that every assembly is only loaded once.
        /// </summary>
        /// <param name="assemblyStream">Stream for the assembly</param>
        /// <param name="assemblyName">Name of the assembly</param>
        /// <returns>Loaded assembly</returns>
        private Assembly Unwrap(Stream assemblyStream, string assemblyName) {
            if (assemblyStream == null) return null;

            const int defaultBufferSize = 1024 * 1024 * 1; // 1MB 

            byte[] assemblyBytes;
            using (MemoryStream memoryStream = new MemoryStream(defaultBufferSize)) {
                assemblyStream.CopyTo(memoryStream);
                assemblyBytes = memoryStream.ToArray();
            }
                
            Assembly assembly = Assembly.Load(assemblyBytes);
            
            // Store mapping
            _nameToAssemblyMap.Add(assemblyName, assembly);
            return assembly;
        }

        /// <summary> Starts the Plug-in Service. </summary>
        public void Start() {
            if (_started) return;

            _log.Info("Starting plug-in service");

            string currentDirectoryPath = Directory.GetCurrentDirectory();
            string pluginDirectoryPath = $"{currentDirectoryPath}{PluginDirectoryPath}";
            DirectoryInfo directoryInfo = new DirectoryInfo(pluginDirectoryPath);
            
            if (!directoryInfo.Exists) throw new DirectoryNotFoundException(pluginDirectoryPath);

            FileInfo[] marcFiles = directoryInfo.GetFiles("*.marc", SearchOption.TopDirectoryOnly);

            // Find all available plug-ins
            for (int i = -1; ++i < marcFiles.Length; ) {
                FileInfo marcFile = marcFiles[i];
                IBundle bundle = BundleFactory.Instance.CreateBundle(marcFile);
                IPluginInfo pluginInfo = RegisterBundle(bundle);

                if (pluginInfo == null) continue;

                _foundPlugins.AddLast(pluginInfo);

                // Handling additional resources
                string[] bundleResources = bundle.GetResources(@"includes/*");
                for (int j = -1; ++j != bundleResources.Length;) {
                    string rsx = bundleResources[j];
                    string[] split = rsx.Split('/');
                    string name = split.Last();
                    string bundlePath = rsx;
                    IncludeMapping inclMap = new IncludeMapping { Name = name, BundlePath = bundlePath, Bundle = bundle };
                    _includeMappings.AddLast(inclMap);
                }
            }

            // Check dependencies
            using (LinkedList<IPluginInfo>.Enumerator enmtor = _foundPlugins.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    IPluginInfo pluginInfo = enmtor.Current;
                    bool dependenciesSatisfied = CheckDependencies(pluginInfo, _foundPlugins, out string missingDependencyName);
                    if (!dependenciesSatisfied) {
                        string message = $"Can not provide '{pluginInfo}' " +
                                         $"because of missing dependency '{missingDependencyName}'. " +
                                          "Plug-in will not be available!";
                        _log.Error(message);
                        continue;
                    }
                    SetPluginState(pluginInfo, EPluginState.Provided);
                    _providedPlugins.AddLast(pluginInfo);
                }
            }
            
            _started = true;
            _log.Info("Plug-in Service started");
        }

        /// <summary>
        /// Checks if all dependencies of the given plug-in can be satisfied. If one dependency is missing, the name 
        /// of that one will be resolved.
        /// </summary>
        /// <param name="pluginInfo">Plug-in to check</param>
        /// <param name="pluginPool">Plug-in pool</param>
        /// <param name="missingDependencyName">Name of the missing dependency or null</param>
        /// <returns>True if all dependencies can be satisfied</returns>
        private bool CheckDependencies(IPluginInfo pluginInfo, LinkedList<IPluginInfo> pluginPool, out string missingDependencyName) {
            // string pluginSymbolicName = pluginInfo.Signature.SymbolicName;
            string[] dependencies = pluginInfo.Signature.Dependencies;

            for (int i = -1; ++i < dependencies.Length;) {
                string dependencySymbolicName = dependencies[i].ToLowerInvariant();

                if(string.IsNullOrEmpty(dependencySymbolicName))
                    continue;

                IPluginInfo dependendPlugin = pluginPool.FirstOrDefault(info => info.Signature.SymbolicName.ToLowerInvariant() == dependencySymbolicName);
                if (dependendPlugin == null) {
                    missingDependencyName = dependencySymbolicName;
                    return false;
                }
            }

            missingDependencyName = null;
            return true;
        }

        /// <summary> Sets the given state for the given plug-in. </summary>
        /// <param name="pluginInfo">Plug-in to modify</param>
        /// <param name="state">New state of the plug-in</param>
        private void SetPluginState(IPluginInfo pluginInfo, EPluginState state) {
            ((PluginInfoImpl) pluginInfo).State = state;
            string message = $"Plug-in '{pluginInfo.Signature.SymbolicName}' {state}";
            _log.Info(message);
        }

        /// <summary> Processes the given marc file and creates an instance of <see cref="IPluginInfo"/>. </summary>
        /// <param name="bundle">Marc file to process</param>
        /// <returns>Instance of PluginInfo or null</returns>
        private IPluginInfo RegisterBundle(IBundle bundle) {
            _log.Debug($"Processing bundle '{bundle}'");
            try {
                Stream signatureFileStream = bundle.GetResourceAsStream("signature.mf");

                if (signatureFileStream == null) {
                    _log.Debug("No signature file found. Plug-in will not be available!");
                    return null;
                }

                _log.Debug("Signature file found");
                IPluginSignature pluginSignature;

                using (signatureFileStream) {
                    pluginSignature = SignatureFactory.Instance.CreateSignature(signatureFileStream);
                    _log.Debug($"Resolved plug-in signature '{pluginSignature}'");
                }

                IPluginInfo pluginInfo = PluginFactory.Instance.CreatePluginInfo(bundle, pluginSignature);
                _log.Debug($"Resolved plug-in info '{pluginInfo}'");

                return pluginInfo;
            } catch (Exception ex) {
                string message = $"Could not process marc file '{bundle}'. Plug-in will not be available!";
                _log.Error(message, ex);
            }
            return null;
        }

        /// <summary> Stops the plug-in service and disposes all of its resources. </summary>
        public void Stop() {
            if (_stopped) return;

            _log.Info("Stopping plug-in service");

            _activatedPlugins.Clear();
            _foundPlugins.Clear();
            _providedPlugins.Clear();

            _instance = null;

            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve -= OnTypeResolve;

            EventHandler stoppedHandler = Stopped;
            Stopped = null;

            _stopped = true;

            stoppedHandler.Dispatch(
                new object[]{this, EventArgs.Empty}, 
                (exception, dlg) => _log.Warning("Error on dispatching stopped event.", exception)
            );
            _log.Info("Plug-in service stopped");
        }

        /// <summary>
        /// Activates the given <paramref name="plugin"/>. Activating a plug-in means to notify the corresponding <see cref="IPluginActivator"/> of 
        /// the plug-in. If there isn't any, the activation will be done silently. An activated plug-in will run a state transition from 
        /// <see cref="EPluginState.Provided"/> to <see cref="EPluginState.Activated"/>. An already activated plug-in cannot be activated again. 
        /// To activate a plug-in its dependencies must be resolvable, otherwise the activation will fail. If the given <paramref name="plugin"/> 
        /// is NULL, nothing will happen.
        /// </summary>
        /// <param name="plugin">Plug-in to activate</param>
        /// <exception cref="PluginActivationException">If an error occurs</exception>
        public void ActivatePlugin(IPluginInfo plugin) {
            if (plugin == null) return;
            if (plugin.State != EPluginState.Provided) return;

            bool isProvided = ProvidedPlugins.Contains(plugin);
            if (!isProvided) throw new PluginActivationException($"Plug-in '{plugin}' cannot be started due to missing dependencies");

            IPluginSignature pluginSignature = plugin.Signature;
            IBundle bundle = plugin.Bundle;
            string pluginActivatorTypeName = pluginSignature.PluginActivatorType;
            pluginActivatorTypeName = pluginActivatorTypeName.Replace("/", ".");

            if (!string.IsNullOrEmpty(pluginActivatorTypeName)) {
                string fullyQualifiedActivatorTypeName = string.Format("{0}.{1},{0}", bundle.Name, pluginActivatorTypeName);
                IPluginActivator pluginActivator;

                // Try to create an instance of 
                try {
                    Type activatorType = Type.GetType(fullyQualifiedActivatorTypeName, true);
                    pluginActivator = activatorType.NewInstance<IPluginActivator>();
                    ((PluginSignatureImpl) pluginSignature).PluginActivator = pluginActivator;
                } catch (Exception ex) {
                    _log.Error($"Error on creating instance of '{fullyQualifiedActivatorTypeName}'", ex);
                    throw new PluginActivationException($"Error on creating an instance of '{plugin}'", ex);
                }

                try {
                    pluginActivator.OnActivate();
                } catch (Exception ex) {
                    _log.Error($"Error on activating plug-in '{plugin}'", ex);
                    throw new PluginActivationException($"Error on activating plug-in '{plugin}'", ex);
                }
            } else {
                _log.Debug($"Plug-in '{plugin}' does not have a plug-in activator. It is activated silently.");
            }

            // Update state model
            SetPluginState(plugin, EPluginState.Activated);
            _activatedPlugins.AddLast(plugin);
        }

        /// <summary> Returns all found plug-ins within a read-only collection. </summary>
        public ReadOnlyCollection<IPluginInfo> FoundPlugins 
            => _foundPlugins.ToList().AsReadOnly();

        /// <summary> Returns all provided plug-ins within a read-only collection. </summary>
        public ReadOnlyCollection<IPluginInfo> ProvidedPlugins 
            => _providedPlugins.ToList().AsReadOnly();

        /// <summary> Returns all activated plug-ins with a read-only collection. </summary>
        public ReadOnlyCollection<IPluginInfo> ActivatedPlugins 
            => _activatedPlugins.ToList().AsReadOnly();

        /// <summary> Defines a include mapping for internal purposes. </summary>
        class IncludeMapping {
            /// <summary> Name of the include </summary>
            public string Name { get; set; }

            /// <summary> Bundle path </summary>
            public string BundlePath { get; set; }

            /// <summary> Contributing bundle </summary>
            public IBundle Bundle { get; set; }
        }
    }
}