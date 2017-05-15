using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using motoi.extensions;
using motoi.platform.application.model;
using motoi.platform.resources;
using motoi.platform.resources.runtime.preference;
using motoi.plugins;
using PTP.Core;
using PTP.Parsers;

namespace motoi.platform.application {
    /// <summary> Provides the common entry point of any motoi application. </summary>
    public class MotoiBootloader {
        /// <summary> Log instance. </summary>
        private static ILog iLogger;

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {
            LogConfigurer.Configurate();
            iLogger = LogManager.GetLogger(typeof (MotoiBootloader));

            try {
                // Load platform settings
                LoadIniFile();

                // Start the Resource Service
                ResourceService.Instance.Handshake();

                // Initialize Preference Store Manager
                PreferenceStoreManager.GetInstance().Initialize();

                // Start Plug-in Service
                PluginService.Instance.Start();

                // Start Extension Service
                ExtensionService.Instance.Start();

                // Start Application Manager
                ApplicationManager.Instance.DoWork();
            } catch (Exception ex) {
                iLogger.Error(ex.Message, ex);
                Environment.Exit(-1);
            } finally {
                ApplicationManager.Instance.HomeTime();
                
                ExtensionService.Instance.Stop();
                PluginService.Instance.Stop();

                PreferenceStoreManager.GetInstance().Dispose();

                ResourceService.Instance.Destroy();

                PlatformSettings.Instance.Dispose();
            }
        }

        /// <summary>
        /// Loads the ini file and stores all settings within the global PlatformSettings instance.
        /// </summary>
        private static void LoadIniFile() {
            string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            string iniName = string.Format("{0}.ini", procName).ToLowerInvariant();

            string currentDirectoryPath = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectoryPath);
            FileInfo iniFile = directoryInfo.GetFiles(iniName).FirstOrDefault();
            if (iniFile == null) throw new InvalidOperationException(string.Format("No ini file '{1}' could be found within the current working dir '{0}'", currentDirectoryPath, iniName));
            
            IniDocumentParser iniDocumentParser = new IniDocumentParser(iniFile);
            IPlainTextDocument document = iniDocumentParser.Parse();
            PlatformSettings platformSettings = PlatformSettings.Instance;
            using (IEnumerator<KeyValuePair<string, string[]>> itr = document.GetEnumerator()) {
                while (itr.MoveNext()) {
                    KeyValuePair<string, string[]> pair = itr.Current;
                    string key = pair.Key;
                    string[] values = pair.Value;
                    string value = (values.Length == 0 ? string.Empty : values[0]);
                    platformSettings.Add(key, value);

                    // Make settings for all components available
                    Environment.SetEnvironmentVariable($"motoi:{key}", value, EnvironmentVariableTarget.Process);
                }
            }
        }
    }
}