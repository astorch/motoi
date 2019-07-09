using System;

namespace motoi.moose.commands {
    /// <summary> Implements a command that handles the "start" action. </summary>
    class StartCommand : IMooseCommand {
        private readonly string _bundleName;

        /// <summary>
        /// Creates a new instance that starts the bundle
        /// with the specified <paramref name="bundleName"/>.
        /// </summary>
        public StartCommand(string bundleName) {
            _bundleName = bundleName.Replace("\"", string.Empty);
        }

        /// <inheritdoc />
        public void Execute(IConsoleWriter consoleWriter) {
            try {
                Moose.Start(_bundleName);
            } catch (Exception ex) {
                consoleWriter.WriteError(ex.ToString());
            }
        }
    }
}