using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;

namespace motoi.moose.applicationstarter {
    /// <summary>
    /// Provides the entry point of an motoi based application.
    /// </summary>
    public class EntryPoint {
        /// <summary>
        /// Implements the entry point for the execution engine.
        /// </summary>
        /// <param name="args">Execution arguments</param>
        static void Main(string[] args) {
            // Show a console window if desired
            if (args.Any(arg => arg == "-consoleLog"))
                ConsoleManager.ShowConsoleWindow();

            // Configure logging
            ConfigureLog();
            ILog log = LogManager.GetLogger(typeof(EntryPoint));
            log.InfoFormat("Log writer initialized");

            string platformPluginName = "motoi.platform.application";

            try {
                EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

                // Exit application when platform plug-in has been stopped
                Moose.PluginStopped += (sender, eventArgs) => {
                    string stoppedPluginName = eventArgs.SymbolicPluginName;
                    if (stoppedPluginName != platformPluginName) return;
                    waitHandle.Set();
                };

                // Start platform plug-in
                Moose.Start(platformPluginName);

                // Wait until the application is ready to exit
                waitHandle.WaitOne();

                // Hide the console window
                ConsoleManager.HideConsoleWindow();
            } catch (Exception ex) {
                log.ErrorFormat("Error on starting plug-in '{0}'. Reason: {1}", platformPluginName, ex);
            }
        }

        /// <summary>
        /// Configures the log4net framework.
        /// </summary>
        private static void ConfigureLog() {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("motoi.moose.applicationstarter.log4net.config")) {
                if (stream == null) throw new InvalidOperationException("Could not read log4net.config stream");
                log4net.Config.XmlConfigurator.Configure(stream);
            }
        }
    }
}
