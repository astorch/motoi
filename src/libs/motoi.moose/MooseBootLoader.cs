using System;
using System.IO;
using motoi.moose.commands;

namespace motoi.moose { 
    /// <summary>
    /// Provides common entry point of a moose based application.
    /// </summary>
    class MooseBootLoader {
        private static bool iDoLoop;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">Program arguments</param>
        static void Main(string[] args) {
            ConsoleWriter consoleWriter = new ConsoleWriter(Console.Out, Console.Error);
            iDoLoop = true;
            while (iDoLoop) {
                Console.Out.Write("moose>");
                string input = Console.In.ReadLine();
                IMooseCommand command = BuildCommand(input);
                command.Execute(consoleWriter);
            }
        }

        /// <summary>
        /// Parses the given input and returns a command that can handle it.
        /// </summary>
        /// <param name="input">Input to parse</param>
        /// <returns>Command that handles the input</returns>
        static IMooseCommand BuildCommand(string input) {
            string[] commandParams = input.Split(' ');
            if (commandParams.Length == 0) return NoActionCommand.Default;

            string command = commandParams[0].ToLowerInvariant();

            // Handling commands without parameters
            if (command == "exit".ToLowerInvariant()) return new ExitCommand();

            // No alternative
            if (commandParams.Length == 1) return NoActionCommand.Default;

            // Handling commands with parameters
            if (command == "start") return new StartCommand(commandParams[1]);
                
            // Default: do nothing
            return NoActionCommand.Default;
        }

        /// <summary>
        /// Implements a command that leads to no action.
        /// </summary>
        class NoActionCommand : IMooseCommand {
            /// <summary>
            /// Default instance. Can be used because it's stateless.
            /// </summary>
            public static readonly NoActionCommand Default = new NoActionCommand();

            /// <summary>
            /// Executes the command
            /// </summary>
            public void Execute(IConsoleWriter consoleWriter) { }
        }

        /// <summary>
        /// Implements a command which ends the application.
        /// </summary>
        class ExitCommand : IMooseCommand {
            /// <summary>
            /// Executes the command
            /// </summary>
            public void Execute(IConsoleWriter consoleWriter) {
                iDoLoop = false;
            }
        }

        /// <summary>
        /// Provides an implementation of <see cref="IConsoleWriter"/>.
        /// </summary>
        class ConsoleWriter : IConsoleWriter {
            private readonly TextWriter iStandardWriter;
            private readonly TextWriter iErrorWriter;

            /// <summary>
            /// Creates a new instance using the given writers.
            /// </summary>
            /// <param name="standardWriter">Standard output writer</param>
            /// <param name="errorWriter">Error writer</param>
            public ConsoleWriter(TextWriter standardWriter, TextWriter errorWriter) {
                iStandardWriter = standardWriter;
                iErrorWriter = errorWriter;
            }

            /// <summary>
            /// Writes the given text to the console using the error output stream
            /// </summary>
            /// <param name="text">Text to write</param>
            public void WriteError(string text) {
                iErrorWriter.WriteLine("[ERROR] {0}", text);
            }

            /// <summary>
            /// Writes the given text to the console using the standard output stream. 
            /// </summary>
            /// <param name="text">Text to write</param>
            public void WriteMessage(string text) {
                iStandardWriter.WriteLine(text);
            }
        }
    }
}
