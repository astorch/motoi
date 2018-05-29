using System;
using motoi.platform.ui;
using motoi.platform.ui.shells;
using motoi.workbench.events;
using motoi.workbench.model;
using NLog;
using xcite.csharp.oop;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an implementation of <see cref="IWorkbench"/>.
    /// </summary>
    public class Workbench : IWorkbench {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly AuxiliaryAudible<IWorkbenchListener> iWorkbenchEventManager = new AuxiliaryAudible<IWorkbenchListener>();

        /// <summary>Creates a new instance of the given <paramref name="mainWindow"/></summary>
        /// <param name="mainWindow">Main window of the application</param>
        public Workbench(IMainWindow mainWindow) {
            MainWindow = mainWindow;
        }

        /// <summary>
        /// Returns the current main window.
        /// </summary>
        public IMainWindow MainWindow { get; private set; }

        /// <summary>
        /// Returns the current active perspective.
        /// </summary>
        public IPerspective ActivePerspective { get; private set; }

        /// <summary>
        /// Opens a perspective with the given perspective id.
        /// </summary>
        /// <param name="perspectiveId">Id of the perspective</param>
        /// <returns>Instance of perspective or null</returns>
        public IPerspective OpenPerspective(string perspectiveId) {
            if (string.IsNullOrEmpty(perspectiveId)) return null;

            IPerspective newPerspective = PerspectiveFactory.Instance.GetPerspective(perspectiveId);
            IWidgetCompound widgetCompound = newPerspective.GetPanel();
            MainWindow.SetContent(widgetCompound);

            IPerspective oldPerspective = ActivePerspective;

            if (oldPerspective != null) {
                // Currently nothing to do
            }

            ActivePerspective = newPerspective;
            iWorkbenchEventManager.Dispatch(lstnr => lstnr.OnPerspectiveChanged(oldPerspective, newPerspective), OnPerspectiveChangedException);

            if (newPerspective != null) {
                // Currently nothing to do
            }

            return newPerspective;
        }

        /// <summary>
        /// Is invoked when an error during the event dispatching occurred.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="workbenchListener">Listener the exception happened to</param>
        private void OnPerspectiveChangedException(Exception exception, IWorkbenchListener workbenchListener) {
            _log.Error(exception, $"Error on dispatching event to '{workbenchListener}'.");
        }

        /// <summary>
        /// Subscribes the given <paramref name="listener"/> to workbench events.
        /// </summary>
        /// <param name="listener">Listener to subscribe</param>
        public void AddWorkbenchListener(IWorkbenchListener listener) {
            iWorkbenchEventManager.AddListener(listener);
        }

        /// <summary>
        /// Unsubscribes the given <paramref name="listener"/> from workbench events.
        /// </summary>
        /// <param name="listener">Listener to unsubscribe</param>
        public void RemoveWorkbenchListener(IWorkbenchListener listener) {
            iWorkbenchEventManager.RemoveListener(listener);
        }
    }
}