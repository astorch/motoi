using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace motoi.plugins {
    /// <summary>
    /// Static class to configue log4net.
    /// </summary>
    static class LogConfigurer {
        /// <summary>
        /// Configurates log4net.
        /// </summary>
        public static void Configurate() {
//            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("motoi.plugins.log4net.config")) {
//                if (stream == null) throw new InvalidOperationException("Could not read log4net.config stream");
//                log4net.Config.XmlConfigurator.Configure(stream);
//            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("motoi.plugins._nlog.config")) {
                if (stream == null) throw new InvalidOperationException("Could not read nlog.config stream");
                using (XmlReader reader = XmlReader.Create(stream)) {
                    NLog.Config.XmlLoggingConfiguration config = new NLog.Config.XmlLoggingConfiguration(reader, "nlog.config");
                    NLog.LogManager.Configuration = config;
                }
            }
        }
    }
}