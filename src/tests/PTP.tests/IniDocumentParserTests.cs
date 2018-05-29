using System.IO;
using System.Text;
using NUnit.Framework;
using PTP;

namespace PTP.tests {
    [TestFixture]
    public class IniDocumentParserTests {
         
        [Test]
        public void Parse() {
            const string fileContent = "application: motoi.test\r\n" +
                                       "nl: de";

            MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(fileContent));
            IniDocumentParser parser = new IniDocumentParser(stream);
            IPlainTextDocument document =  parser.Parse();
            stream.Dispose();

            string applicationValue = document.SelectValue("application");
            string nlValue = document.SelectValue("nl");

            Assert.AreEqual("motoi.test", applicationValue, "Wrong application value");
            Assert.AreEqual("de", nlValue, "Wrong nl value");
        }
    }
}