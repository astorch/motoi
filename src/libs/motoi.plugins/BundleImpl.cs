using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;

namespace motoi.plugins {
    /// <summary> Provides an implementation of <see cref="IBundle"/> </summary>
    class BundleImpl : IBundle {
        private readonly FileInfo _marcFile;
        private Assembly _bundledAssembly;
        private string[] _resources;

        /// <summary> Creates a new instance for the given marc file. </summary>
        /// <param name="marcFile">Marc file</param>
        public BundleImpl(FileInfo marcFile) {
            if (marcFile == null) throw new ArgumentNullException(nameof(marcFile));

            if (!marcFile.Exists)
                throw new ArgumentException($"Marc file does not exist! Path is '{marcFile.FullName}'");

            _marcFile = marcFile;
            Name = Path.GetFileNameWithoutExtension(_marcFile.FullName);
        }

        /// <summary> Returns a stream to the given resource within the bundle. </summary>
        /// <param name="resource">Path of the resource within the bundle</param>
        /// <returns>Stream or null</returns>
        public Stream GetResourceAsStream(string resource) {
            string filePath = _marcFile.FullName;

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
            if (_bundledAssembly == null)
                ResolveBundledAssembly();

            string translatedResourceName = resource.Replace('/', '.');
            string assemblyQualifiedName = $"{Name}.{translatedResourceName}";

            if(_bundledAssembly == null) throw new InvalidOperationException("Bundled assembly is null");

            return _bundledAssembly.GetManifestResourceStream(assemblyQualifiedName);
        }

        /// <summary> Returns the name of the bundle. </summary>
        public string Name { get; }

        /// <summary> Returns a collection of all available resources within the bundle. </summary>
        /// <returns>Array of resource paths</returns>
        public string[] GetResources() {
            if (_resources != null) return _resources;

            string filePath = _marcFile.FullName;
            using (ZipFile zipFile = new ZipFile(filePath)) {
                string[] entryNames = new string[zipFile.Count];
                int p = 0;

                IEnumerator itr = zipFile.GetEnumerator();
                try {
                    while (itr.MoveNext()) {
                        ZipEntry zipEntry = (ZipEntry) itr.Current;
                        if (zipEntry == null) continue;
                        entryNames[p++] = zipEntry.Name;
                    }
                } finally {
                    (itr as IDisposable)?.Dispose();
                }

                _resources = entryNames;
            }

            return _resources;
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

        /// <inheritdoc />
        public override string ToString() {
            return _marcFile.FullName;
        }

        /// <summary> Resolves the assembly that has been packed with this bundle. </summary>
        private void ResolveBundledAssembly() {
            string assemblyName = $"{Name}.dll";
            Assembly assembly = Assembly.Load(assemblyName);
            if (assembly == null) return;
            _bundledAssembly = assembly;
        }
    }
}