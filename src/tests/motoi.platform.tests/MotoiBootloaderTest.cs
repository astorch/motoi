using motoi.platform;
using motoi.platform.annotations;
using motoi.platform.model;
using motoi.ui.shells;
using NUnit.Framework;

namespace Motoi.PlatformTest
{
    [TestFixture]
    public class MotoiBootloaderTest {
        [Test]
        public void Test_PlatformSettings_Load() {
            MotoiBootloader.Main(null);

            Assert.IsNotNull(TestApplicationImplementation.Instance);
            Assert.IsTrue(TestApplicationImplementation.Instance.PlatformSettingsValid);
        }

        [Test]
        public void Test_ApplicationManager_DoWork() {
            MotoiBootloader.Main(null);

            Assert.IsNotNull(TestApplicationImplementation.Instance);
            Assert.IsTrue(TestApplicationImplementation.Instance.OnStartupCalled);
            Assert.IsTrue(TestApplicationImplementation.Instance.OnApplicationRunCalled);
            Assert.IsTrue(TestApplicationImplementation.Instance.OnShutdownCalled);
        }
    }

    [MotoiApplication("test.application")]
    public class TestApplicationImplementation : IMotoiApplication {

        public static TestApplicationImplementation Instance { get; private set; }

        public TestApplicationImplementation() {
            Instance = this;
            AssertPlatformSettings();
        }

        public bool OnStartupCalled { get; set; }

        public bool OnApplicationRunCalled { get; set; }

        public bool OnShutdownCalled { get; set; }

        public bool PlatformSettingsValid { get; set; }

        public void AssertPlatformSettings() {
            Assert.AreEqual("test.application", PlatformSettings.Instance["application"]);
            Assert.AreEqual("de", PlatformSettings.Instance["nl"]);
            PlatformSettingsValid = true;
        }

        public bool ShowSplashscreen { get { return false;} }

        /// <summary>
        /// Return TRUE if the application shall run headless. In this case, no main window 
        /// is being initialized.
        /// </summary>
        public bool IsHeadless { get { return false; } }

        public void OnStartup() {
            OnStartupCalled = true;
        }

        public void OnPreInitializeMainWindow() { }

        public void OnPostInitializeMainWindow(IMainWindow mainWindow) { }

        public void OnApplicationRun() {
            OnApplicationRunCalled = true;
        }

        public void OnApplicationShutdown() { }

        public void OnShutdown() {
            OnShutdownCalled = true;
        }
    }
}
