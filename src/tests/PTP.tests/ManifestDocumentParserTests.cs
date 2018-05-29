using System.IO;
using NUnit.Framework;
using PTP;

namespace PTP.tests {
    [TestFixture]
    public class ManifestDocumentParserTests {
        [Test]
        public void Parse() {
            FileInfo fileInfo = new FileInfo("./Additionals/signatur_simple.mf");
            ManifestDocumentParser parser = new ManifestDocumentParser(fileInfo);
            IPlainTextDocument document = parser.Parse();

            string nameValue = document.SelectValue("name");
            string symbolicValue = document.SelectValue("symbolicName");
            string versionValue = document.SelectValue("version");
            string activatorValue = document.SelectValue("activator");
            string vendorValue = document.SelectValue("vendor");
            string[] dependenciesValue = document.SelectValues("dependencies");

            Assert.AreEqual("TestBundle", nameValue, "Name value is wrong");
            Assert.AreEqual("org.ptp4net.testbundle", symbolicValue, "Symbolic value is wrong");
            Assert.AreEqual("0.0.1", versionValue, "Version value is wrong");
            Assert.AreEqual("-", activatorValue, "Activator value is wrong");
            Assert.AreEqual("PTP dev team", vendorValue, "Vendor value is wrong");
            Assert.AreEqual("-", dependenciesValue[0], "Dependencies value is wrong");
        }
    }
}