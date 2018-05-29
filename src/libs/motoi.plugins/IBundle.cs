using System.IO;

namespace motoi.plugins {
    /// <summary> Defines a bundle. </summary>
    public interface IBundle {
        /// <summary>
        /// Returns a stream for the given resource within the bundle.
        /// </summary>
        /// <param name="resource">Path of the resource within the bundle</param>
        /// <returns>Stream or null</returns>
        Stream GetResourceAsStream(string resource);

        /// <summary>
        /// Returns a stream for the given resource that has been packed within the assembly 
        /// as embedded resource.
        /// </summary>
        /// <param name="resource">Path to embedded resource within the assembly</param>
        /// <returns>Stream or null</returns>
        Stream GetAssemblyResourceAsStream(string resource);

        /// <summary>
        /// Returns a collection of all available resources within the bundle.
        /// </summary>
        /// <returns>Array of resource paths</returns>
        string[] GetResources();

        /// <summary>
        /// Returns a collection of all available resources within the bundle 
        /// that fullfil the search pattern.
        /// </summary>
        /// <param name="searchPattern">Search pattern</param>
        /// <returns>Array of resource paths</returns>
        string[] GetResources(string searchPattern);

        /// <summary>
        /// Returns the name of the bundle.
        /// </summary>
        string Name { get; }
    }
}