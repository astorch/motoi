using System;
using System.IO;
using System.Reflection;
using Xcite.Csharp.assertions;

namespace motoi.platform.ui.images {
    /// <summary>
    /// Provides properties to describe an image.
    /// </summary>
    public class ImageDescriptor {
        /// <summary>
        /// Creates a new instance based on the given arguments.
        /// </summary>
        /// <param name="id">Id of the image</param>
        /// <param name="imageStream">Stream to the image data</param>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public ImageDescriptor(string id, Stream imageStream) {
            Id = Assert.NotNullOrEmpty(() => id);
            ImageStream = Assert.NotNull(() => imageStream);
        }

        /// <summary>
        /// Returns the id of the image.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Returns a stream to the image data.
        /// </summary>
        public Stream ImageStream { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="ImageDescriptor"/> using the given arguments. In contrast to <see cref="Create(string,Assembly,string)"/> 
        /// this method uses <see cref="Assembly.GetCallingAssembly"/> to determine the providing assembly.
        /// </summary>
        /// <param name="id">Id of the image</param>
        /// <param name="pathToAssemblyResource">Path within the providing assembly to the image data</param>
        /// <returns>Created instance of <see cref="ImageDescriptor"/></returns>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public static ImageDescriptor Create(string id, string pathToAssemblyResource) {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            return Create(id, callingAssembly, pathToAssemblyResource);
        }

        /// <summary>
        /// Creates an instance of <see cref="ImageDescriptor"/> using the given arguments.
        /// </summary>
        /// <param name="id">Id of the image</param>
        /// <param name="assembly">Assembly that provides the image data</param>
        /// <param name="pathToAssemblyResource">Path within the providing assembly to the image data</param>
        /// <returns>Created instance of <see cref="ImageDescriptor"/></returns>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public static ImageDescriptor Create(string id, Assembly assembly, string pathToAssemblyResource) {
            Assert.NotNullOrEmpty(() => id);
            Assert.NotNull(() => assembly);
            Assert.NotNullOrEmpty(() => pathToAssemblyResource);

            string assemblyName = assembly.GetName().Name;
            string adjustedResourcePath = pathToAssemblyResource.Replace('/', '.');
            string assemblyResourcePath = string.Format("{0}.{1}", assemblyName, adjustedResourcePath);

            Stream resourceStream = assembly.GetManifestResourceStream(assemblyResourcePath);
            return new ImageDescriptor(id, resourceStream);
        }
    }
}