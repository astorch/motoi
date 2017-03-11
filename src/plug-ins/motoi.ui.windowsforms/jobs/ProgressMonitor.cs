using System;
using motoi.ui.windowsforms.controls;
using motoi.workbench;
using motoi.workbench.bindings;
using motoi.workbench.model.jobs;

namespace motoi.ui.windowsforms.jobs {
    /// <summary>
    /// Provides a win forms specific implementation of <see cref="IProgressMonitor"/>.
    /// </summary>
    public class ProgressMonitor : WorkbenchPropertyChangedDispatcher, IProgressMonitor {

        private static readonly ProgressMonitor Default = new ProgressMonitor();

        /// <summary>
        /// Returns the currently assigned instance of <see cref="ToolStripLabelledProgressBar"/> 
        /// that handles the property changes of each instance and delegates them to UI.
        /// </summary>
        internal static ToolStripLabelledProgressBar ToolStripProgressBar { get; set; }

        /// <summary>
        /// Returns an instance of this clas.
        /// </summary>
        /// <returns>Instance of this class</returns>
        public static ProgressMonitor GetInstance() {
            return Default.Prepare();
        }

        /// <inheritdoc />
        public bool IsIndetermine {
            get { return ToolStripProgressBar.IsIndetermine; }
            set {
                ExecuteOnUIThread(() => ToolStripProgressBar.IsIndetermine = value);
            } 
        }

        /// <inheritdoc />
        public string Text {
            get { return ToolStripProgressBar.ProgressHint; }
            set { ExecuteOnUIThread(() => ToolStripProgressBar.ProgressHint = value); } 
        }

        /// <inheritdoc />
        public ushort Value {
            get { return (ushort) ToolStripProgressBar.ProgressValue; }
            set { ExecuteOnUIThread(() => ToolStripProgressBar.ProgressValue = value); } 
        }

        /// <summary>
        /// Returns TRUE if the progress monitor is visible to the user or does set it.
        /// </summary>
        public bool IsVisible {
            get { return ToolStripProgressBar.Visible; }
            set { ExecuteOnUIThread(() => ToolStripProgressBar.ProgressVisible = value); }
        }

        /// <inheritdoc />
        public void Dispose() {
            IsVisible = false;
        }

        /// <summary>
        /// Resets the instance to an inital state.
        /// </summary>
        /// <returns>Same instance set to an initial state</returns>
        private ProgressMonitor Prepare() {
            IsIndetermine = false;
            Text = string.Empty;
            Value = 0;
            IsVisible = true;
            return this;
        }

        /// <summary>
        /// Invokes the given <paramref name="action"/> on the UI thread.
        /// </summary>
        /// <param name="action">Action to invoke</param>
        private void ExecuteOnUIThread(Action action) {
            PlatformUI.Instance.Invoker.InvokeAsync(action);
        }
    }
}