namespace motoi.workbench.model.jobs {
    /// <summary>
    /// Provides methods to schedule jobs.
    /// </summary>
    public interface IJobService {
        /// <summary>
        /// Schedules a job. When the job is being executed the given <paramref name="onExecute"/> 
        /// handler is invoked.
        /// </summary>
        /// <param name="onExecute">Handle that is invoked when the job is being executed</param>
        /// <param name="state">(Optional) Job state object</param>
        /// <param name="jobName">(Optional) Job name</param>
        /// <returns>Job handle</returns>
        IJobHandle Schedule(JobExecutionHandler onExecute, object state, string jobName);
    }
}