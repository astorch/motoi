using System;

namespace motoi.moose.commands {
    /// <summary>
    /// Implements a command that handles the "start" action.
    /// </summary>
    class StartCommand : IMooseCommand {
        private readonly string iBundleName;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="bundleName"></param>
        public StartCommand(string bundleName) {
            iBundleName = bundleName.Replace("\"", string.Empty);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute(IConsoleWriter consoleWriter) {
            try {
                Moose.Start(iBundleName);
            } catch (Exception ex) {
                consoleWriter.WriteError(ex.ToString());
            }
        }
    }
}