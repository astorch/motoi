using System;
using System.Collections.Generic;
using motoi.extensions;
using motoi.platform.ui.shells;
using NLog;
using xcite.csharp;

namespace motoi.platform.application {
    /// <summary>
    /// Provides access to core components of the application.
    /// </summary>
    public class Platform : GenericSingleton<Platform> {
        /// <summary> Extension point id </summary>
        private const string PlatformServiceExtensionPointId = "org.motoi.platform.service";

        private static readonly Logger _platformLog = LogManager.GetCurrentClassLogger(typeof(Platform));
        private readonly Dictionary<Type, IPlatformService> _platformServices = new Dictionary<Type, IPlatformService>();

        public TService GetService<TService>() where TService : class, IPlatformService {
            Type serviceType = typeof(TService);

            if (!_platformServices.TryGetValue(serviceType, out IPlatformService serviceInstance)) return null;

            return (TService) serviceInstance;
        }

        /// <summary> Returns the platform log. </summary>
        public Logger PlatformLog 
            => _platformLog;

        /// <summary>
        /// Returns the current main window of the application. May be NULL if the application runs headless.
        /// </summary>
        public IMainWindow MainWindow { get; internal set; }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            _platformServices.Clear();
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            IConfigurationElement[] extensions = ExtensionService.Instance.GetConfigurationElements(PlatformServiceExtensionPointId);
        }
    }
}