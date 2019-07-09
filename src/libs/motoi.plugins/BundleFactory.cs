using System.IO;

namespace motoi.plugins {
    /// <summary> Implements a factory for Bundles. </summary>
    class BundleFactory : AbstractFactory<BundleFactory> {
        /// <summary> Creates a new Bundle for the given marc file. </summary>
        /// <param name="marcFile">Marc file</param>
        /// <returns>New Bundle instance</returns>
        public IBundle CreateBundle(FileInfo marcFile) {
            return new BundleImpl(marcFile);
        }
    }
}