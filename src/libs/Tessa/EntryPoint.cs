using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Tessa {
    /// <summary> Provides the entry point for the application. </summary>
    class EntryPoint {
        /// <summary> Implements the entry point of the application. </summary>
        /// <param name="args">Execution arguments</param>
        static void Main(string[] args) {
            // TODO Print options to console
            if (args.Length == 0) return;

            if (args.Any(arg => arg == "debug"))
                Debugger.Launch();
            
            /* (1) Path to csproj
             * (2) Path to compiled assembly
             * (3) [Optional] Path to ouput directory
             */

            // Default output directory
            string currentDir = Directory.GetCurrentDirectory();

            // Check for custom output directory
            if (args.Length > 2) {
                string customOutputPath = args[2];
                DirectoryInfo directoryInfo = new DirectoryInfo(customOutputPath);
                if (!directoryInfo.Exists) 
                    throw new InvalidOperationException($"Custom output path '{directoryInfo.FullName}' does not exist");

                currentDir = directoryInfo.FullName;
            }

            // Build output directory path
            string pluginOutputPath = $@"{currentDir}\\plug-ins\\";

            // Perform packaging
            Packager.Instance.PackMarc(args[0], args[1], pluginOutputPath);

            // Print result to console
            FileInfo csprojFileInfo = new FileInfo(args[0]);
            string fileName = Path.GetFileNameWithoutExtension(csprojFileInfo.FullName);
            string marcFilePath = pluginOutputPath.Replace(@"\\", @"\") + fileName + ".marc";

            Console.WriteLine("[Tessa] {0} -> {1}", fileName, marcFilePath);
        }
    }
}