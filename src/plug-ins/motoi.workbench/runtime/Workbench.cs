using System;
using motoi.platform.ui;
using motoi.platform.ui.shells;
using motoi.workbench.events;
using motoi.workbench.model;
using xcite.csharp.oop;
using xcite.logging;

namespace motoi.workbench.runtime {
    /// <summary> Provides an implementation of <see cref="IWorkbench"/>. </summary>
    public class Workbench : IWorkbench {
        private static readonly ILog _log = LogManager.GetLog(typeof(Workbench));

        private readonly AuxiliaryAudible<IWorkbenchListener> _workbenchEventManager = new AuxiliaryAudible<IWorkbenchListener>();

        /// <summary>Creates a new instance of the given <paramref name="mainWindow"/></summary>
        /// <param name="mainWindow">Main window of the application</param>
        public Workbench(IMainWindow mainWindow) {
            MainWindow = mainWindow;
        }


        /// <inheritdoc />
        public IMainWindow MainWindow { get; }
        
        /// <inheritdoc />
        public IPerspective ActivePerspective { get; private set; }
        
        /// <inheritdoc />
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
            _workbenchEventManager.Dispatch(lstnr => lstnr.OnPerspectiveChanged(oldPerspective, newPerspective), OnPerspectiveChangedException);

            if (newPerspective != null) {
                // Currently nothing to do
            }

            return newPerspective;
        }

        /// <summary> Is invoked when an error during the event dispatching occurred. </summary>
        /// <param name="exception">Exception</param>
        /// <param name="workbenchListener">Listener the exception happened to</param>
        private void OnPerspectiveChangedException(Exception exception, IWorkbenchListener workbenchListener) {
            _log.Error($"Error on dispatching event to '{workbenchListener}'.", exception);
        }
        
        /// <inheritdoc />
        public void AddWorkbenchListener(IWorkbenchListener listener) {
            _workbenchEventManager.AddListener(listener);
        }
        
        /// <inheritdoc />
        public void RemoveWorkbenchListener(IWorkbenchListener listener) {
            _workbenchEventManager.RemoveListener(listener);
        }
    }
}