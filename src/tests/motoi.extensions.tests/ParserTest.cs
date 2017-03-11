using NUnit.Framework;

namespace motoi.extensions.tests {
    [TestFixture] 
    public class ParserTest {
        [Test] 
        public void Parse() {
            const string xml =
                "<?xml version=\"1.0\" ?>" +
                "<extensions>" +
                "<extension point=\"org.motoi.command\">" +
                "<command id=\"my.first.command\" class=\"Motoi.Cmd1\" />" +
                "<command id=\"my.second.command\" class=\"Motoi.Cmd2\" />" +
                "</extension>" +
                "</extensions>";

//            MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(xml));
//            ExtensionFileParser parser = ExtensionFileParser.GetInstance(stream);
//            ExtensionPointMap extensions = parser.Parse();
//            stream.Dispose();
//
//            Assert.AreEqual(1, extensions.Count, "Wrong count of extension points");
//            IConfigurationElement[] elements = extensions["org.motoi.command"];
//            
//            Assert.AreEqual(2, elements.Length, "Wrong count of command elements");
//            Assert.AreEqual("command", elements[1].Prefix, "Wrong element prefix");
//            Assert.AreEqual("my.second.command", elements[1].Id, "Wrong element id");
//            Assert.AreEqual("Motoi.Cmd2", elements[1]["class"], "Wrong element attribute");
        }
    }
}
