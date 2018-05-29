using System;
using System.Linq;
using motoi.extensions;
using motoi.plugins;
using motoi.workbench.model;
using NLog;
using xcite.collections;
using xcite.csharp;

namespace motoi.workbench.registries {
    /// <summary>
    /// Provides methods to manage instances of <see cref="IEditor"/>.
    /// </summary>
    public class EditorRegistry : GenericSingleton<EditorRegistry> {
        /// <summary> Extension Point id. </summary>
        private const string EditorExtensionPointId = "org.motoi.ui.editor";

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly LinearList<EditorContribution> iRegisteredEditors = new LinearList<EditorContribution>();

        /// <summary>
        /// Returns an editor that has been registered to the given <paramref name="fileExtension"/>. If there is 
        /// no editor for that, NULL is returned.
        /// </summary>
        /// <param name="fileExtension">File extension an editor can handle</param>
        /// <returns>Instance of <see cref="IEditor"/> or NULL</returns>
        public IEditor GetEditorForExtension(string fileExtension) {
            EditorContribution editorContribution = iRegisteredEditors.FirstOrDefault(entry => entry.SupportsFileExtension(fileExtension));
            if (editorContribution == null) return null;

            return GetEditorInstance(editorContribution);
        }

        /// <summary>
        /// Returns an editor that has been registered with the <paramref name="editorId"/>. If there is 
        /// no editor for that, NULL is returned.
        /// </summary>
        /// <param name="editorId">Id of the editor</param>
        /// <returns>Instance of <see cref="IEditor"/> or NULL</returns>
        public IEditor GetEditorForId(string editorId) {
            EditorContribution editorContribution = iRegisteredEditors.FirstOrDefault(entry => string.Equals(entry.EditorId, editorId));
            if (editorContribution == null) return null;

            return GetEditorInstance(editorContribution);
        }

        /// <summary>
        /// Newly creates an instance of <see cref="IEditor"/> based on the given <paramref name="editorContribution"/>.
        /// </summary>
        /// <param name="editorContribution">Editor contribution</param>
        /// <returns>Newly created instance or NULL</returns>
        private IEditor GetEditorInstance(EditorContribution editorContribution) {
            if (editorContribution == null) return null;

            Type editorType = editorContribution.EditorType;
            try {
                // IEditor editor = editorType.NewInstance<IEditor>();
                IEditor editor = (IEditor) Activator.CreateInstance(editorType);
                return editor;
            } catch (Exception ex) {
                _log.Error(ex, $"Error on initiating a new instance of '{editorType}'.");
                return null;
            }
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(EditorExtensionPointId);
            if (configurationElements.Length == 0) {
                _log.Warn("No editor has been contributed by any extension point!");
                return;
            }

            _log.Debug($"{configurationElements.Length} editor contributions has been found");
            for (int i = -1; ++i != configurationElements.Length;) {
                IConfigurationElement configurationElement = configurationElements[i];

                string id = configurationElement["id"];
                string cls = configurationElement["class"];
                string fileExtension = configurationElement["fileExtension"];

                _log.Debug($"Registering contribution {{id: '{id}', cls: '{cls}', fileExtension: '{fileExtension}'}}");

                if (string.IsNullOrWhiteSpace(id)) {
                    _log.Error("Id attribute of editor extension contribution is null or empty!");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(cls)) {
                    _log.Error($"Class attribute of editor extension contribution is null or empty!. Contribution id: '{id}'");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(fileExtension)) {
                    _log.Error($"File extension attribute of editor extension contribution is null or empty! Contribution id: '{id}'");
                    continue;
                }

                string[] fileExtensionParts = fileExtension.Split(new []{ ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (fileExtensionParts.Length == 0) {
                    _log.Error($"File extension declaration has unexpected format. Use comma separated values like 'txt,log'! Contribution id: '{id}'");
                    continue;
                }

                fileExtensionParts = Enumerable.ToArray(fileExtensionParts.Select(item => item.Trim()));

                IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(configurationElement);
                try {
                    Type editorType = TypeLoader.TypeForName(providingBundle, cls);
                    EditorContribution editorContribution = new EditorContribution {
                        EditorId = id,
                        EditorType = editorType,
                        SupportedFileExtensions = fileExtensionParts
                    };
                    iRegisteredEditors.Add(editorContribution);
                    _log.Info($"Editor contribution '{id}' registered.");
                } catch (Exception ex) {
                    _log.Error(ex, $"Error loading type '{cls}'.");
                }
            }
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            iRegisteredEditors.Clear();
        }

        /// <summary>
        /// Describes an editor contribution.
        /// </summary>
        class EditorContribution {
            /// <summary>
            /// Returns the id of the editor or does set it.
            /// </summary>
            public string EditorId { get; set; }

            /// <summary>
            /// Returns the <see cref="Type"/> of the editor or does set it.
            /// </summary>
            public Type EditorType { get; set; }

            /// <summary>
            /// Returns the supported file extensions by the editor or does set them.
            /// </summary>
            public string[] SupportedFileExtensions { get; set; }

            /// <summary>
            /// Returns TRUE if the editor supports the given <paramref name="fileExtension"/>.
            /// </summary>
            /// <param name="fileExtension">File extension to proof</param>
            /// <returns>TRUE or FALSE</returns>
            public bool SupportsFileExtension(string fileExtension) {
                if (string.IsNullOrWhiteSpace(fileExtension)) return false;
                bool any = SupportedFileExtensions.Any(x => x == fileExtension);
                return any;
            }
        }
    }
}