using System;
using System.Linq;
using motoi.plugins;
using NLog;
using xcite.csharp;

namespace motoi.moose {
    /// <summary> Provides methods to start or stop plug-ins. </summary>
    public static class Moose {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger(typeof(Moose));

        /// <summary> Is invoked when a plug-in has been started properly. </summary>
        public static event EventHandler<MooseEventArgs> PluginStarted;

        /// <summary> Is invoked when a plug-in has been stopped. </summary>
        public static event EventHandler<MooseEventArgs> PluginStopped; 

        /// <summary>
        /// Resolves and starts the plug-in with the given name. If there is no plug-in 
        /// with the given name, a <see cref="UnknownPluginException"/> is thrown.
        /// </summary>
        /// <param name="symbolicPluginName">Symbolic name of the plug-in to start</param>
        /// <exception cref="ArgumentNullException">If the given plug-in name is NULL or empty</exception>
        /// <exception cref="UnknownPluginException">If there is no plug-in with the given name</exception>
        public static void Start(string symbolicPluginName) {
            if (string.IsNullOrWhiteSpace(symbolicPluginName)) throw new ArgumentNullException(nameof(symbolicPluginName));
            
            IPluginInfo plugin = PluginService.Instance.FoundPlugins.FirstOrDefault(plgn => plgn.Signature.SymbolicName == symbolicPluginName);
            if (plugin == null) throw new UnknownPluginException(symbolicPluginName);

            try {
                // Start plug-in
                PluginService.Instance.ActivatePlugin(plugin);

                // TODO Remove multiple registration
                // TODO Add error handling
                PluginService.Instance.Stopped += (sender, args) => PluginStopped.Dispatch(new[] {(object) null, new MooseEventArgs(symbolicPluginName)});

                // Notify listener
                PluginStarted.Dispatch(
                    new object[] { new MooseEventArgs(symbolicPluginName) },
                    (ex, dlg) => _log.Error(ex, $"Error on dispatching PluginStarted event to '{dlg}'"));
            } catch (Exception ex) {
                _log.Error(ex, $"Error on starting plug-in '{symbolicPluginName}'");
            }   
        }

        /// <summary> Stops the plug-in with the given name. </summary>
        /// <param name="symbolicPluginName">Name of the plug-in to stop</param>
        /// <exception cref="ArgumentNullException">If the given plug-in name is NULL or empty</exception>
        public static void Stop(string symbolicPluginName) {
            PluginStopped.Dispatch(
                new[] {(object) null, new MooseEventArgs(symbolicPluginName)},
                (ex, dlg) => _log.Error(ex, $"Error on dispatching PluginStopped event to '{dlg}'"));
            throw new NotImplementedException();
        }
    }
}