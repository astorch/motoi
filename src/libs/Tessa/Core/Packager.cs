using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ionic.Zip;
using PTP.Core;
using PTP.Parsers;

namespace Tessa.Core {
	/// <summary>
	/// Implements a packager that can be used to create Motoi plug-in packages.
	/// </summary>
	public class Packager {
        /// <summary>
        /// Backing variable of the packager instance.
        /// </summary>
	    private static Packager iInstance;

        /// <summary>
        /// Returns an instance of the packager.
        /// </summary>
	    public static Packager Instance {
            get { return iInstance ?? (iInstance = new Packager()); }
	    }

	    /// <summary>
	    /// Creates a marc file for the given Assembly within the given output path.
	    /// </summary>
	    /// <param name="csprojFile">Path of the 'csproj' file</param>
	    /// <param name="assemblyFile">Path of the assembly file</param>
	    /// <param name="outputPath">Target output path of the pack</param>
	    public FileInfo PackMarc(string csprojFile, string assemblyFile, string outputPath) {
            // Assertions
	        if (!new FileInfo(csprojFile).Exists)
                throw new NullReferenceException(string.Format("csproj file '{0}' does not exist!", csprojFile));
            if (!new FileInfo(assemblyFile).Exists)
                throw new NullReferenceException(string.Format("Assembly file '{0}' does not exist!", assemblyFile));

            // Resolve the Assembly, Meta-Inf and build properties files
            FileInfo assemblyFileInfo = new FileInfo(assemblyFile);
		    FileInfo[] metaInfFiles = ResolveMetaInfFiles(csprojFile);
	        CopyInfo[] buildPropDefinedFiles = ResolveBuildPropertiesFiles(csprojFile);

            // Resolve the output path and file info
	        string assemblyBaseName = Path.GetFileNameWithoutExtension(assemblyFile);
		    string assemblyFilePath = assemblyFileInfo.FullName;
		    string absoluteOutputPath = string.Format(AbsoluteOutputPathFormat, outputPath, assemblyBaseName);
            FileInfo marcFileInfo = new FileInfo(absoluteOutputPath);
	        DirectoryInfo marcDirectoryInfo = marcFileInfo.Directory;

            if (marcDirectoryInfo == null)
                throw new NullReferenceException(string.Format("DirectoryInfo is null for '{0}'", absoluteOutputPath));

            // If the ouput path does not exists, create it
            if (!marcDirectoryInfo.Exists)
                marcDirectoryInfo.Create();

            // Zip the contents
		    using(ZipFile zipFile = new ZipFile()) {

                // Processing the meta inf files
                for (int i = -1; ++i < metaInfFiles.Length;) {
                    FileInfo file = metaInfFiles[i];
                    string filePath = file.FullName;
                    zipFile.AddFile(filePath, ZipBasePath);
                }

                // Processing the build properties files
                for (int i = -1; ++i < buildPropDefinedFiles.Length;) {
                    CopyInfo cpyInfo = buildPropDefinedFiles[i];
                    string srcPath = cpyInfo.Source;
                    string tgtPath = cpyInfo.Target;
                    zipFile.AddFile(srcPath, tgtPath);
                }

                // Copy the assembly file
                zipFile.AddFile(assemblyFilePath, ZipBasePath);
		    	
		    	zipFile.Save(absoluteOutputPath);
		    }

	        return marcFileInfo;
        }

        /// <summary>
        /// Absolute output path format.
        /// </summary>
	    private const string AbsoluteOutputPathFormat = "{0}{1}.marc";

        /// <summary>
        /// Wildcard definition for properties file
        /// </summary>
	    private const string WildcardDefinition = "/.";

        /// <summary>
        /// Base path within the zip file.
        /// </summary>
	    private const string ZipBasePath = "./";

        /// <summary>
        /// Returns all files that has been placed within the 'meta-inf' directory within a plug-in.
        /// </summary>
        /// <param name="csprojFile">Name of the assembly</param>
        /// <returns>Collection of all files within the 'meta-inf' directory</returns>
        private FileInfo[] ResolveMetaInfFiles(string csprojFile) {
            string metaInfDirPath = GetMetaInfDir(csprojFile);
            DirectoryInfo metaInfDir = new DirectoryInfo(metaInfDirPath);
            FileInfo[] metaInfFiles = metaInfDir.GetFiles().Where(x => x.Extension != ".properties").ToArray();
            return metaInfFiles;
        }

        /// <summary>
        /// Returns a CopyInfo for all files that have to be packed through the definition of 
        /// the 'build.properties' file.
        /// </summary>
        /// <param name="csprojFile">Path of a 'csproj' file</param>
        /// <returns>Array of CopyInfo</returns>
        private CopyInfo[] ResolveBuildPropertiesFiles(string csprojFile) {
            FileInfo buildPropFile;
            if (!HasBuildProperties(csprojFile, out buildPropFile)) return new CopyInfo[0];

            string dirPath = Path.GetDirectoryName(csprojFile);
            if (string.IsNullOrEmpty(dirPath)) throw new NullReferenceException("Directory path is null");

            LinkedList<CopyInfo> cpyInfos = new LinkedList<CopyInfo>();

            IPlainTextDocument document = new PropertiesDocumentParser(buildPropFile).Parse();
            
            // Processing includes
            string[] inclDef = document.SelectValues("include") ?? new string[0];
            for (int i = -1; ++i < inclDef.Length;) {
                string include = inclDef[i];

                // Wildcard?
                if (include.EndsWith(WildcardDefinition)) {
                    include = include.Replace(WildcardDefinition, string.Empty);
                    string subDirPath = string.Format("{0}\\{1}", dirPath, include);
                    DirectoryInfo subDirInfo = new DirectoryInfo(subDirPath);
                    if (!subDirInfo.Exists) throw new NullReferenceException(string.Format("The directory '{0}' does not exist!", subDirPath));
                    FileInfo[] subDirFiles = subDirInfo.GetFiles();
                    for (int j = -1; ++j < subDirFiles.Length;) {
                        FileInfo file = subDirFiles[j];
                        string target = string.Format("{0}/includes/{1}", ZipBasePath, include);
                        CopyInfo cpyInf = new CopyInfo { Source = file.FullName, Target = target };
                        cpyInfos.AddLast(cpyInf);
                    }
                }

                // TODO non wildcard
            }

            // Processing add
            string[] addDefs = document.SelectValues("add") ?? new string[0];
            IDictionary<string, string> referencesTable = GetReferencesTable(csprojFile);
            for (int i = -1; ++i != addDefs.Length;) {
                string add = addDefs[i];

                // Wildcard usage?
                int wildcardIndex = add.IndexOf("*");

                // Handle wildcard
                if (wildcardIndex != -1) { 
                    string prefix = add.Substring(0, wildcardIndex);
                    IEnumerable<KeyValuePair<string, string>> matchingEntries = referencesTable.Where(entry => entry.Key.StartsWith(prefix));
                    using (IEnumerator<KeyValuePair<string, string>> itr = matchingEntries.GetEnumerator()) {
                        while (itr.MoveNext()) {
                            KeyValuePair<string, string> entry = itr.Current;
                            string assemblyPath = entry.Value;
                            AddToCopyList(assemblyPath, cpyInfos);
                        }
                    }
                    continue;
                }

                // Relative path?
                if (add.StartsWith("./")) {
                    // TODO Add support of relative paths
                    continue;
                }

                // Absolute path?
                if (File.Exists(add)) {
                    // TODO Add support of absolute paths
                    continue;
                }

                // Simple reference
                string referenceItem = referencesTable.FirstOrDefault(entry => entry.Key == add).Value;
                AddToCopyList(referenceItem, cpyInfos);
            }

            return cpyInfos.ToArray();
        }

        /// <summary>
        /// Creates and adds an entry of type <see cref="CopyInfo"/> to the given <paramref name="copyList"/> based 
        /// on the given <paramref name="assemblyPath"/>.
        /// </summary>
        /// <param name="assemblyPath">Path to the assembly to add</param>
        /// <param name="copyList">Collection the new entry is added</param>
	    private void AddToCopyList(string assemblyPath, LinkedList<CopyInfo> copyList) {
            string target = string.Format("{0}/includes/{1}", ZipBasePath, string.Empty);
            copyList.AddLast(new CopyInfo { Source = assemblyPath, Target = target });
	    }

        /// <summary>
        /// Returns true if the project referenced by the given 'csproj' file contains 
        /// a 'build.properties' file.
        /// </summary>
        /// <param name="csprojFile">Path of a 'csproj' file</param>
        /// <param name="buildPropFile">Reference of the 'build.properties' file if one exists</param>
        /// <returns>True or false</returns>
        private bool HasBuildProperties(string csprojFile, out FileInfo buildPropFile) {
            string metaInfDirPath = GetMetaInfDir(csprojFile);
            string buildPropFilePath = string.Format("{0}\\{1}", metaInfDirPath, "build.properties");
            FileInfo filePath = new FileInfo(buildPropFilePath);
            if (filePath.Exists) {
                buildPropFile = filePath;
                return true;
            }

            buildPropFile = null;
            return false;
        }

        /// <summary>
        /// Returns the full path of the 'Meta-Inf' directory based on the given path 
        /// to 'csproj' file.
        /// </summary>
        /// <param name="csprojFile">Path of a 'csproj' file</param>
        /// <returns>Full path of the 'Meta-Inf' directory</returns>
        private string GetMetaInfDir(string csprojFile) {
            string projectDirectory = Path.GetDirectoryName(csprojFile);
            string metaInfDirPath = string.Format("{0}\\{1}", projectDirectory, "meta-inf");
            return metaInfDirPath;
        }

        /// <summary>
        /// Creates a table of references (Name -> Full Path) for all references declared by the given <paramref name="csprojFile"/>.
        /// </summary>
        /// <param name="csprojFile">Full path to the csproj file</param>
        /// <returns>Table of references</returns>
	    private IDictionary<string, string> GetReferencesTable(string csprojFile) {
	        FileInfo csprojFileInfo = new FileInfo(csprojFile);
            if (!csprojFileInfo.Exists) throw new InvalidOperationException(string.Format("csproj file '{0}' does not exist!", csprojFileInfo));

	        string fileContent = File.ReadAllText(csprojFileInfo.FullName);
            Dictionary<string, string> referencesTable = new Dictionary<string, string>(20);
            AddExternalReferences(csprojFileInfo, fileContent, referencesTable);
            AddSolutionReferences(csprojFileInfo, fileContent, referencesTable);
	        
            return referencesTable;
	    }

        /// <summary>
        /// Parses the given <paramref name="fileContent"/> and resolves all project references. All the found references 
        /// are added to the given <paramref name="referencesTable"/>. The parameter <paramref name="csprojFileInfo"/> must point 
        /// to the file which content is provided.
        /// </summary>
        /// <param name="csprojFileInfo">Pointer to the csproj file that is processed</param>
        /// <param name="fileContent">Content of the csproj file that is processed</param>
        /// <param name="referencesTable">Collection new references are added to</param>
	    private void AddSolutionReferences(FileInfo csprojFileInfo, string fileContent, IDictionary<string, string> referencesTable) {
            Match includeMatch = Regex.Match(fileContent, "(\\<ProjectReference Include=\"(?<include>[^\"]+\"\\>))");
	        while (true) {
                // Nothing found or left, we're done here
	            if (!includeMatch.Success) break;

                // Extract the node content
	            int includeBegin = includeMatch.Index;
	            int includeFinish = fileContent.IndexOf("</ProjectReference>", includeBegin);
                string includeContent = fileContent.Substring(includeBegin, includeFinish - includeBegin);

                Match namePath = Regex.Match(includeContent, "(\\<Name\\>(?<name>[^\\<]+)\\</Name\\>)");

                // Build name
                string includeSimpleName = namePath.Groups["name"].Value;

                // Build path
                string includeReferencePath = includeMatch.Groups["include"].Value.TrimEnd('"', '>');

                // The include path is relative to the csproj file, so we must re-align it
                string basePath = csprojFileInfo.Directory.FullName;
                string projectReferencePath = Path.Combine(basePath, includeReferencePath);

                FileInfo projectFileInfo = new FileInfo(projectReferencePath);
                DirectoryInfo projectDirectoryInfo = projectFileInfo.Directory;

                // Look up latest file
                string assemblyFileName = string.Format("{0}.dll", includeSimpleName);
                FileInfo[] assemblies = projectDirectoryInfo.GetFiles(assemblyFileName, SearchOption.AllDirectories);

                // To avoid conflicts between debug and release build, we take the last written one
                FileInfo includeAssembly = assemblies.OrderByDescending(_ => _.LastWriteTime).First();

                // Add to table
                referencesTable.Add(includeSimpleName, includeAssembly.FullName);

                // Navigate to next match
                includeMatch = includeMatch.NextMatch();
	        }
	    }

        /// <summary>
        /// Parses the given <paramref name="fileContent"/> and resolves all external references. All the found references 
        /// are added to the given <paramref name="referencesTable"/>. The parameter <paramref name="csprojFileInfo"/> must point 
        /// to the file which content is provided.
        /// </summary>
        /// <param name="csprojFileInfo">Pointer to the csproj file that is processed</param>
        /// <param name="fileContent">Content of the csproj file that is processed</param>
        /// <param name="referencesTable">Collection new references are added to</param>
	    private void AddExternalReferences(FileInfo csprojFileInfo, string fileContent, IDictionary<string, string> referencesTable) {
            Match includeMatch = Regex.Match(fileContent, "(\\<Reference Include=\"(?<include>[^\"]+)\"\\>)");
            while (true) {
                // Nothing found or left, we're done here
                if (!includeMatch.Success) break;

                // Extract the node content
                int includeBegin = includeMatch.Index;
                int includeFinish = fileContent.IndexOf("</Reference>", includeBegin);
                string includeContent = fileContent.Substring(includeBegin, includeFinish - includeBegin);

                Match hintPath = Regex.Match(includeContent, "(\\<HintPath\\>(?<path>[^\\<]+)\\</HintPath\\>)");

                // Build name
                string includeFullName = includeMatch.Groups["include"].Value;
                string[] nameParts = includeFullName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string includeSimpleName = nameParts[0];

                // Build path
                string includePath = hintPath.Groups["path"].Value;

                // The include path is relative to the csproj file, so we must re-align it
                string basePath = csprojFileInfo.Directory.FullName;
                string includeFilePath = Path.Combine(basePath, includePath);

                FileInfo includeFileInfo = new FileInfo(includeFilePath);

                // Add to table
                referencesTable.Add(includeSimpleName, includeFileInfo.FullName);

                // Navigate to next match
                includeMatch = includeMatch.NextMatch();
            }
	    }

        /// <summary>
        /// Defines a simple structure for internal copying purposes.
        /// </summary>
        struct CopyInfo {
            /// <summary>
            /// Source path.
            /// </summary>
            public string Source { get; set; }
            /// <summary>
            /// Target path.
            /// </summary>
            public string Target { get; set; }
        }
	}
}