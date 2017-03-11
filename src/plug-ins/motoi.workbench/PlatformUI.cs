using motoi.platform.application;
using motoi.platform.ui.messaging;
using motoi.platform.ui.shells;
using motoi.workbench.model;
using motoi.workbench.model.jobs;
using motoi.workbench.runtime;
using motoi.workbench.runtime.jobs;
using Xcite.Csharp.generics;

namespace motoi.workbench {
    /// <summary>
    /// Provides access to UI components of the platform.
    /// </summary>
    public class PlatformUI : GenericSingleton<PlatformUI> {
        /// <summary>
        /// Returns the current workbench.
        /// </summary>
        public IWorkbench Workbench { get; private set; }

        /// <summary>
        /// Returns the current UI Invoker.
        /// </summary>
        public IUIInvoker Invoker { get; private set; }

        /// <summary>
        /// Returns the current job service.
        /// </summary>
        public IJobService JobService { get; private set; }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            IMainWindow mainWindow = Platform.Instance.MainWindow;
            Workbench = new Workbench(mainWindow);
            Invoker = mainWindow;
            JobService = new JobService();
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}