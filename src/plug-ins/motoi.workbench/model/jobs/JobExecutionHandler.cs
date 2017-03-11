namespace motoi.workbench.model.jobs {
    /// <summary>
    /// Defines the signature of a method that is invoked when a job is being executed.
    /// </summary>
    /// <param name="progressMonitor">Associated progress monitor</param>
    /// <param name="state">State object</param>
    public delegate void JobExecutionHandler(IProgressMonitor progressMonitor, object state);
}