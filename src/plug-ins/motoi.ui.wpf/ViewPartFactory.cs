using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using motoi.platform.ui;
using motoi.platform.ui.shells;

namespace Motoi.UI.WPF
{
    /// <summary>
    /// Implements <see cref="IViewPartFactory"/> to create Motoi-UI instances 
    /// for WPF.
    /// </summary>
    public class ViewPartFactory : IViewPartFactory {

        private Window iMainWindow;
        private static Dispatcher iUiThreadDispatcher;

        /// <summary>
        /// Initializes the UI Thread.
        /// </summary>
        static ViewPartFactory() {
            Thread wpfUiThread = new Thread(OnUIThreadStart);
            wpfUiThread.SetApartmentState(ApartmentState.STA);
            wpfUiThread.Name = "Motoi WPF UI Thread";
            wpfUiThread.Start();
        }

        /// <summary>
        /// Will be called when the UI has been started.
        /// </summary>
        private static void OnUIThreadStart()
        {
            iUiThreadDispatcher = Dispatcher.CurrentDispatcher;
            Dispatcher.Run();
        }

        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TViewPart">Type of the view part to create</typeparam>
        /// <returns>Newly created instance of the given type</returns>
        public TViewPart CreateInstance<TViewPart>() where TViewPart : class, IViewPart {
            if (!iUiThreadDispatcher.CheckAccess()) {
                TViewPart viewPart = null;
                RunOnUIThread(() => viewPart = CreateInstance<TViewPart>());
                return viewPart;
            }

            Type viewPartType = typeof (TViewPart);
            if (typeof(IWindow).IsAssignableFrom(viewPartType)) {
                Type implementingType = GetImplementingType(viewPartType);
                WindowModel windowModel = (WindowModel) Activator.CreateInstance(implementingType);
                Window window = new Window { DataContext = windowModel };
                windowModel.OnShow += (sender, args) => RunOnUIThread(window.Show);
                windowModel.OnClose += (sender, args) => RunOnUIThread(window.Close);

                if (viewPartType == typeof (IMainWindow)) {
                    iMainWindow = window;
                }

                if (viewPartType == typeof (IDialogWindow)) {
                    // TODO
                }

                IViewPart viewPartRef = windowModel;
                return (TViewPart)viewPartRef;
            }

            return null;
        }

        /// <summary>
        /// Returns the implementing type of the given interface type.
        /// </summary>
        /// <param name="interfaceType">Interface type to resolve</param>
        /// <returns>Implementing type</returns>
        private Type GetImplementingType(Type interfaceType) {
            if (interfaceType == typeof (IMainWindow)) return typeof (WindowModel);
            if (interfaceType == typeof (IDialogWindow)) return typeof (DialogWindowModel);
            throw new NotSupportedException(string.Format("Given interface type '{0}' is not known by the factory!", interfaceType));
        }

        /// <summary>
        /// Invokes the given action within the UI thread.
        /// </summary>
        /// <param name="action">Delegate to invoke within the UI thread</param>
        private void RunOnUIThread(Action action) {
            iUiThreadDispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        /// <summary>
        /// Tells the factory to start the message dispatching for the given main window.
        /// </summary>
        /// <param name="mainWindow">Main window of the application</param>
        public void RunApplication(IMainWindow mainWindow) {
            RunOnUIThread(() => new Application {ShutdownMode = ShutdownMode.OnMainWindowClose}.Run(iMainWindow));
        }
    }
}
