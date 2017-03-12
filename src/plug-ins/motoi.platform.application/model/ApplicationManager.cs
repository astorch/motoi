﻿using System;
using System.Threading;
using log4net;
using motoi.extensions;
using motoi.extensions.core;
using motoi.platform.resources.model.preference;
using motoi.platform.resources.runtime.preference;
using motoi.platform.ui;
using motoi.platform.ui.menus;
using motoi.platform.ui.shells;
using motoi.platform.ui.toolbars;
using motoi.plugins.model;

namespace motoi.platform.application.model
{
    /// <summary>
    /// Provides a manager for application instances.
    /// </summary>
    class ApplicationManager {

        /// <summary>
        /// Backing variable for the instance.
        /// </summary>
        private static ApplicationManager iInstance;

        /// <summary>
        /// Returns the current instance of the manager.
        /// </summary>
        public static ApplicationManager Instance {
            get { return iInstance ?? (iInstance = new ApplicationManager()); }
        }

        /// <summary> Extension point id. </summary>
        private const string ApplicationExtensionPointId = "org.motoi.application";
        private readonly ILog iLogger = LogManager.GetLogger(typeof (ApplicationManager));

        private IMotoiApplication iApplication;
        private IViewPartFactory iViewPartFactory;
        private IMainWindow iMainWindow;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private ApplicationManager() {
            // Nothing to do here
        }

        /// <summary>
        /// Tells the manager to do his job.
        /// </summary>
        public void DoWork() {
            iLogger.Debug("Doing my work");

            string applicationId = PlatformSettings.Instance["application"];

            if (string.IsNullOrEmpty(applicationId))
                throw new NullReferenceException("Application definition is null or empty! Check the ini file!");

            iApplication = GetApplicationInstance(applicationId);

            if (iApplication == null)
                throw new NullReferenceException(string.Format("There is no application implementation for the given application id '{0}'. " +
                                                               "Check the ini file and the existence of the application annotation!", applicationId));

            iViewPartFactory = FactoryProvider.GetViewPartFactory();
            iApplication.OnStartup();

            // Creating and Starting UI-Thread
            Thread uiThread = new Thread(OnRunningUI);
            uiThread.TrySetApartmentState(ApartmentState.STA);
            uiThread.Name = "Motoi Application UI-Thread";
            uiThread.Start();

            iLogger.Debug("Time to get some coffee");

            // Wait here until the UI thread has been finished
            uiThread.Join();
        }

        /// <summary>
        /// Thread method which will start the UI message dispatching
        /// </summary>
        private void OnRunningUI() {

            if (!iApplication.IsHeadless) {
                iApplication.OnPreInitializeMainWindow();
                iMainWindow = iViewPartFactory.CreateInstance<IMainWindow>();
                Platform.Instance.MainWindow = iMainWindow;
                MenuItemProvider.AddExtensionPointMenuItems(iMainWindow);
                ToolbarItemProvider.AddExtensionPointToolbarItems(iMainWindow);
                RestoreWindowState(iMainWindow);
                iApplication.OnPostInitializeMainWindow(iMainWindow);
            } else {
                iLogger.Info("Application will run headless");
            }

            iApplication.OnApplicationRun();

            try {
                iViewPartFactory.RunApplication(iMainWindow);
            } catch (Exception ex) {
                iLogger.Error(ex.Message, ex);
            }

            if (!iApplication.IsHeadless) {
                MemorizeWindowState(iMainWindow);
            }

            iApplication.OnApplicationShutdown();
        }

        /// <summary>
        /// Tells the manager to stop and to release all its resources (dispose).
        /// </summary>
        public void HomeTime() {
            iLogger.Debug("Time to go home");

            iApplication.OnShutdown();
            iApplication = null;

            iViewPartFactory = null;
            iMainWindow = null;

            iInstance = null;

            iLogger.Debug("Goodbye");
        }

        /// <summary>
        /// Returns an instance of <see cref="IMotoiApplication"/> defined by the given extension point definition id.
        /// If no implementation could be found, null will be returned.
        /// </summary>
        /// <param name="id">Id of the implementation definition</param>
        /// <returns>Instance of <see cref="IMotoiApplication"/> or null</returns>
        private IMotoiApplication GetApplicationInstance(string id) {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ApplicationExtensionPointId);
            IConfigurationElement applicationConfigurationElement = Array.Find(configurationElements, el => el["id"] == id);
            if (applicationConfigurationElement == null)
                return null;

            string className = applicationConfigurationElement["class"];

            if (string.IsNullOrEmpty(className))
                throw new NullReferenceException(string.Format("Attribute 'class' is null or empty for the application id '{0}'!", id));

            IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(applicationConfigurationElement);
            Type type = TypeLoader.TypeForName(providingBundle, className);

            IMotoiApplication application = Activator.CreateInstance(type) as IMotoiApplication;
            return application;
        }

        /// <summary>
        /// Restores the state of the given <paramref name="mainWindow"/> based on the values that 
        /// have been saved into a specific preference store before. If there aren't stored settings, 
        /// a default value set is used. If the given <paramref name="mainWindow"/> is NULL, nothing 
        /// will happen.
        /// </summary>
        /// <param name="mainWindow">Window to restore</param>
        private void RestoreWindowState(IMainWindow mainWindow) {
            if (mainWindow == null) return;

            try {
                IPreferenceStore preferenceStore = GetPreferenceStore();
                int windowState = preferenceStore.GetValue(WindowStateKey, (int)EWindowState.Normal);
                int windowWidth = preferenceStore.GetValue(WindowWidthKey, 800);
                int windowHeight = preferenceStore.GetValue(WindowHeightKey, 600);
                int windowTop = preferenceStore.GetValue(WindowTopKey, 0);
                int windowLeft = preferenceStore.GetValue(WindowLeftKey, 0);

                mainWindow.WindowState = (EWindowState)windowState;
                mainWindow.WindowWidth = windowWidth;
                mainWindow.WindowHeight = windowHeight;
                mainWindow.WindowTopLocation = windowTop;
                mainWindow.WindowLeftLocation = windowLeft;
            } catch (Exception ex) {
                iLogger.Error("Error on restoring window state.", ex);
            }
        }

        /// <summary>
        /// Memorizes the state of the given <paramref name="mainWindow"/> and writes all data 
        /// into a specific preference store. If the given <paramref name="mainWindow"/> is NULL, nothing 
        /// will happen.
        /// </summary>
        /// <param name="mainWindow">Window which state is to memorize</param>
        private void MemorizeWindowState(IMainWindow mainWindow) {
            if (mainWindow == null) return;
            int windowState = (int)mainWindow.WindowState;
            int windowWidth = mainWindow.WindowWidth;
            int windowHeight = mainWindow.WindowHeight;
            int windowTop = mainWindow.WindowTopLocation;
            int windowLeft = mainWindow.WindowLeftLocation;

            try {
                IPreferenceStore preferenceStore = GetPreferenceStore();
                preferenceStore.SetValue(WindowStateKey, windowState);
                preferenceStore.SetValue(WindowWidthKey, windowWidth);
                preferenceStore.SetValue(WindowHeightKey, windowHeight);
                preferenceStore.SetValue(WindowTopKey, windowTop);
                preferenceStore.SetValue(WindowLeftKey, windowLeft);
                preferenceStore.Flush();
            } catch (Exception ex) {
                iLogger.Error("Error on memorizing window state.", ex);
            }
        }

        /// <summary>
        /// Returns the associated preference store.
        /// </summary>
        /// <returns>Associated preference store</returns>
        private IPreferenceStore GetPreferenceStore() {
            return PreferenceStoreManager.GetInstance().GetStore(PreferenceStoreName, EStoreScope.User);
        }

        private const string PreferenceStoreName = "org.motoi.platform";
        private const string WindowStateKey = "application.window.state";
        private const string WindowWidthKey = "application.window.width";
        private const string WindowHeightKey = "application.window.height";
        private const string WindowTopKey = "application.window.top";
        private const string WindowLeftKey = "application.window.left";
    }
}