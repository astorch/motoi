using System;

namespace motoi.workbench.model.jobs {
    /// <summary>
    /// Implements a handle for a job that has being scheduled for execution.
    /// </summary>
    public interface IJobHandle {
        /// <summary> Event that is raised when the job execution has been started. </summary>
        event EventHandler Started;

        /// <summary> Event that is raised when the job execution has been finished. </summary>
        event EventHandler Finished;

        /// <summary> Event that is raised when the job execution has been cancelled. </summary>
        event EventHandler Cancelled;

        /// <summary>
        /// Blocks the current thread until the associated job has been executed.
        /// </summary>
        void Wait();

        /// <summary>
        /// Cancels the execution of the associated job.
        /// </summary>
        void Cancel();
    }
}