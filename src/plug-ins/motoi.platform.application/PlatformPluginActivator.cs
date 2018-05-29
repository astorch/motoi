using System.Threading;
using motoi.plugins;

namespace motoi.platform.application {
    /// <summary> Provides an implementation of <see cref="IPluginActivator"/>. </summary>
    public class PlatformPluginActivator : IPluginActivator {
        /// <inheritdoc />
        public void OnActivate() 
            => new Thread(OnPlatformStart) {Name = "Motoi Platform Runner"}.Start();

        /// <inheritdoc />
        public void OnDispose() {
            // Currently nothing to do here
        }

        /// <summary> Is invoked when the 'Motoi Platform Runner' threas has been started. </summary>
        private void OnPlatformStart() {
            MotoiBootloader.Main(new string[0]);
        }
    }
}