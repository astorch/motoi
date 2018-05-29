using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace motoi.plugins {
    /// <summary>
    /// Provides an implementation of <see cref="IBundle"/>
    /// </summary>
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
        }

        /// <summary>
        /// Returns a stream to the given resource within the bundle.
        /// </summary>
        /// <param name="resource">Path of the resource within the bundle</param>
        /// <returns>Stream or null</returns>
        public Stream GetResourceAsStream(string resource) {
            string filePath = iMarcFile.FullName;
            byte[] buffer;

            using (ZipFile zipFile = new ZipFile(filePath)) {
                ZipEntry signatureZipEntry = zipFile[resource];

                if (signatureZipEntry == null)
                    return null;

                long uncompressedSize = signatureZipEntry.UncompressedSize;
                buffer = new byte[uncompressedSize];

                using (MemoryStream memoryOutputStream = new MemoryStream(buffer)) {
                    signatureZipEntry.Extract(memoryOutputStream);
                }
            }

            MemoryStream memoryInputStream = new MemoryStream(buffer);
            return memoryInputStream;
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

            if(iBundledAssembly == null)
                throw new NullReferenceException("Bundled assembly is null");

            Stream resourceStream = iBundledAssembly.GetManifestResourceStream(assemblyQualifiedName);
            return resourceStream;
        }

        /// <summary>
        /// Returns the name of the bundle.
        /// </summary>
        public string Name {
            get { return Path.GetFileNameWithoutExtension(iMarcFile.FullName); }
        }

        /// <summary>
        /// Returns a collection of all available resources within the bundle.
        /// </summary>
        /// <returns>Array of resource paths</returns>
        public string[] GetResources() {
            if (iResources != null)
                return iResources;

            string filePath = iMarcFile.FullName;
            using (ZipFile zipFile = new ZipFile(filePath)) {
                iResources = zipFile.EntryFileNames.ToArray();
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
            string[] filtered = resources.Where(rsx => regex.IsMatch(rsx)).ToArray();
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