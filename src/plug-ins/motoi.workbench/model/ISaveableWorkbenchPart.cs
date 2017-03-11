using System;
using motoi.workbench.model.jobs;

namespace motoi.workbench.model {
    /// <summary>
    /// Defines a <see cref="IWorkbenchPart"/> which supports an (internal) savable 
    /// state model.
    /// </summary>
    public interface ISaveableWorkbenchPart : IWorkbenchPart {
        /// <summary>
        /// Event that is raised when the <see cref="IsDirty"/> flag has been changed.
        /// </summary>
        event EventHandler DirtyChanged;

        /// <summary>
        /// Event that is raised when the <see cref="ExecuteSave"/> operation has been done.
        /// </summary>
        event EventHandler SaveExecuted;

        /// <summary>
        /// Event that is raised when the <see cref="ExecuteSaveAs"/> operation has been done.
        /// </summary>
        event EventHandler SaveAsExecuted; 

        /// <summary>
        /// Returns TRUE if the workbench part is dirty and needs to be saved.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Returns TRUE if the workbench part supports the save as feature.
        /// </summary>
        bool IsSaveAsAllowed { get; }

        /// <summary>
        /// Tells the workbench part to perform its save behavior.
        /// </summary>
        /// <param name="progressMonitor">Progress monitor</param>
        void ExecuteSave(IProgressMonitor progressMonitor);

        /// <summary>
        /// Tells the workbench part to perform its save as behavior.
        /// </summary>
        void ExecuteSaveAs();
    }
}