using System;
using System.Collections.Generic;
using System.Threading;
using log4net;
using motoi.platform.ui.factories;
using motoi.workbench.model.jobs;
using xcite.csharp;

namespace motoi.workbench.runtime.jobs {
    /// <summary>
    /// Provides an implementation of <see cref="IJobService"/>.
    /// </summary>
    public class JobService : IJobService {
        private readonly ILog iLogWriter = LogManager.GetLogger(typeof(JobService));
        private readonly Queue<JobHandle> iJobQueue = new Queue<JobHandle>(7);
        private readonly AutoLockStruct<bool> iProcessingQueue = new AutoLockStruct<bool>();

        /// <inheritdoc />
        public IJobHandle Schedule(JobExecutionHandler onExecute, object state, string jobName) {
            if (onExecute == null) throw new ArgumentNullException(nameof(onExecute));

            JobHandle jobHandle = new JobHandle(onExecute, state, jobName);
            lock (iJobQueue) {
                iJobQueue.Enqueue(jobHandle);
            }

            using (iProcessingQueue.Lock()) {
                if (!iProcessingQueue.Get()) {
                    iProcessingQueue.Set(true);
                    ThreadPool.QueueUserWorkItem(ProcessJobQueue, iJobQueue);
                }
            }

            return jobHandle;
        }

        /// <summary>
        /// Is invoked when a thread to process the job queue has been started.
        /// </summary>
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

            iProcessingQueue.Set(false);
        }

        /// <summary>
        /// Is invoked when a task for a job is being executed.
        /// </summary>
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
                iLogWriter.Fatal("Thread has been explicitly cancelled!", ex);
            } catch (Exception ex) {
                iLogWriter.ErrorFormat("Error on executing job '{0}'. Reason: {1}", jobName, ex);
            }
        }

        /// <summary>
        /// Provides an anonymous implementation of <see cref="IJobHandle"/>.
        /// </summary>
        class JobHandle : IJobHandle {
            private readonly JobExecutionHandler iJobExecutionHandler;
            private readonly object iStateObject;
            private readonly string iJobName;
            private readonly ManualResetEventSlim iWaitHandle;

            /// <summary>
            /// Creates a new instance that operates with the given values.
            /// </summary>
            /// <param name="onExecute"></param>
            /// <param name="stateObj"></param>
            /// <param name="jobName"></param>
            public JobHandle(JobExecutionHandler onExecute, object stateObj, string jobName) {
                iJobExecutionHandler = onExecute;
                iStateObject = stateObj;
                iJobName = jobName;
                iWaitHandle = new ManualResetEventSlim(false);
            }

            /// <summary>
            /// Returns the data set of the job. The data set contains of (in order) 
            /// the <see cref="JobExecutionHandler"/>, state object and a job name - (both may be NULL).
            /// </summary>
            /// <returns>Job data set</returns>
            public object[] GetDataSet() {
                return new[] {iJobExecutionHandler, iStateObject, iJobName};
            }

            /// <inheritdoc />
            public void Wait() {
                iWaitHandle.Wait();
            }

            /// <inheritdoc />
            public void Cancel() {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Notices the job that its execution has been started.
            /// </summary>
            public void AcknowledgeStart() {
                Started.Dispatch(new object[] {this, EventArgs.Empty});
            }

            /// <summary>
            /// Notices the job that its execution has been finished.
            /// </summary>
            public void AcknowledgeFinish() {
                Finished.Dispatch(new object[] {this, EventArgs.Empty});
                SetWaitHandle();
            }

            /// <summary>
            /// Notices the job that its execution has been cancelled.
            /// </summary>
            public void AcknowledgeCancel() {
                Cancelled.Dispatch(new object[] {this, EventArgs.Empty});
                SetWaitHandle();
            }

            /// <summary>
            /// Sets the wait handle to signalled (true).
            /// </summary>
            private void SetWaitHandle() {
                iWaitHandle.Set();
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