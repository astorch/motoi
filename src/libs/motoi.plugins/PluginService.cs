using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using motoi.plugins.enums;
using motoi.plugins.exceptions;
using motoi.plugins.model;
using Xcite.Csharp.generics;
using Xcite.Csharp.lang;

namespace motoi.plugins {
    /// <summary>
    /// Provides a service to handle all installed plug-ins.
    /// </summary>
    public class PluginService {

        /// <summary>
        /// Static initializer.
        /// </summary>
        static PluginService() {
            LogConfigurer.Configurate();
        }

        /// <summary>
        /// Log instance.
        /// </summary>
        private readonly ILog iLogger = LogManager.GetLogger(typeof (PluginService));

        /// <summary>
        /// Backing variable of the manager instance.
        /// </summary>
        private static PluginService iInstance;

        /// <summary>
        /// Returns the instance of this service.
        /// </summary>
        public static PluginService Instance { get { return iInstance ?? (iInstance = new PluginService()); } }

        private readonly IDictionary<string, Assembly> iNameToAssemblyMap = new Dictionary<string, Assembly>(100);
        private readonly LinkedList<IncludeMapping> iIncludeMappings = new LinkedList<IncludeMapping>();

        private readonly LinkedList<IPluginInfo> iFoundPlugins = new LinkedList<IPluginInfo>();
        private readonly LinkedList<IPluginInfo> iProvidedPlugins = new LinkedList<IPluginInfo>();
        private readonly LinkedList<IPluginInfo> iActivatedPlugins = new LinkedList<IPluginInfo>();

        private const string iPluginDirectoryPath = "\\plug-ins\\";

        private bool iStarted;
        private bool iStopped;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private PluginService() {
            // AppDomainSetup setup = new AppDomainSetup();
            // AppDomain motoiAppDomain = AppDomain.CreateDomain("Motoi Application Domain", null, setup);
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve += OnTypeResolve;
            Start();
        }

        /// <summary>
        /// Event that is raised when the service has been stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Will be invoked when the app domain cannot resolve a type on its own.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly OnTypeResolve(object sender, ResolveEventArgs args) {
            return null;
        }

        /// <summary>
        /// Will be invoked when the app domain cannot resolve an assembly on its own.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args) {
            string assemblyName = args.Name;

            string[] nameSplit = assemblyName.Split(',');
            // Fully qualified name
            if (nameSplit.Length > 1)
                assemblyName = nameSplit[0];

            // Remove trailing '.dll'
            assemblyName = assemblyName.Replace(".dll", string.Empty);

            // Maybe the assembly has already been resolved
            Assembly assmbly;
            if (iNameToAssemblyMap.TryGetValue(assemblyName, out assmbly))
                return assmbly;

            Stream assemblyStream;
            // Maybe it's an included assembly?
            string dllName = string.Format("{0}.dll", assemblyName);
            IncludeMapping inclMap = iIncludeMappings.FirstOrDefault(x => x.Name == dllName);
            if (inclMap != null) {
                string path = inclMap.BundlePath;
                IBundle bundle = inclMap.Bundle;
                assemblyStream = bundle.GetResourceAsStream(path);
                return Unwrap(assemblyStream, assemblyName);
            }

            // TODO Maybe a hash algorithmn would be faster
            // Looking for normal plugged assembly
            using (LinkedList<IPluginInfo>.Enumerator enmtor = iProvidedPlugins.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    IPluginInfo plugin = enmtor.Current;
                    IBundle bundle = plugin.Bundle;
                    if (bundle.Name != assemblyName)
                        continue;

                    string lookupName = string.Format("{0}.dll", assemblyName);
                    assemblyStream = bundle.GetResourceAsStream(lookupName);
                    return Unwrap(assemblyStream, assemblyName);
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
            if (assemblyStream == null)
                return null;

            const int defaultBufferSize = 1024*300; // 300kb 

            MemoryStream memoryStream = new MemoryStream(defaultBufferSize);
            assemblyStream.CopyTo(memoryStream);
            assemblyStream.Dispose();
            byte[] assemblyBytes = memoryStream.ToArray();
            memoryStream.Dispose();
            Assembly assembly = Assembly.Load(assemblyBytes);
            // Store mapping
            iNameToAssemblyMap.Add(assemblyName, assembly);
            return assembly;
        }

        /// <summary>
        /// Starts the Plug-in Service.
        /// </summary>
        public void Start() {
            if (iStarted)
                return;

            iLogger.Info("Starting Plug-in Service");

            string currentDirectoryPath = Directory.GetCurrentDirectory();
            string pluginDirectoryPath = string.Format("{0}{1}", currentDirectoryPath, iPluginDirectoryPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(pluginDirectoryPath);
            
            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException(pluginDirectoryPath);

            FileInfo[] marcFiles = directoryInfo.GetFiles("*.marc", SearchOption.TopDirectoryOnly);

            // Find all available plug-ins
            for (int i = -1; ++i < marcFiles.Length; ) {
                FileInfo marcFile = marcFiles[i];
                IBundle bundle = BundleFactory.Instance.CreateBundle(marcFile);
                IPluginInfo pluginInfo = RegisterBundle(bundle);

                if (pluginInfo == null)
                    continue;

                iFoundPlugins.AddLast(pluginInfo);

                // Handling additional resources
                string[] bundleResources = bundle.GetResources(@"includes/*");
                Array.ForEach(bundleResources, rsx => {
                        string[] split = rsx.Split('/');
                        string name = split.Last();
                        string bundlePath = rsx;
                        IncludeMapping inclMap = new IncludeMapping { Name = name, BundlePath = bundlePath, Bundle = bundle };
                        iIncludeMappings.AddLast(inclMap);
                    });
            }

            // Check dependencies
            using (LinkedList<IPluginInfo>.Enumerator enmtor = iFoundPlugins.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    IPluginInfo pluginInfo = enmtor.Current;
                    string missingDependencyName;
                    bool dependenciesSatisfied = CheckDependencies(pluginInfo, iFoundPlugins, out missingDependencyName);
                    if (!dependenciesSatisfied) {
                        string message = string.Format("Can not provide '{0}' because of missing dependency '{1}'. Plug-in will not be available!",
                                pluginInfo, missingDependencyName);
                        iLogger.Error(message);
                        continue;
                    }
                    SetPluginState(pluginInfo, EPluginState.Provided);
                    iProvidedPlugins.AddLast(pluginInfo);
                }
            }
            
            iStarted = true;
            iLogger.Info("Plug-in Service started");
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

        /// <summary>
        /// Sets the given state for the given plug-in.
        /// </summary>
        /// <param name="pluginInfo">Plug-in to modify</param>
        /// <param name="state">New state of the plug-in</param>
        private void SetPluginState(IPluginInfo pluginInfo, EPluginState state) {
            ((PluginInfoImpl)pluginInfo).State = state;
            string message = string.Format("Plug-in '{0}' {1}", pluginInfo.Signature.SymbolicName, state);
            iLogger.Info(message);
        }

        /// <summary>
        /// Processes the given marc file and creates an instance of <see cref="IPluginInfo"/>.
        /// </summary>
        /// <param name="bundle">Marc file to process</param>
        /// <returns>Instance of PluginInfo or null</returns>
        private IPluginInfo RegisterBundle(IBundle bundle) {
            iLogger.Debug(string.Format("Processing bundle '{0}'", bundle));
            try {
                Stream signatureFileStream = bundle.GetResourceAsStream("signature.mf");

                if (signatureFileStream == null) {
                    iLogger.Debug("No signature file found. Plug-in will not be available!");
                    return null;
                }

                iLogger.Debug("Signature file found");
                IPluginSignature pluginSignature;

                using (signatureFileStream) {
                    pluginSignature = SignatureFactory.Instance.CreateSignature(signatureFileStream);
                    iLogger.Debug(string.Format("Resolved plug-in signature '{0}'", pluginSignature));
                }

                IPluginInfo pluginInfo = PluginFactory.Instance.CreatePluginInfo(bundle, pluginSignature);
                iLogger.Debug(string.Format("Resolved plug-in info '{0}'", pluginInfo));

                return pluginInfo;
            } catch (Exception ex) {
                string message = string.Format("Could not process marc file '{0}'. Plug-in will not be available!",
                                               bundle);
                iLogger.Error(message, ex);
            }
            return null;
        }

        /// <summary>
        /// Stops the Plug-in Service and disposes all of its resources.
        /// </summary>
        public void Stop() {
            if (iStopped)
                return;

            iLogger.Info("Stopping Plug-in Service");

            iActivatedPlugins.Clear();
            iFoundPlugins.Clear();
            iProvidedPlugins.Clear();

            iInstance = null;

            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve -= OnTypeResolve;

            EventHandler stoppedHandler = Stopped;
            Stopped = null;

            iStopped = true;

            stoppedHandler.Dispatch(
                new object[]{this, EventArgs.Empty}, 
                (exception, dlg) => iLogger.WarnFormat("Error on dispatching Stopped event. Reason: {0}", exception)
            );
            iLogger.Info("Plug-in Service Stopped");
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
            if (!isProvided) throw new PluginActivationException(string.Format("Plug-in '{0}' cannot be started due to missing dependencies", plugin));

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
                    iLogger.ErrorFormat("Error on creating an instance of '{0}'. Could not activate plug-in! Reason: {1}", fullyQualifiedActivatorTypeName, ex);
                    throw new PluginActivationException(string.Format("Error on creating an instance of '{0}'", plugin), ex);
                }

                try {
                    pluginActivator.OnActivate();
                } catch (Exception ex) {
                    iLogger.ErrorFormat("Error on activating plug-in '{0}'. Reason: {1}", plugin, ex);
                    throw new PluginActivationException(string.Format("Error on activating plug-in '{0}'", plugin), ex);
                }
            } else {
                iLogger.DebugFormat("Plug-in '{0}' does not have an plug-in activator. It is activated silently.", plugin);
            }

            // Update state model
            SetPluginState(plugin, EPluginState.Activated);
            iActivatedPlugins.AddLast(plugin);
        }

        /// <summary>
        /// Returns all found plug-ins within a read-only collection.
        /// </summary>
        public ReadOnlyCollection<IPluginInfo> FoundPlugins { get { return iFoundPlugins.ToList().AsReadOnly(); } }

        /// <summary>
        /// Returns all provided plug-ins within a read-only collection.
        /// </summary>
        public ReadOnlyCollection<IPluginInfo> ProvidedPlugins { get { return iProvidedPlugins.ToList().AsReadOnly(); } }

        /// <summary>
        /// Returns all activated plug-ins with a read-only collection.
        /// </summary>
        public ReadOnlyCollection<IPluginInfo> ActivatedPlugins { get { return iActivatedPlugins.ToList().AsReadOnly(); } }

        /// <summary>
        /// Defines a include mapping for internal purposes.
        /// </summary>
        class IncludeMapping {

            /// <summary>
            /// Name of the include.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Bundle path.
            /// </summary>
            public string BundlePath { get; set; }

            /// <summary>
            /// Contributing bundle.
            /// </summary>
            public IBundle Bundle { get; set; }
        }
    }
}