using System;
using motoi.platform.ui;
using motoi.platform.ui.factories;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.jobs;
using motoi.ui.windowsforms.shells;
using motoi.workbench.model;
using motoi.workbench.model.jobs;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Implements <see cref="IShellFactory"/> for the windows forms UI platform.
    /// </summary>
    public class ShellFactory : IShellFactory {
        /// <inheritdoc />
        public TShell CreateInstance<TShell>() where TShell : class, IShell {
            Type type = typeof(TShell);

            if (type.IsAssignableFrom(typeof (IMainWindow))) {
                return new MainWindow() as TShell;
            }

            if (type.IsAssignableFrom(typeof (IDialogWindow))) {
                return new DialogWindow() as TShell;
            }

            if (type.IsAssignableFrom(typeof (ITitledAreaDialog))) {
                return new TitledAreaDialog() as TShell;
            }

            if (type.IsAssignableFrom(typeof(IMessageDialogWindow))) {
                return new MessageDialogWindow() as TShell;
            }

            if (type.IsAssignableFrom(typeof(IProgressMonitor))) {
                return ProgressMonitor.GetInstance() as TShell;
            }

            if (type.IsAssignableFrom(typeof (ISingleViewPerspective))) {
                return new SingleViewPerspective() as TShell;
            }

            if (type.IsAssignableFrom(typeof (IMultiViewPerspective))) {
                return new MultiViewPerspective() as TShell;
            }

            return null; // TODO Throw exception
        }
    }
}
