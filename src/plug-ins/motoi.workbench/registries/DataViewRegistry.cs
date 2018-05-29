using System;
using System.Collections.Generic;
using System.Linq;
using motoi.extensions;
using motoi.platform.nls;
using motoi.platform.ui;
using motoi.plugins;
using motoi.workbench.model;
using NLog;
using xcite.csharp;

namespace motoi.workbench.registries {
    /// <summary>
    /// Provides methods to manage instances of <see cref="IDataView"/>.
    /// </summary>
    public class DataViewRegistry : GenericSingleton<DataViewRegistry> {
        /// <summary> Extension Point id. </summary>
        private const string DataViewExtensionPointId = "org.motoi.ui.dataview";

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly List<DataViewContribution> iRegisteredDataViews = new List<DataViewContribution>(31);
        private readonly Dictionary<Type, IDataView> iCreatedDataViews = new Dictionary<Type, IDataView>(31);

        /// <summary>
        /// Returns an enumerable of all registered view references.
        /// </summary>
        /// <returns>Enumerable of all registered view references</returns>
        /// <seealso cref="IViewReference"/>
        public IEnumerable<IViewReference> GetViewReferences() {
            for (int i = -1; ++i != iRegisteredDataViews.Count;) {
                DataViewContribution dataViewContribution = iRegisteredDataViews[i];
                yield return new ViewReferenceImpl(dataViewContribution.DataViewId, dataViewContribution.DataViewLabel);
            }
        }

        /// <summary>
        /// Returns the instance of <see cref="IDataView"/> with the given <paramref name="dataViewId"/> from the registry. 
        /// If the id is not known by the registry, NULL is returned. If an instance of the data view has already been created, the existing one is 
        /// returned. There won't be more than one instance of a data view.
        /// </summary>
        /// <param name="dataViewId">Id of the data view to get</param>
        /// <returns>Instance of <see cref="IDataView"/> or NULL</returns>
        public IDataView GetDataView(string dataViewId) {
            DataViewContribution dataViewContribution = iRegisteredDataViews.FirstOrDefault(entry => entry.DataViewId == dataViewId);
            if (dataViewContribution == null) return null;

            Type dataViewType = dataViewContribution.DataViewType;
            return GetOrCreateDataView<IDataView>(dataViewType);
        }

        /// <summary>
        /// Returns the instance of <see cref="IDataView"/> that matches the given <typeparamref name="TDataView"/> type.
        /// If the type is not known by the registry, NULL is returned. If an instance of the data view has already been created, the existing one is 
        /// returned. There won't be more than one instance of a data view.
        /// </summary>
        /// <typeparam name="TDataView">Type of data view to get</typeparam>
        /// <returns>Instance of <see cref="IDataView"/> or NULL</returns>
        public TDataView GetDataView<TDataView>() where TDataView : class, IDataView {
            Type dataViewType = typeof (TDataView);
            DataViewContribution dataViewContribution = iRegisteredDataViews.FirstOrDefault(entry => entry.DataViewType == dataViewType);
            if (dataViewContribution == null) return null;

            return GetOrCreateDataView<TDataView>(dataViewType);
        }

        /// <summary>
        /// Returns the existing instance of the given <paramref name="dataViewType"/>. If there isn't one, a new one is created.
        /// </summary>
        /// <typeparam name="TDataView">Type of data view to get</typeparam>
        /// <param name="dataViewType">Type of data view to get</param>
        /// <returns>Instance of <see cref="IDataView"/> or NULL</returns>
        private TDataView GetOrCreateDataView<TDataView>(Type dataViewType) where TDataView : class, IDataView {
            try {
                // Consult internal cache
                if (iCreatedDataViews.TryGetValue(dataViewType, out IDataView rawDataViewInstance)) return rawDataViewInstance as TDataView;

                // Create new instance
                TDataView dataViewInstance = dataViewType.NewInstance<TDataView>();
                iCreatedDataViews.Add(dataViewType, dataViewInstance);

                // Exit
                return dataViewInstance;
            } catch (Exception ex) {
                _log.Error(ex, $"Error on initiating a new instance of '{dataViewType}'.");
                return null;
            }
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(DataViewExtensionPointId);
            if (configurationElements.Length == 0) {
                _log.Warn("No data view has been contributed by any extension point!");
                return;
            }

            _log.Debug($"{configurationElements.Length} data view contributions has been found");
            for (int i = -1; ++i != configurationElements.Length; ) {
                IConfigurationElement configurationElement = configurationElements[i];

                string id = configurationElement["id"];
                string cls = configurationElement["class"];
                string label = configurationElement["label"];

                if (string.IsNullOrEmpty(label))
                    label = id;

                _log.Debug($"Registering contribution {{id: '{id}', cls: '{cls}', label: '{label}'}}");

                if (string.IsNullOrWhiteSpace(id)) {
                    _log.Error("Id attribute of data view extension contribution is null or empty!");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(cls)) {
                    _log.Error($"Class attribute of data view extension contribution is null or empty!. Contribution id: '{id}'");
                    continue;
                }

                IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(configurationElement);
                try {
                    Type dataViewType = TypeLoader.TypeForName(providingBundle, cls);

                    // NLS support
                    label = NLS.Localize(label, dataViewType);

                    DataViewContribution dataViewContribution = new DataViewContribution {
                        DataViewId = id,
                        DataViewType = dataViewType,
                        DataViewLabel = label
                    };

                    iRegisteredDataViews.Add(dataViewContribution);
                    _log.Info($"Data view contribution '{id}' registered.");
                } catch (Exception ex) {
                    _log.Error(ex, $"Error loading type '{cls}'.");
                }
            }
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            iRegisteredDataViews.Clear();
            iCreatedDataViews.Clear();
        }

        /// <summary>
        /// Provides an anonymous implementation of <see cref="IViewReference"/>.
        /// </summary>
        class ViewReferenceImpl : IViewReference {
            /// <inheritdoc />
            public ViewReferenceImpl(string id, string title) {
                if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
                if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));
                Id = id;
                Title = title;
            }

            /// <inheritdoc />
            public IViewPart GetInstance(bool restore) {
                throw new NotSupportedException();
            }

            /// <inheritdoc />
            public string Id { get; }

            /// <inheritdoc />
            public string Title { get; }
        }

        /// <summary> Describes a contribution of a data view. </summary>
        class DataViewContribution {
            /// <summary> Returns the id of the data view or does set it. </summary>
            public string DataViewId { get; set; }

            /// <summary> Returns the <see cref="Type"/> of the data view or does set it. </summary>
            public Type DataViewType { get; set; }

            /// <summary> Returns the label of the data view. </summary>
            public string DataViewLabel { get; set; }
        }
    }
}