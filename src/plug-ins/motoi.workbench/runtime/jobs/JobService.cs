using System;
using System.Collections.Generic;
using System.Threading;
using motoi.platform.ui.factories;
using motoi.workbench.model.jobs;
using xcite.csharp;
using xcite.logging;

namespace motoi.workbench.runtime.jobs {
    /// <summary> Provides an implementation of <see cref="IJobService"/>. </summary>
    public class JobService : IJobService {
        private readonly ILog _log = LogManager.GetLog(typeof(JobService));
        private readonly Queue<JobHandle> _jobQueue = new Queue<JobHandle>(7);
        private readonly AutoLockStruct<bool> _processingQueue = new AutoLockStruct<bool>();

        /// <inheritdoc />
        public IJobHandle Schedule(JobExecutionHandler onExecute, object state, string jobName) {
            if (onExecute == null) throw new ArgumentNullException(nameof(onExecute));

            JobHandle jobHandle = new JobHandle(onExecute, state, jobName);
            lock (_jobQueue) {
                _jobQueue.Enqueue(jobHandle);
            }

            using (_processingQueue.Lock()) {
                if (!_processingQueue.Get()) {
                    _processingQueue.Set(true);
                    ThreadPool.QueueUserWorkItem(ProcessJobQueue, _jobQueue);
                }
            }

            return jobHandle;
        }

        /// <summary> Is invoked when a thread to process the job queue has been started. </summary>
        /// <param name="state">Reference to the job queue</param>
        private void ProcessJobQueue(object state) {
            Thread.CurrentThread.Name = "Job Service - Job Queue Processor";
            Queue<JobHandle> jobQueue = (Queue<JobHandle>) state;
            while (true) {
                JobHandle job;
                lock (jobQueue) {
                    if (jobQueue.Count == 0) break;
                    job = jobQueue.Dequeue();
                }

                job.AcknowledgeStart();
                OnTaskExecution(job.GetDataSet());
                job.AcknowledgeFinish();
            }

            _processingQueue.Set(false);
        }

        /// <summary> Is invoked when a task for a job is being executed. </summary>
        /// <param name="dataStore">Data store object array</param>
        private void OnTaskExecution(object[] dataStore) {
            string jobName = null;
            try {
                JobExecutionHandler onExecute = (JobExecutionHandler) dataStore[0];
                object onExecuteState = dataStore[1];
                jobName = (string) dataStore[2] ?? "unnamed";
                using (IProgressMonitor progressMonitor = UIFactory.NewService<IProgressMonitor>()) {
                    onExecute(progressMonitor, onExecuteState);
                }
            } catch (ThreadAbortException ex) {
                Thread.ResetAbort();
                _log.Fatal("Thread has been explicitly cancelled!", ex);
            } catch (Exception ex) {
                _log.Error($"Error on executing job '{jobName}'.", ex);
            }
        }

        /// <summary> Provides an anonymous implementation of <see cref="IJobHandle"/>. </summary>
        class JobHandle : IJobHandle {
            private readonly JobExecutionHandler _jobExecutionHandler;
            private readonly object _stateObject;
            private readonly string _jobName;
            private readonly ManualResetEventSlim _waitHandle;

            /// <summary> Creates a new instance that operates with the given values. </summary>
            /// <param name="onExecute"></param>
            /// <param name="stateObj"></param>
            /// <param name="jobName"></param>
            public JobHandle(JobExecutionHandler onExecute, object stateObj, string jobName) {
                _jobExecutionHandler = onExecute;
                _stateObject = stateObj;
                _jobName = jobName;
                _waitHandle = new ManualResetEventSlim(false);
            }

            /// <summary>
            /// Returns the data set of the job. The data set contains of (in order) 
            /// the <see cref="JobExecutionHandler"/>, state object and a job name - (both may be NULL).
            /// </summary>
            /// <returns>Job data set</returns>
            public object[] GetDataSet() {
                return new[] {_jobExecutionHandler, _stateObject, _jobName};
            }

            /// <inheritdoc />
            public void Wait() {
                _waitHandle.Wait();
            }

            /// <inheritdoc />
            public void Cancel() {
                throw new NotSupportedException();
            }

            /// <summary> Notifies the job that its execution has been started. </summary>
            public void AcknowledgeStart() {
                Started.Dispatch(new object[] {this, EventArgs.Empty});
            }

            /// <summary> Notifies the job that its execution has been finished. </summary>
            public void AcknowledgeFinish() {
                Finished.Dispatch(new object[] {this, EventArgs.Empty});
                SetWaitHandle();
            }

            /// <summary> Notifies the job that its execution has been cancelled. </summary>
            public void AcknowledgeCancel() {
                Cancelled.Dispatch(new object[] {this, EventArgs.Empty});
                SetWaitHandle();
            }

            /// <summary> Sets the wait handle to signalled (true). </summary>
            private void SetWaitHandle() {
                _waitHandle.Set();
            }

            /// <inheritdoc />
            public event EventHandler Cancelled;

            /// <inheritdoc />
            public event EventHandler Finished;

            /// <inheritdoc />
            public event EventHandler Started;
        }
    }
}