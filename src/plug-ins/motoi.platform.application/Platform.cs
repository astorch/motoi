using System;
using System.Collections.Generic;
using log4net;
using motoi.extensions;
using motoi.extensions.core;
using motoi.platform.ui.shells;
using Xcite.Csharp.generics;

namespace motoi.platform.application {
    /// <summary>
    /// Provides access to core components of the application.
    /// </summary>
    public class Platform : GenericSingleton<Platform> {
        /// <summary> Extension point id </summary>
        private const string PlatformServiceExtensionPointId = "org.motoi.platform.service";

        private static readonly ILog iPlatformLog = LogManager.GetLogger(typeof(Platform));
        private readonly Dictionary<Type, IPlatformService> iRegisteredPlatformServices = new Dictionary<Type, IPlatformService>();

        public TService GetService<TService>() where TService : class, IPlatformService {
            Type serviceType = typeof(TService);
            
            IPlatformService serviceInstance;
            if (!iRegisteredPlatformServices.TryGetValue(serviceType, out serviceInstance)) return null;

            return (TService) serviceInstance;
        }

        /// <summary>
        /// Returns the platform log.
        /// </summary>
        public ILog PlatformLog { get { return iPlatformLog; } }

        /// <summary>
        /// Returns the current main window of the application. May be NULL if the application runs headless.
        /// </summary>
        public IMainWindow MainWindow { get; internal set; }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            iRegisteredPlatformServices.Clear();
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            IConfigurationElement[] extensions = ExtensionService.Instance.GetConfigurationElements(PlatformServiceExtensionPointId);
        }
    }
}