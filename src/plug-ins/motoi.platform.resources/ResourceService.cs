using System;
using System.IO;
using System.Runtime.Serialization;
using log4net;
using motoi.platform.resources.model;
using motoi.platform.resources.runtime.filesystem;
using Xcite.Csharp.generics;
using Xcite.Csharp.io;

namespace motoi.platform.resources
{
    /// <summary>
    /// Provides access to the resources used by this platform.
    /// </summary>
    public class ResourceService : GenericSingleton<ResourceService> {

        private const string iMetadataFolderName = ".metadata";
        private const string iMetadataFileName = "resources.info";

        private static readonly ILog iLog = LogManager.GetLogger(typeof (ResourceService));

        // TODO Use another serializer cause to bad performance
        private readonly DataContractSerializer iXmlSerializer = new DataContractSerializer(typeof(PlatformMetaInformation));
        private PlatformMetaInformation iPlatformMetaInformation;

        /// <summary>
        /// Returns the current workspace.
        /// </summary>
        public IWorkspace Workspace { get; private set; }

        /// <summary>
        /// Returns the meta data folder.
        /// </summary>
        internal string MetaDataFolder { get; private set; }

        /// <summary>
        /// Returns the log of the <see cref="ResourceService"/>.
        /// </summary>
        public ILog ResourceServiceLog { get { return iLog; } }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            iLog.Info("Initializing...");

            string metaDataFilePath = Path.Combine(iMetadataFolderName, iMetadataFileName);
            FileInfo metaDataFile = new FileInfo(metaDataFilePath);
            MetaDataFolder = metaDataFile.DirectoryName;
            iLog.InfoFormat("Meta data file: '{0}'", metaDataFile.FullName);
            
            if (!metaDataFile.Exists) {
                iPlatformMetaInformation = CreatePlatformMetaInformation();
            } else {
                iLog.Info("Deserializing platform meta information");
                using (FileStream fileStream = metaDataFile.OpenRead()) // TODO Try catch
                    iPlatformMetaInformation = (PlatformMetaInformation) iXmlSerializer.ReadObject(fileStream);

                if (iPlatformMetaInformation == null) {
                    iLog.Error("Platform meta information null");
                    CreatePlatformMetaInformation();
                }

                iLog.Info("Deserialization finished");
            }

            iLog.InfoFormat("Workspace root path: {0}", iPlatformMetaInformation.WorkspaceDirectoryPath);
            Workspace = new FileSystemWorkspace(new DirectoryInfo(iPlatformMetaInformation.WorkspaceDirectoryPath)); // TODO Introduce workspace provider extension point
            // Workspace.Refresh(); // XXX 07.11.2016 Is done by ctor
            iLog.Info("Workspace successfully set");

            iLog.Info("Finished");
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            iLog.Info("Destroying...");

            string metaDataFilePath = Path.Combine(iMetadataFolderName, iMetadataFileName);
            FileInfo metaDataFile = new FileInfo(metaDataFilePath);
            iLog.InfoFormat("Meta data file: '{0}'", metaDataFile);

            if (!metaDataFile.Exists) {
                iLog.Info("Platform meta information does not exist. Creating a new one.");
                metaDataFile.CreateFile();
            }

            iLog.Info("Serializing platform meta information");
            using (FileStream fileStream = metaDataFile.OpenWrite())
                iXmlSerializer.WriteObject(fileStream, iPlatformMetaInformation);

            iLog.Info("Serialization finished");
   
            iLog.Info("Finished");
        }

        /// <summary>
        /// Brings up a dialog so the user can set up the default platform meta information.
        /// </summary>
        /// <returns>Newly created platform meta information</returns>
        private PlatformMetaInformation CreatePlatformMetaInformation() {
            iLog.Info("Platform meta information does not exist. Creating a new one.");
            
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

    /// <summary>
    /// Defines platform meta information.
    /// </summary>
    [DataContract]
    class PlatformMetaInformation {

        /// <summary>
        /// Returns the path to the workspace root directory or does set it.
        /// </summary>
        [DataMember]
        public string WorkspaceDirectoryPath { get; set; }
    }
}