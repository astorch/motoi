using System;
using System.Diagnostics;
using Xcite.Csharp.assertions;

namespace Xcite.Csharp.lang {
    /// <summary>
    /// Creates a stopwatch and performs a log method invocation when this instance is being disposed.
    /// </summary>
    public class StopwatchLog : IDisposable {
        private readonly Action<Stopwatch, object> iLogEntryAction;
        private readonly Stopwatch iStopwatch;

        /// <summary>
        /// Creates a new instance that invokes the given delegate when the instance is being disposed.
        /// </summary>
        /// <param name="logEntryAction">Delegate to invoke</param>
        private StopwatchLog(Action<Stopwatch, object> logEntryAction) {
            iLogEntryAction = logEntryAction;
            iStopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Stops the stopwatch and invokes the delegate using the given data object.
        /// </summary>
        /// <param name="data">Additional data object</param>
        private void Stop(object data) {
            iStopwatch.Stop();
            iLogEntryAction(iStopwatch, data);            
        }

        /// <summary>
        /// Disposes the instance and invokes the log method delegate. No additional data is passed.
        /// </summary>
        public void Dispose() {
            Dispose(null);
        }

        /// <summary>
        /// Disposes the instance and invokes the log method delegate. The given data object is passed additionally.
        /// </summary>
        /// <param name="data">Additional data object</param>
        public void Dispose(object data) {
            Stop(data);
        }

        /// <summary>
        /// Creates a new instance that invokes the given delegate when the instance is being disposed.
        /// </summary>
        /// <param name="logEntry">Delegate to invoke</param>
        /// <returns>Newly created instance</returns>
        public static StopwatchLog StartNew(Action<Stopwatch, object> logEntry) {
            Assert.NotNull(() => logEntry);
            return new StopwatchLog(logEntry);
        }
    }
}