using System;
using motoi.platform.ui;
using motoi.platform.ui.factories;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.shells;

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

            return null; // TODO Throw exception
        }
    }
}
