using System;
using System.IO;
using System.Reflection;

namespace motoi.extensions {
    /// <summary>
    /// Static class to configue log4net.
    /// </summary>
    static class LogConfigurer {
        /// <summary>
        /// Configurates log4net.
        /// </summary>
        public static void Configurate() {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("motoi.extensions.log4net.config")) {
                if (stream == null) throw new InvalidOperationException("Could not read log4net.config stream");
                log4net.Config.XmlConfigurator.Configure(stream);
            }
        }
    }
}