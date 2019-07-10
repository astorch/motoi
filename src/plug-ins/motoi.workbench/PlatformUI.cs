using motoi.platform.application;
using motoi.platform.ui.messaging;
using motoi.platform.ui.shells;
using motoi.workbench.model;
using motoi.workbench.model.jobs;
using motoi.workbench.runtime;
using motoi.workbench.runtime.jobs;
using xcite.csharp;

namespace motoi.workbench {
    /// <summary> Provides access to UI components of the platform. </summary>
    public class PlatformUI : GenericSingleton<PlatformUI> {
        /// <summary> Returns the current workbench. </summary>
        public IWorkbench Workbench { get; private set; }

        /// <summary> Returns the current UI Invoker. </summary>
        public IUIInvoker Invoker { get; private set; }

        /// <summary> Returns the current job service. </summary>
        public IJobService JobService { get; private set; }
        
        /// <inheritdoc />
        protected override void OnInitialize() {
            IMainWindow mainWindow = Platform.Instance.MainWindow;
            Workbench = new Workbench(mainWindow);
            Invoker = mainWindow;
            JobService = new JobService();
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}