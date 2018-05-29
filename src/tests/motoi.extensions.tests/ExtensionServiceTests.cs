using NUnit.Framework;

namespace motoi.extensions.tests {
    [TestFixture]
    class ExtensionServiceTests {

        [Test]
        public void Start() {
            ExtensionService.Instance.Start();

            IConfigurationElement[] elements = ExtensionService.Instance.GetConfigurationElements("org.motoi.command");

            Assert.AreEqual(1, elements.Length, "Wrong count of elements");
            Assert.AreEqual("my.first.command", elements[0].GetAttributeValue("id"), "Wrong id attribute");
            Assert.AreEqual("Motoi.Test.CmdImpl", elements[0].GetAttributeValue("class"), "Wrong class attribute");

            ExtensionService.Instance.Stop();
        }
    }
}
