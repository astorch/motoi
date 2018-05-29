using System;
using motoi.workbench.model;
using motoi.workbench.model.jobs;
using NLog;
using xcite.csharp;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="ISaveableWorkbenchPart"/> that is based on <see cref="AbstractWorkbenchPart"/>.
    /// </summary>
    public abstract class AbstractSaveableWorkbenchPart : AbstractWorkbenchPart, ISaveableWorkbenchPart {
        private bool iIsDirty;

        /// <summary> Returns a log instance to write to. </summary>
        protected Logger Log { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Event that is raised when the <see cref="ISaveableWorkbenchPart.IsDirty"/> flag has been changed.
        /// </summary>
        public virtual event EventHandler DirtyChanged;

        /// <summary>
        /// Event that is raised when the <see cref="ISaveableWorkbenchPart.ExecuteSave"/> operation has been done.
        /// </summary>
        public virtual event EventHandler SaveExecuted;

        /// <summary>
        /// Event that is raised when the <see cref="ISaveableWorkbenchPart.ExecuteSaveAs"/> operation has been done.
        /// </summary>
        public virtual event EventHandler SaveAsExecuted;

        /// <summary>
        /// Returns TRUE if the workbench part is dirty and needs to be saved.
        /// </summary>
        public virtual bool IsDirty {
            get { return iIsDirty; }
            protected set {
                if (value == iIsDirty) return;
                iIsDirty = value;
                DispatchPropertyChanged(nameof(IsDirty));
                RaiseDirtyChangedEvent();
            }
        }

        /// <summary>
        /// Returns TRUE if the workbench part supports the save as feature.
        /// </summary>
        public virtual bool IsSaveAsAllowed {
            get { return true; }
        }

        /// <summary>
        /// Tells the workbench part to perform its save behavior.
        /// </summary>
        /// <param name="progressMonitor">Progress monitor</param>
        public abstract void ExecuteSave(IProgressMonitor progressMonitor);

        /// <summary>
        /// Tells the workbench part to perform its save as behavior.
        /// </summary>
        public abstract void ExecuteSaveAs();

        /// <summary>
        /// Raises the <see cref="DirtyChanged"/> event.
        /// </summary>
        protected virtual void RaiseDirtyChangedEvent() {
            DirtyChanged.Dispatch(new object[] {this, EventArgs.Empty}, OnEventDispatchException);
        }

        /// <summary>
        /// Raises the <see cref="SaveExecuted"/> event.
        /// </summary>
        protected virtual void RaiseSaveExecutedEvent() {
            SaveExecuted.Dispatch(new object[]{this, EventArgs.Empty}, OnEventDispatchException);
        }

        /// <summary>
        /// Raises the <see cref="SaveAsExecuted"/> event.
        /// </summary>
        protected virtual void RaiseSaveAsExecutedEvent() {
            SaveAsExecuted.Dispatch(new object[] {this, EventArgs.Empty}, OnEventDispatchException);
        }

        /// <summary>
        /// Is invoked when an error during the event dispatching occurred.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="delegate">Handler</param>
        protected virtual void OnEventDispatchException(Exception exception, Delegate @delegate) {
            Log.Error(exception, $"Error on dispatching event to '{@delegate}'.");
        }
    }
}