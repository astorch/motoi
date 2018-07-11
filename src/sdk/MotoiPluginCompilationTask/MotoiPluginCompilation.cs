using System;
using System.IO;
using Microsoft.Build.Framework;

namespace MotoiPluginCompilationTask {
    /// <summary>
    /// Implements a <see cref="ITask"/> that compiles a Visual Studio Project as 
    /// Motoi plug-in.
    /// </summary>
    public class MotoiPluginCompilation : Microsoft.Build.Utilities.Task {
        /// <summary> Returns the set motoi binary folder. </summary>
        [Required]
        public string MotoiBinFolder { get; set; }

        /// <summary> Returns the set motoi source folder. </summary>
        [Required]
        public string MotoiSrcFolder { get; set; }

        /// <summary> Returns the Tessa file path. </summary>
        [Required]
        public string TessaFilePath { get; set; }

        /// <summary> Target path the project is compiled to. </summary>
        [Required]
        public string TargetPath { get; set; }

        /// <summary> Returns TRUE if the task runs in debug mode. </summary>
        public bool Debug { get; set; }

        /// <inheritdoc />
        public override bool Execute() {
            if (Debug) System.Diagnostics.Debugger.Launch();

            if (string.IsNullOrEmpty(MotoiBinFolder)) throw new ArgumentNullException(nameof(MotoiBinFolder));
            if (string.IsNullOrEmpty(MotoiSrcFolder)) throw new ArgumentNullException(nameof(MotoiSrcFolder));
            if (string.IsNullOrEmpty(TessaFilePath)) throw new ArgumentNullException(nameof(TessaFilePath));
            if (string.IsNullOrEmpty(TargetPath)) throw new ArgumentNullException(nameof(TargetPath));

            Log.LogMessage(MessageImportance.Normal, "Starting Motoi Plug-in compilation task");
            Log.LogMessage(MessageImportance.Normal, "MotoiBinFolder: {0}", MotoiBinFolder);
            Log.LogMessage(MessageImportance.Normal, "MotoiSrcFolder: {0}", MotoiSrcFolder);
            Log.LogMessage(MessageImportance.Normal, "TessaFilePath: {0}", TessaFilePath);

            if (!File.Exists(TessaFilePath)) throw new ArgumentException("Tessa.exe not found. Check the file path!");
            if (!Directory.Exists(MotoiSrcFolder)) throw new ArgumentException("Motoi src folder not found. Check the folder path!");
            
            try {
                CopyDirectory(MotoiSrcFolder, MotoiBinFolder, true, false);
            } catch (Exception ex) {
                Log.LogError("Error on copying files of motoi src folder to motoi bin folder");
                Log.LogErrorFromException(ex);
                return false;
            }

            string projectPath = BuildEngine.ProjectFileOfTaskNode;
            string targetPath = TargetPath;

            try {
                string argLine = $"\"{projectPath}\" \"{targetPath}\" \"{MotoiBinFolder}\"";
                Log.LogMessage(MessageImportance.Normal, "Starting tessa using: {0} {1}", TessaFilePath, argLine);
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(TessaFilePath, argLine);
                Log.LogMessage(MessageImportance.Normal, "Tessa started. Waiting for finish...");
                process.WaitForExit();
                int exitCode = process.ExitCode;
                if (exitCode != 0) {
                    Log.LogError("Tessa finished unsuccessfully. Exit code is {0}", exitCode);
                    return false;
                }
                Log.LogMessage(MessageImportance.Normal, "Tessa process has finished successfully");
                return true;
            } catch (Exception ex) {
                Log.LogError("Error on starting tessa process");
                Log.LogErrorFromException(ex);
                return false;
            }
        }

        /// <summary>
        /// Copies all files of the given <paramref name="sourceDirName"/> to the given <paramref name="destDirName"/>. 
        /// If <paramref name="copySubDirs"/> is set TRUE, so all sub directories are copied as well. If <paramref name="overrideFile"/> 
        /// is set TRUE, existing files will be overriden.
        /// </summary>
        /// <param name="sourceDirName">Path of the source directory</param>
        /// <param name="destDirName">Path of the destination directory</param>
        /// <param name="copySubDirs">TRUE if sub directories shall be copied too</param>
        /// <param name="overrideFile">TRUE if existing files shall be overriden</param>
        private static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs, bool overrideFile) {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists) throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            
            DirectoryInfo[] dirs = dir.GetDirectories();
            
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            for (int i = -1; ++i != files.Length;) {
                FileInfo file = files[i];
                string temppath = Path.Combine(destDirName, file.Name);
                if (!overrideFile && File.Exists(temppath)) continue;
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs) {
                for (int i = -1; ++i != dirs.Length;) {
                    DirectoryInfo subdir = dirs[i];
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, temppath, true, overrideFile);
                }
            }
        }
    }
}
