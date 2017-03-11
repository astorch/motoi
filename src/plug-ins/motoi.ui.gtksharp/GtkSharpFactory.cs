using System;
using Gtk;
using motoi.platform.ui;
using motoi.platform.ui.shells;

namespace Motoi.UI.GtkSharp {
    /// <summary>
    /// Implements <see cref="IViewPartFactory"/> to create Motoi-UI instances 
    /// for Gtk#.
    /// </summary>
    public class GtkSharpFactory : IViewPartFactory {

        private bool iInitialized;

        /// <inheritdoc />
        public void RunApplication(IMainWindow mainWindow) {
            Window wnd = mainWindow as Window;
            if (wnd == null)
                return;

            wnd.ShowAll();
            wnd.DeleteEvent += (o, args) => Application.Quit();
            Application.Run();
        }

        /// <inheritdoc />
        public TViewPart CreateInstance<TViewPart>() where TViewPart : class, IViewPart {
            if (!iInitialized) {
                Application.Init();
                iInitialized = true;
            }

            Type type = typeof(TViewPart);

            if (type.IsAssignableFrom(typeof(IMainWindow)))
                return new Window();

            return null;
        }
    }
}