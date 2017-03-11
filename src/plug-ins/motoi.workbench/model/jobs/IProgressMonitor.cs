using System;
using motoi.platform.ui;

namespace motoi.workbench.model.jobs {
    /// <summary>
    /// Provides methods to visualize a background task with a measurable progress for 
    /// the user.
    /// </summary>
    public interface IProgressMonitor : IViewPart, IDisposable {
        /// <summary>
        /// Returns TRUE if the progress is not measurable or does set it.
        /// </summary>
        bool IsIndetermine { get; set; }

        /// <summary>
        /// Returns the current hint text to display or does set it.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Returns the current progress value (max. 100) or does set it.
        /// </summary>
        ushort Value { get; set; }
    }
}