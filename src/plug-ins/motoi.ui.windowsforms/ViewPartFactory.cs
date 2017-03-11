using System;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.jobs;
using motoi.ui.windowsforms.shells;
using motoi.workbench.model;
using motoi.workbench.model.jobs;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Implements <see cref="IViewPartFactory"/> to create Motoi-UI conform instances 
    /// using Windows Forms
    /// </summary>
    public class ViewPartFactory : IViewPartFactory {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ViewPartFactory() {
            // TODO Move to another lifecycle conform position
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TViewPart">Type of the view part to create</typeparam>
        /// <returns>Newly created instance of the given type</returns>
        public TViewPart CreateInstance<TViewPart>() 
            where TViewPart : class, IViewPart {
            Type type = typeof(TViewPart);

            if (type.IsAssignableFrom(typeof (IMainWindow))) {
                return new MainWindow() as TViewPart;
            }

            if (type.IsAssignableFrom(typeof (IDialogWindow))) {
                return new DialogWindow() as TViewPart;
            }

            if (type.IsAssignableFrom(typeof (ITitledAreaDialog))) {
                return new TitledAreaDialog() as TViewPart;
            }

            if (type.IsAssignableFrom(typeof(IMessageDialogWindow))) {
                return new MessageDialogWindow() as TViewPart;
            }

            if (type.IsAssignableFrom(typeof(IProgressMonitor))) {
                return ProgressMonitor.GetInstance() as TViewPart;
            }

            if (type.IsAssignableFrom(typeof (ISingleViewPerspective))) {
                return new SingleViewPerspective() as TViewPart;
            }

            if (type.IsAssignableFrom(typeof (IMultiViewPerspective))) {
                return new MultiViewPerspective() as TViewPart;
            }

            return null; // TODO Throw exception
        }

        /// <summary>
        /// Tells the factory to start the message dispatching for the given main window.
        /// </summary>
        /// <param name="mainWindow">Main window of the application</param>
        public void RunApplication(IMainWindow mainWindow) {
            ApplicationContext applicationContext = new ApplicationContext(mainWindow as Form);
            Application.Run(applicationContext);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
