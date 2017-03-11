using System.Threading;
using motoi.plugins.model;

namespace motoi.platform.application.internals {
    /// <summary>
    /// Provides an implementation of <see cref="IPluginActivator"/>.
    /// </summary>
    public class PlatformPluginActivator : IPluginActivator {
        /// <summary>
        /// Will be invoked when the plug-in has been activated.
        /// </summary>
        public void OnActivate() {
            new Thread(OnPlatformStart) {Name = "Motoi Platform Runner"}.Start();
        }

        /// <summary>
        /// Is invoked when the 'Motoi Platform Runner' threas has been started.
        /// </summary>
        private void OnPlatformStart() {
            MotoiBootloader.Main(new string[0]);
        }

        /// <summary>
        /// Will be invoked when the plug-in has been disposed.
        /// </summary>
        public void OnDispose() {
            // Currently nothing to do here
        }
    }
}