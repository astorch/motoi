using NUnit.Framework;

namespace motoi.plugins.tests {
    [TestFixture]
    public class PluginManagerTests {
        [Test]
        public void Start() {
            PluginService.Instance.Start();

            Assert.AreEqual(4, PluginService.Instance.FoundPlugins.Count, "Wrong count of found plug-ins");
            Assert.AreEqual(3, PluginService.Instance.ProvidedPlugins.Count, "Wrong count of provided plug-ins");
            Assert.AreEqual(0, PluginService.Instance.ActivatedPlugins.Count, "Wrong count of activated plug-ins");
            PluginService.Instance.Stop();
        }
    }
}
