using Motoi.Platform.Core;
using Motoi.UI;
using Motoi.UI.Shells;

namespace SharpFlow
{
    public class SharpFlowApplication : IMotoiApplication {

        public bool ShowSplashscreen { get { return false; } }
        
        public void OnStartup() { }

        public void OnPreInitializeMainWindow() { }

        public void OnApplicationRun() { }

        public void OnApplicationShutdown() { }

        public void OnShutdown() { }

        public void OnPostInitializeMainWindow(IMainWindow mainWindow) {
            mainWindow.WindowTitle = "SharpFlow - Business Process Modelling";
            mainWindow.WindowWidth = 1024;
            mainWindow.WindowHeight = 640;
            
        }
    }
}