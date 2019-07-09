using System;
using xcite.csharp;
using xcite.logging;

namespace motoi.platform.commons {
    /// <summary>
    /// Implements a platform error log. Added log entries are written into a specific log file 
    /// and can be accessed even if the platform has been shut down.
    /// </summary>
    public class PlatformErrorLog : GenericSingleton<PlatformErrorLog> {
        private readonly ILog _log = LogManager.GetLog(typeof(PlatformErrorLog));

        /// <summary> Event that is raised when an entry has been added. </summary>
        public event EventHandler<ErrorLogEntry> Added; 

        /// <summary>
        /// Adds an entry with the given arguments to the error log.
        /// </summary>
        /// <param name="logEntryType">Log entry type</param>
        /// <param name="pluginName">Name of the bundle that is the source of the log entry</param>
        /// <param name="message">Log entry message</param>
        /// <param name="exception">Log entry exception</param>
        public void Add(ELogEntryType logEntryType, string pluginName, string message, Exception exception) {
            if (exception == null && string.IsNullOrEmpty(message)) throw new ArgumentException("An exception or a message must be given");
            
            ErrorLogEntry errorLogEntry = new ErrorLogEntry(logEntryType, message ?? exception.Message, exception, pluginName);

            // TODO Write error log entry into a file
            
            // Log entry
            if (logEntryType == ELogEntryType.Error)
                _log.Error(errorLogEntry.Message + $" ({pluginName})", errorLogEntry.Exception);
            else
                _log.Warning(errorLogEntry.Message + $" ({pluginName})", errorLogEntry.Exception);

            // Dispatch event
            Added.Dispatch(new object[] {this, errorLogEntry}, OnDispatchError);
        }

        /// <summary>
        /// Is invoked when an error during the dispatching of the <see cref="Added"/> event occurred.
        /// </summary>
        /// <param name="exception">Occurred exception</param>
        /// <param name="delegate">Affected event handler</param>
        private void OnDispatchError(Exception exception, Delegate @delegate) {
            _log.Error($"Error on dispatching added event to '{@delegate}'. Event handler is ", exception);
        }

        /// <inheritdoc />
        protected override void OnInitialize() {
            // Nothing to do here
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            Added = null;
        }
    }

    /// <summary> Describes an error log entry. </summary>
    public class ErrorLogEntry : EventArgs {
        /// <summary>
        /// Creates a new instance with the given arguments.
        /// </summary>
        /// <param name="logEntryType">Log entry type</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="pluginName">Name of the plug-in that is source of the entry</param>
        public ErrorLogEntry(ELogEntryType logEntryType, string message, Exception exception, string pluginName) {
            LogEntryType = logEntryType;
            Message = message;
            Exception = exception;
            PluginName = pluginName;
            Timestamp = DateTime.Now;
        }

        /// <summary> Returns the log entry type. </summary>
        public ELogEntryType LogEntryType { get; }

        /// <summary> Returns the name of the plug-in name that added the entry. </summary>
        public string PluginName { get; }

        /// <summary> Returns the log entry message. </summary>
        public string Message { get; }

        /// <summary> Returns the logged exception. </summary>
        public Exception Exception { get; }

        /// <summary> Returns the timestamp the log entry has been created. </summary>
        public DateTime Timestamp { get; }
    }

    /// <summary> Defines kinds of log entry types. </summary>
    public enum ELogEntryType {
        /// <summary> Defines the error log entry type. </summary>
        Error,

        /// <summary> Defines the warning log entry type. </summary>
        Warning
    }
}