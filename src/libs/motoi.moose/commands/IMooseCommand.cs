namespace motoi.moose.commands {
    /// <summary>
    /// Defines a command.
    /// </summary>
    public interface IMooseCommand {
        /// <summary>
        /// Executes the command
        /// </summary>
        void Execute(IConsoleWriter consoleWriter);
    }
}