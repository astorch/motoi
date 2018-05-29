using System.IO;
using System.Text;
using NUnit.Framework;
using PTP;

namespace PTP.tests {
	[TestFixture]
	public class PlainTextParserTests {
		[Test]
		public void Parse_StringManifestStream() {
			string manifest = 
				"Vendor: Arian Storch;\r\n" +
				"Name: TestPlugin;\r\n" +
				"Version: 1.0.0;\r\n" +
				"Dependencies: org.demo.1, org.demo.2;";
			
			IPlainTextDocument document = PlainTextParser.Instance.
                Parse(new MemoryStream(Encoding.Default.GetBytes(manifest)), new string[0], new[] { ";" }, new[] { "/" }, new[] { "," });
			
			string vendorValue = document.SelectValue("vendor");
			string nameValue = document.SelectValue("name");
			string versionValue = document.SelectValue("version");
			string[] dependenciesValue = document.SelectValues("dependencies");
			
			Assert.AreEqual("Arian Storch", vendorValue, "Vendor value is wrong");
			Assert.AreEqual("TestPlugin", nameValue, "Name value is wrong");
			Assert.AreEqual("1.0.0", versionValue, "Version value is wrong");
			
			Assert.AreEqual(2, dependenciesValue.Length, "Unexpected count of depencendies value");
			Assert.AreEqual("org.demo.1", dependenciesValue[0], "Dependency value 1 is wrong");
			Assert.AreEqual("org.demo.2", dependenciesValue[1], "Dependency value 2 is wrong");
		}

        [Test]
        public void Parse_ManifestFile() {
            FileInfo fileInfo = new FileInfo("./Additionals/signatur_test.mf");

            IPlainTextDocument document = PlainTextParser.Instance.
                Parse(fileInfo, new string[0], new[] { ";" }, new[] { "/" }, new[] { "," });

            string vendorValue = document.SelectValue("vendor");
            string nameValue = document.SelectValue("name");
            string versionValue = document.SelectValue("version");
            string[] dependenciesValue = document.SelectValues("dependencies");

            Assert.AreEqual("Arian Storch", vendorValue, "Vendor value is wrong");
            Assert.AreEqual("TestPlugin", nameValue, "Name value is wrong");
            Assert.AreEqual("1.0.0", versionValue, "Version value is wrong");

            Assert.AreEqual(2, dependenciesValue.Length, "Unexpected count of depencendies value");
            Assert.AreEqual("org.demo.1", dependenciesValue[0], "Dependency value 1 is wrong");
            Assert.AreEqual("org.demo.2", dependenciesValue[1], "Dependency value 2 is wrong");
        }
	}
}