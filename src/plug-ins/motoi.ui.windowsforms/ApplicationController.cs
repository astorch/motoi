using System.Windows.Forms;
using motoi.platform.ui.factories;
using motoi.platform.ui.shells;

namespace motoi.ui.windowsforms {
    /// <summary> Implements <see cref="IApplicationController"/> for the windows forms UI platform. </summary>
    public class ApplicationController : IApplicationController {
        /// <inheritdoc />
        public void RunApplication(IMainWindow mainWindow) {
            ApplicationContext applicationContext = new ApplicationContext(mainWindow as Form);
            Application.Run(applicationContext);
        }

        /// <inheritdoc />
        public void ShutdownApplication() {
            Application.Exit();
        }
    }
}