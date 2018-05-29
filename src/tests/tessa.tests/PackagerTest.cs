using System;
using System.IO;
using NUnit.Framework;
using Tessa;

namespace tessa.tests {
	[TestFixture]
	public class PackagerTest {
		[Test]
		public void PackMarc() {
		    string currentDir = Directory.GetCurrentDirectory();
		    string pluginOutputPath = string.Format("{0}\\plug-ins\\", currentDir);

		    string dpPath = GetFullProjectFilePath("DemoPlugin");
		    string dprPath = GetFullProjectFilePath("DemoPluginRef");
		    string dpiPath = GetFullProjectFilePath("DemoPluginInvalid");

		    string dpAssPath = GetAssemblyFilePath("DemoPlugin");
		    string dprAssPath = GetAssemblyFilePath("DemoPluginRef");
		    string dpiAssPath = GetAssemblyFilePath("DemoPluginInvalid");

			FileInfo pluginPackage1 = Packager.Instance.PackMarc(dpPath, dpAssPath, pluginOutputPath);
            FileInfo pluginPackage2 = Packager.Instance.PackMarc(dprPath, dprAssPath, pluginOutputPath);
            FileInfo pluginPackage3 = Packager.Instance.PackMarc(dpiPath, dpiAssPath, pluginOutputPath);

            Assert.IsTrue(pluginPackage1.Exists);
            Assert.IsTrue(pluginPackage2.Exists);
            Assert.IsTrue(pluginPackage3.Exists);
        }

        private string GetFullProjectFilePath(string projectName) {
            DirectoryInfo directoryInfo = new DirectoryInfo("..\\..\\Motoi\\tests\\plug-ins");
            string projectFilePath = string.Format("{0}\\{1}\\{1}.csproj", directoryInfo.FullName, projectName);
            if(!new FileInfo(projectFilePath).Exists)
                throw new NullReferenceException(string.Format("File '{0}' does not exist!", projectFilePath));
            return projectFilePath;
        }

        private string GetAssemblyFilePath(string projectName) {
            DirectoryInfo directoryInfo = new DirectoryInfo(@".\");
            string assemblyFilePath = string.Format("{0}{1}.dll", directoryInfo.FullName, projectName);
            if(!new FileInfo(assemblyFilePath).Exists)
                throw new NullReferenceException(string.Format("File '{0}' does not exist!", assemblyFilePath));
            return assemblyFilePath;
        }
	}
}
