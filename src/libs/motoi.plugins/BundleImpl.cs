using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using xcite.collections;

namespace motoi.plugins {
    /// <summary> Provides an implementation of <see cref="IBundle"/> </summary>
    class BundleImpl : IBundle {
        private readonly FileInfo iMarcFile;
        private Assembly iBundledAssembly;
        private string[] iResources;

        /// <summary>
        /// Creates a new instance for the given marc file.
        /// </summary>
        /// <param name="marcFile">Marc file</param>
        public BundleImpl(FileInfo marcFile) {
            if (marcFile == null)
                throw new NullReferenceException("Marc file is null!");

            if (!marcFile.Exists)
                throw new NullReferenceException(string.Format("Marc file does not exist! Path is '{0}'",
                    marcFile.FullName));

            iMarcFile = marcFile;
            Name = Path.GetFileNameWithoutExtension(iMarcFile.FullName);
        }

        /// <summary>
        /// Returns a stream to the given resource within the bundle.
        /// </summary>
        /// <param name="resource">Path of the resource within the bundle</param>
        /// <returns>Stream or null</returns>
        public Stream GetResourceAsStream(string resource) {
            string filePath = iMarcFile.FullName;

            using (ZipFile zipFile = new ZipFile(filePath)) {
                ZipEntry signatureZipEntry = zipFile.GetEntry(resource);
                if (signatureZipEntry == null) return null;

                MemoryStream buffer = new MemoryStream(new byte[signatureZipEntry.Size]);
                using (Stream entryStream = zipFile.GetInputStream(signatureZipEntry)) {
                    entryStream.CopyTo(buffer);
                }

                // Rewind to make it readable
                buffer.Position = 0;
                return buffer;
            }
        }

        /// <summary>
        /// Returns a stream for the given resource that has been packed within the assembly 
        /// as embedded resource.
        /// </summary>
        /// <param name="resource">Path to embedded resource within the assembly</param>
        /// <returns>Stream or null</returns>
        public Stream GetAssemblyResourceAsStream(string resource) {
            if (iBundledAssembly == null)
                ResolveBundledAssembly();

            string translatedResourceName = resource.Replace('/', '.');
            string assemblyQualifiedName = string.Format("{0}.{1}", Name, translatedResourceName);

            if(iBundledAssembly == null) throw new NullReferenceException("Bundled assembly is null");

            Stream resourceStream = iBundledAssembly.GetManifestResourceStream(assemblyQualifiedName);
            return resourceStream;
        }

        /// <summary> Returns the name of the bundle. </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a collection of all available resources within the bundle.
        /// </summary>
        /// <returns>Array of resource paths</returns>
        public string[] GetResources() {
            if (iResources != null) return iResources;

            string filePath = iMarcFile.FullName;
            using (ZipFile zipFile = new ZipFile(filePath)) {
                string[] entryNames = new string[zipFile.Count];
                int p = 0;
                zipFile.ForEach(entry => {
                    ZipEntry zipEntry = (ZipEntry) entry;
                    entryNames[p++] = zipEntry.Name;
                });

                iResources = entryNames;
            }

            return iResources;
        }

        /// <summary>
        /// Returns a collection of all available resources within the bundle 
        /// that fullfil the search pattern.
        /// </summary>
        /// <param name="searchPattern">Search pattern</param>
        /// <returns>Array of resource paths</returns>
        public string[] GetResources(string searchPattern) {
            Regex regex = new Regex(searchPattern);
            string[] resources = GetResources();
            string[] filtered = new string[resources.Length];
            int p = 0;
            for (int i = -1; ++i != resources.Length;) {
                string resource = resources[i];
                if (!regex.IsMatch(resource)) continue;
                filtered[p++] = resource;
            }
            
            Array.Resize(ref filtered, p);
            return filtered;
        }

        public override string ToString() {
            return iMarcFile.FullName;
        }

        /// <summary>
        /// Resolves the assembly that has been packed with this bundle.
        /// </summary>
        private void ResolveBundledAssembly() {
            string assemblyName = string.Format("{0}.dll", Name);
            Assembly assembly = Assembly.Load(assemblyName);
            if (assembly == null)
                return;
            iBundledAssembly = assembly;
        }
    }
}