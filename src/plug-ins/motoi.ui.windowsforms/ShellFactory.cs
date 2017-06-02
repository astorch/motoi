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

            if (typeof(IMainWindow).IsAssignableFrom(type))
                return new MainWindow() as TShell;

            if (typeof(ITitledAreaDialog).IsAssignableFrom(type))
                return new TitledAreaDialog() as TShell;

            if (typeof(IExceptionDialog).IsAssignableFrom(type))
                return new ExceptionDialog() as TShell;

            if (typeof(IMessageDialog).IsAssignableFrom(type))
                return new MessageDialog() as TShell;

            if (typeof(IDialogWindow).IsAssignableFrom(type))
                return new DialogWindow() as TShell;

            return null; // TODO Throw exception
        }
    }
}
