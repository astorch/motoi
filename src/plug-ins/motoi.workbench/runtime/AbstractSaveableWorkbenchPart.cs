using System;
using motoi.workbench.model;
using motoi.workbench.model.jobs;
using xcite.csharp;
using xcite.logging;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="ISaveableWorkbenchPart"/>
    /// that is based on <see cref="AbstractWorkbenchPart"/>.
    /// </summary>
    public abstract class AbstractSaveableWorkbenchPart : AbstractWorkbenchPart, ISaveableWorkbenchPart {
        private bool _isDirty;

        /// <inheritdoc />
        protected AbstractSaveableWorkbenchPart() {
            Log = LogManager.GetLog(GetType());
        }
        
        /// <summary> Returns a log instance to write to. </summary>
        protected ILog Log { get; }
        
        /// <inheritdoc />
        public virtual event EventHandler DirtyChanged;

        /// <inheritdoc />
        public virtual event EventHandler SaveExecuted;
        
        /// <inheritdoc />
        public virtual event EventHandler SaveAsExecuted;
        
        /// <inheritdoc />
        public virtual bool IsDirty {
            get { return _isDirty; }
            protected set {
                if (value == _isDirty) return;
                _isDirty = value;
                DispatchPropertyChanged(nameof(IsDirty));
                RaiseDirtyChangedEvent();
            }
        }
        
        /// <inheritdoc />
        public virtual bool IsSaveAsAllowed 
            => true;

        /// <inheritdoc />
        public abstract void ExecuteSave(IProgressMonitor progressMonitor);
        
        /// <inheritdoc />
        public abstract void ExecuteSaveAs();

        /// <summary> Raises the <see cref="DirtyChanged"/> event. </summary>
        protected virtual void RaiseDirtyChangedEvent() {
            DirtyChanged.Dispatch(new object[] {this, EventArgs.Empty}, OnEventDispatchException);
        }

        /// <summary> Raises the <see cref="SaveExecuted"/> event. </summary>
        protected virtual void RaiseSaveExecutedEvent() {
            SaveExecuted.Dispatch(new object[]{this, EventArgs.Empty}, OnEventDispatchException);
        }

        /// <summary>
        /// Raises the <see cref="SaveAsExecuted"/> event. </summary>
        protected virtual void RaiseSaveAsExecutedEvent() {
            SaveAsExecuted.Dispatch(new object[] {this, EventArgs.Empty}, OnEventDispatchException);
        }

        /// <summary> Is invoked when an error during the event dispatching occurred. </summary>
        /// <param name="exception">Exception</param>
        /// <param name="delegate">Handler</param>
        protected virtual void OnEventDispatchException(Exception exception, Delegate @delegate) {
            Log.Error($"Error on dispatching event to '{@delegate}'.", exception);
        }
    }
}