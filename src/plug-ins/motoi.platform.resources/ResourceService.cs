using System;
using System.IO;
using System.Runtime.Serialization;
using motoi.platform.resources.model;
using motoi.platform.resources.runtime.filesystem;
using NLog;
using xcite.csharp;
using xcite.io;

namespace motoi.platform.resources
{
    /// <summary>
    /// Provides access to the resources used by this platform.
    /// </summary>
    public class ResourceService : GenericSingleton<ResourceService> {

        private const string MetadataFolderName = ".metadata";
        private const string MetadataFileName = "resources.info";

        private static readonly Logger _log = LogManager.GetCurrentClassLogger(typeof(ResourceService));

        // TODO Use another serializer cause to bad performance
        private readonly DataContractSerializer iXmlSerializer = new DataContractSerializer(typeof(PlatformMetaInformation));
        private PlatformMetaInformation iPlatformMetaInformation;

        /// <summary> Returns the current workspace. </summary>
        public IWorkspace Workspace { get; private set; }

        /// <summary> Returns the meta data folder. </summary>
        internal string MetaDataFolder { get; private set; }

        /// <summary> Returns the log of the <see cref="ResourceService"/>. </summary>
        public Logger ResourceServiceLog 
            => _log;

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            _log.Info("Initializing...");

            string metaDataFilePath = Path.Combine(MetadataFolderName, MetadataFileName);
            FileInfo metaDataFile = new FileInfo(metaDataFilePath);
            MetaDataFolder = metaDataFile.DirectoryName;
            _log.Info($"Meta data file: '{metaDataFile.FullName}'");
            
            if (!metaDataFile.Exists) {
                iPlatformMetaInformation = CreatePlatformMetaInformation();
            } else {
                _log.Info("Deserializing platform meta information");
                using (FileStream fileStream = metaDataFile.OpenRead()) // TODO Try catch
                    iPlatformMetaInformation = (PlatformMetaInformation) iXmlSerializer.ReadObject(fileStream);

                if (iPlatformMetaInformation == null) {
                    _log.Error("Platform meta information null");
                    CreatePlatformMetaInformation();
                }

                _log.Info("Deserialization finished");
            }

            _log.Info($"Workspace root path: {iPlatformMetaInformation.WorkspaceDirectoryPath}");
            Workspace = new FileSystemWorkspace(new DirectoryInfo(iPlatformMetaInformation.WorkspaceDirectoryPath)); // TODO Introduce workspace provider extension point
            // Workspace.Refresh(); // XXX 07.11.2016 Is done by ctor
            _log.Info("Workspace successfully set");

            _log.Info("Finished");
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            _log.Info("Destroying...");

            string metaDataFilePath = Path.Combine(MetadataFolderName, MetadataFileName);
            FileInfo metaDataFile = new FileInfo(metaDataFilePath);
            _log.Info($"Meta data file: '{metaDataFile}'");

            if (!metaDataFile.Exists) {
                _log.Info("Platform meta information does not exist. Creating a new one.");
                metaDataFile.CreateFile();
            }

            _log.Info("Serializing platform meta information");
            using (FileStream fileStream = metaDataFile.OpenWrite())
                iXmlSerializer.WriteObject(fileStream, iPlatformMetaInformation);

            _log.Info("Serialization finished");
   
            _log.Info("Finished");
        }

        /// <summary>
        /// Brings up a dialog so the user can set up the default platform meta information.
        /// </summary>
        /// <returns>Newly created platform meta information</returns>
        private PlatformMetaInformation CreatePlatformMetaInformation() {
            _log.Info("Platform meta information does not exist. Creating a new one.");
            
            PlatformMetaInformation platformMetaInformation = new PlatformMetaInformation();

//            IDialogWindow dialog = UIFactory.NewViewPart<IDialogWindow>();
//            dialog.WindowHeight = 60;
//            dialog.WindowWidth = 200;
//            IGridComposite gridComposite = UIFactory.NewWidget<IGridComposite>(dialog);
//            dialog.Show();

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string defaultWorkspaceRootPath = string.Format("{0}\\Motoi\\Workspace", appDataPath);

            DirectoryInfo workspaceDirectoryInfo = new DirectoryInfo(defaultWorkspaceRootPath);
            if (!workspaceDirectoryInfo.Exists)
                workspaceDirectoryInfo.Create();

            // TODO Show Message Dialog
            platformMetaInformation.WorkspaceDirectoryPath = workspaceDirectoryInfo.FullName;

            return platformMetaInformation;
        }
    }

    /// <summary> Defines platform meta information. </summary>
    [DataContract]
    class PlatformMetaInformation {
        /// <summary>
        /// Returns the path to the workspace root directory or does set it.
        /// </summary>
        [DataMember]
        public string WorkspaceDirectoryPath { get; set; }
    }
}