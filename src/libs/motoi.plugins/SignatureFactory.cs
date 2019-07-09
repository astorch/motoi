using System.IO;
using PTP;

namespace motoi.plugins {
    /// <summary> Provides a factory for signature files. </summary>
    class SignatureFactory : AbstractFactory<SignatureFactory> {
        /// <inheritdoc />
        protected SignatureFactory() {
            // Only clients may initialize
        }

        /// <summary>
        /// Creates an instance of <see cref="IPluginSignature"/> from the given 
        /// signature file stream.
        /// </summary>
        /// <param name="signatureFileStream">Stream to read from</param>
        /// <returns>Instance of ISignature</returns>
        public IPluginSignature CreateSignature(Stream signatureFileStream) {
            ManifestDocumentParser parser = new ManifestDocumentParser(signatureFileStream);
            IPlainTextDocument document = parser.Parse();

            string nameValue = document.SelectValue("name");
            string symbolicValue = document.SelectValue("symbolicName");
            string versionValue = document.SelectValue("version");
            string activatorValue = document.SelectValue("activator");
            string vendorValue = document.SelectValue("vendor");
            string[] dependenciesValue = document.SelectValues("dependencies");

            PluginSignatureImpl pluginSignature = new PluginSignatureImpl {
                Name = nameValue, SymbolicName = symbolicValue, Vendor = vendorValue,
                Dependencies = dependenciesValue, Version = new Version(versionValue),
                PluginActivatorType = activatorValue
            };

            return pluginSignature;
        }
    }
}