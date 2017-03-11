using System.IO;
using System.Reflection;

namespace motoi.plugins
{
    /// <summary>
    /// Static class to configue log4net.
    /// </summary>
    static class LogConfigurer
    {
        /// <summary>
        /// Configurates log4net.
        /// </summary>
        public static void Configurate()
        {
            Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("motoi.core.plugins.adjuncts.log4net.config");

            if (stream == null)
                return;

            log4net.Config.XmlConfigurator.Configure(stream);
            stream.Dispose();
        }
    }
}