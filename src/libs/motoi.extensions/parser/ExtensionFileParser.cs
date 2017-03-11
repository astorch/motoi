using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using motoi.extensions.core;
using motoi.extensions.exceptions;

namespace motoi.extensions.parser
{
    /// <summary>
    /// Provides a parser for extension.xml files.
    /// </summary>
    class ExtensionFileParser {

        /// <summary>
        /// Backing variable for the parser instance.
        /// </summary>
        private static ExtensionFileParser iParserInstance;

        /// <summary>
        /// Returns an instance for the given input stream.
        /// </summary>
        /// <param name="stream">Input stream for the parser</param>
        /// <returns></returns>
        public static ExtensionFileParser GetInstance(Stream stream) {
            if(iParserInstance == null)
                iParserInstance = new ExtensionFileParser();

            iParserInstance.iStream = stream;
            return iParserInstance;
        }

        /// <summary>
        /// Point definition.
        /// </summary>
        private const string PointDefinition = "point";

        /// <summary>
        /// Backing variable for the input stream.
        /// </summary>
        private Stream iStream;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private ExtensionFileParser() { }

        /// <summary>
        /// Parses the extension definition stream and creates a collection of Extension Map.
        /// </summary>
        /// <returns>Instance of <see cref="ExtensionService"/></returns>
        public ExtensionPointMap Parse() {
            ExtensionPointMap map = new ExtensionPointMap();
            XDocument xmlDocument;
            try {
                xmlDocument = XDocument.Load(iStream);
            } catch (Exception ex) {
                throw new ExtensionsParsingException("Error on parsing the 'extensions.xml'.", ex);
            }

            IEnumerable<XElement> extensions = from extension in xmlDocument.Descendants("extension")
                                                select extension;

            foreach (XElement extension in extensions) {
                XAttribute attrPoint = extension.Attribute(PointDefinition);
                if(attrPoint == null)
                    throw new ExtensionsParsingException("Attribute 'point' is missing or invalid!");

                string id = attrPoint.Value;

                try {
                    IList<IConfigurationElement> configurationElements = ParseConfigurationElements(extension);
                    map.AddAll(id, configurationElements);
                } catch {
                    throw;
                }
            }

            return map;
        }

        /// <summary>
        /// Parses the given extension node and resolves all Configuration Elements.
        /// </summary>
        /// <param name="extension">Extension node definition</param>
        /// <returns>Collection resolved configuration elements</returns>
        private IList<IConfigurationElement> ParseConfigurationElements(XElement extension) {
            IList<IConfigurationElement> configurationElements = new List<IConfigurationElement>(5);

            foreach (XElement element in extension.Descendants()) {
                string prefix = element.Name.LocalName;

                XAttribute attrId = element.Attribute("id");

                // There must be an id attribute
                if (attrId == null) {
                    string message = string.Format("There is no id attribute for the current element: {0}", element);
                    throw new ExtensionsParsingException(message);
                }

                string id = attrId.Value;

                IEnumerable<XAttribute> attributes = from attribute in element.Attributes() select attribute;
                IDictionary<string,string> attributeMap = new Dictionary<string, string>(3);
                foreach (XAttribute attribute in attributes) {
                    string key = attribute.Name.LocalName;
                    string value = attribute.Value;
                    attributeMap.Add(key, value);
                }

                IConfigurationElement configurationElement = new ConfigurationElementImpl(id, prefix, attributeMap);
                configurationElements.Add(configurationElement);
            }

            return configurationElements;
        }
    }
}