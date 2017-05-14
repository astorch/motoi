using System;
using System.IO;
using System.Reflection;

namespace motoi.platform.commons {
    /// <summary> Provides methods to open streams or load data from a resource of an assembly. </summary>
    public static class ResourceLoader {
        /// <summary>
        /// Opens a stream to a resource with the given <paramref name="resourceUrl"/>. The stream is fully copied 
        /// into a buffer that is returned. The stream will be closed afterwards. The resource must be an 
        /// embedded resource of the calling assembly. The resource URL must match 
        /// the given format <code>{folder}/{subfolder}/{fileName}.{suffix}</code> starting from the root path of 
        /// the assembly, for instance, 'resources/images/icon.png', 'config/check.xml' or 'log4net.config'. 
        /// Note that you have to close the stream yourself!
        /// </summary>
        /// <param name="resourceUrl">URL of the resource starting from the root path of the assembly</param>
        /// <returns>Data copied from the stream</returns>
        public static byte[] ReadStream(string resourceUrl) {
            return ReadStream(Assembly.GetCallingAssembly(), resourceUrl);
        }

        /// <summary>
        /// Opens a stream to a resource with given <paramref name="resourceUrl"/>. The stream is fully copied 
        /// into a buffer that is returned. The stream will be closed afterwards. 
        /// The resource must be an embedded resource of the given <paramref name="assembly"/>. The resource URL must match 
        /// the given format <code>{folder}/{subfolder}/{fileName}.{suffix}</code> starting from the root path of 
        /// the assembly, for instance, 'resources/images/icon.png', 'config/check.xml' or 'log4net.config'. 
        /// Note that you have to close the stream yourself!
        /// </summary>
        /// <param name="assembly">Assembly that contains the resource</param>
        /// <param name="resourceUrl">URL of the resource starting from the root path of the assembly</param>
        /// <returns>Data copied from the stream</returns>
        public static byte[] ReadStream(Assembly assembly, string resourceUrl) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (BufferedStream bufferedStream = new BufferedStream(memoryStream)) {
                    using (Stream resourceStream = OpenStream(assembly, resourceUrl)) {
                        resourceStream.CopyTo(bufferedStream);
                    }
                }
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Opens a stream to a resource with the given <paramref name="resourceUrl"/>. The resource must be an 
        /// embedded resource of the calling assembly. The resource URL must match 
        /// the given format <code>{folder}/{subfolder}/{fileName}.{suffix}</code> starting from the root path of 
        /// the assembly, for instance, 'resources/images/icon.png', 'config/check.xml' or 'log4net.config'. 
        /// Note that you have to close the stream yourself!
        /// </summary>
        /// <param name="resourceUrl">URL of the resource starting from the root path of the assembly</param>
        /// <returns>Stream</returns>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public static Stream OpenStream(string resourceUrl) {
            return OpenStream(Assembly.GetCallingAssembly(), resourceUrl);
        }

        /// <summary>
        /// Opens a stream to a resource with the given <paramref name="resourceUrl"/>. The resource 
        /// must be an embedded resource of the given <paramref name="assembly"/>. The resource URL must match 
        /// the given format <code>{folder}/{subfolder}/{fileName}.{suffix}</code> starting from the root path of 
        /// the assembly, for instance, 'resources/images/icon.png', 'config/check.xml' or 'log4net.config'. 
        /// Note that you have to close the stream yourself!
        /// </summary>
        /// <param name="assembly">Assembly that contains the resource</param>
        /// <param name="resourceUrl">URL of the resource starting from the root path of the assembly</param>
        /// <returns>Stream</returns>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public static Stream OpenStream(Assembly assembly, string resourceUrl) {
            if (string.IsNullOrEmpty(resourceUrl)) throw new ArgumentNullException(nameof(resourceUrl));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            string dotNetUrl = resourceUrl.Replace('/', '.');
            string fullQualifiedDotNetUrl = string.Format("{0}.{1}", assembly.GetName().Name, dotNetUrl);
            Stream memoryStream = assembly.GetManifestResourceStream(fullQualifiedDotNetUrl);
            return memoryStream;
        }
    }
}