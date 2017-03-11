using System;
using System.Linq;
using log4net;
using motoi.moose.events;
using motoi.moose.exceptions;
using motoi.plugins;
using motoi.plugins.model;
using Xcite.Csharp.lang;

namespace motoi.moose {
    /// <summary>
    /// Provides methods to start or stop plug-ins.
    /// </summary>
    public static class Moose {
        private static readonly ILog iLog = LogManager.GetLogger(typeof(Moose));

        /// <summary>
        /// Is invoked when a plug-in has been started properly.
        /// </summary>
        public static event EventHandler<MooseEventArgs> PluginStarted;

        /// <summary>
        /// Is invoked when a plug-in has been stopped.
        /// </summary>
        public static event EventHandler<MooseEventArgs> PluginStopped; 

        /// <summary>
        /// Resolves and starts the plug-in with the given name. If there is no plug-in 
        /// with the given name, a <see cref="UnknownPluginException"/> is thrown.
        /// </summary>
        /// <param name="symbolicPluginName">Symbolic name of the plug-in to start</param>
        /// <exception cref="ArgumentNullException">If the given plug-in name is NULL or empty</exception>
        /// <exception cref="UnknownPluginException">If there is no plug-in with the given name</exception>
        public static void Start(string symbolicPluginName) {
            if (string.IsNullOrWhiteSpace(symbolicPluginName)) throw new ArgumentNullException("symbolicPluginName");
            
            IPluginInfo plugin = PluginService.Instance.FoundPlugins.FirstOrDefault(plgn => plgn.Signature.SymbolicName == symbolicPluginName);
            if (plugin == null) throw new UnknownPluginException(symbolicPluginName);

            try {
                // Start plug-in
                PluginService.Instance.ActivatePlugin(plugin);

                // TODO Remove multiple registration
                // TODO Add error handling
                PluginService.Instance.Stopped += (sender, args) => PluginStopped.Dispatch(new object[] {(object)null, new MooseEventArgs(symbolicPluginName)});

                // Notify listener
                PluginStarted.Dispatch(
                    new object[] { new MooseEventArgs(symbolicPluginName) },
                    (ex, dlg) => iLog.ErrorFormat("Error on dispatching PluginStarted event to '{0}'. Reason: {1}", dlg, ex));
            } catch (Exception ex) {
                iLog.ErrorFormat("Error on starting plug-in '{0}'. Reason: {1}", symbolicPluginName, ex);
            }   
        }

        /// <summary>
        /// Stops the plug-in with the given name.
        /// </summary>
        /// <param name="symbolicPluginName">Name of the plug-in to stop</param>
        /// <exception cref="ArgumentNullException">If the given plug-in name is NULL or empty</exception>
        public static void Stop(string symbolicPluginName) {
            PluginStopped.Dispatch(
                new object[] { (object)null, new MooseEventArgs(symbolicPluginName) },
                (ex, dlg) => iLog.ErrorFormat("Error on dispatching PluginStopped event to '{0}'. Reason: {1}", dlg, ex));
            throw new NotImplementedException();
        }
    }
}