using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using motoi.extensions;
using motoi.extensions.core;
using motoi.platform.nls;
using motoi.platform.ui.actions;
using motoi.platform.ui.shells;
using motoi.plugins.model;
using xcite.collections;
using xcite.csharp;

namespace motoi.platform.ui.menus {
    /// <summary>
    /// Implements a provider which reads all registered menus and items and tells the main window 
    /// to handle it.
    /// </summary>
    public static class MenuItemProvider {
        /// <summary> Extension point ID of menu items </summary>
        private const string ExtensionPointIdMenuItems = "org.motoi.application.menu";
        
        /// <summary> Extension point ID of custom menu configurer </summary>
        private const string ExtensionPointIdCustomMenuConfigurer = "org.motoi.application.menu.customconfigurer";
        
        private static readonly Dictionary<string, MenuContribution> iIdToMenuMap = new Dictionary<string, MenuContribution>(10);

        /// <summary>
        /// Resolves all registered menus and items and tells the main window to handle it.
        /// </summary>
        /// <param name="mainWindow">Main window</param>
        public static void AddExtensionPointMenuItems(IMainWindow mainWindow) {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointIdMenuItems);
            
            LinearList<IConfigurationElement> menuElementCollection = new LinearList<IConfigurationElement>();
            LinearList<IConfigurationElement> menuItemElementsCollection = new LinearList<IConfigurationElement>();
            LinearList<IConfigurationElement> menuItemSeparatorsCollection = new LinearList<IConfigurationElement>();

            Func<string, ICollection<IConfigurationElement>> getCollection = prefix => {
                if (prefix == "menu") return menuElementCollection;
                if (prefix == "menuItem") return menuItemElementsCollection;
                if (prefix == "separator") return menuItemSeparatorsCollection;
                return null;
            };

            // Sort elements into corresponding collections
            for (int i = -1; ++i != configurationElements.Length;) {
                IConfigurationElement element = configurationElements[i];
                string prefix = element.Prefix;
                ICollection<IConfigurationElement> collection = getCollection(prefix);
                if (collection != null)
                    collection.Add(element);
            }

            // Process registered menus
            using (IEnumerator<IConfigurationElement> itr = menuElementCollection.GetEnumerator()) {
                while (itr.MoveNext()) {
                    IConfigurationElement element = itr.Current;
                    string id = element["id"];
                    string label = element["label"];

                    // NLS support
                    if (label.StartsWith("%")) {
                        string nlsKey = label.Substring(1);
                        string assemblyName = ExtensionService.Instance.GetProvidingBundle(element).Name;
                        // We cannot use the AppDomain here, because the assembly must not have been loaded yet
                        Assembly assembly = Assembly.Load(assemblyName); // TODO Check if this may be an issue
                        string localizationId = NLS.GetLocalizationId(assembly);
                        label = NLS.GetText(localizationId, nlsKey);
                    }

                    MenuContribution menu = new MenuContribution(id, label);
                    iIdToMenuMap.Add(id, menu);
                }
            }

            // Collection of all opened streams
            LinkedList<Stream> streamList = new LinkedList<Stream>();

            // Process registered menu items
            using (IEnumerator<IConfigurationElement> itr = menuItemElementsCollection.GetEnumerator()) {
                while (itr.MoveNext()) {
                    IConfigurationElement element = itr.Current;
                    string id = element["id"];
                    string menu = element["menu"];
                    string label = element["label"];
                    string handler = element["handler"];
                    string shortcut = element["shortcut"];
                    string image = element["image"];

                    IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(element);

                    IActionHandler actionHandler = null;
                    if (!string.IsNullOrEmpty(handler)) {
                        Type handlerType = TypeLoader.TypeForName(providingBundle, handler);
                        actionHandler = handlerType.NewInstance<IActionHandler>();
                    }

                    Stream imageStream = null;
                    if (!string.IsNullOrEmpty(image)) {
                        imageStream = providingBundle.GetAssemblyResourceAsStream(image);
                        streamList.AddLast(imageStream);
                    }

                    // NLS support
                    label = NLS.Localize(label, actionHandler);

                    MenuItemContribution menuItem = new MenuItemContribution(id, label, menu, actionHandler, shortcut, imageStream);
                    MenuContribution menuInstance;
                    if (iIdToMenuMap.TryGetValue(menu, out menuInstance))
                        menuInstance.MenuItems.Add(menuItem);
                }
            }

            // Process registered separators
            using (IEnumerator<IConfigurationElement> itr = menuItemSeparatorsCollection.GetEnumerator()) {
                while (itr.MoveNext()) {
                    IConfigurationElement element = itr.Current;
                    string id = element["id"];
                    string menu = element["menu"];
                    string insertBefore = element["insertBefore"];
                    string insertAfter = element["insertAfter"];

                    MenuItemContribution menuItem = new MenuItemContribution(id, string.Empty, menu, null, null, null) { IsSeparator = true };
                    MenuContribution menuContribution;
                    if (!iIdToMenuMap.TryGetValue(menu, out menuContribution)) continue;

                    bool useInsertBefore = !string.IsNullOrEmpty(insertBefore);
                    string itemReference = useInsertBefore ? insertBefore : insertAfter;
                    IList<MenuItemContribution> menuItemCollection = menuContribution.MenuItems;
                    int referenceIndex = menuItemCollection.IndexOf(item => item.Id == itemReference);
                    if (referenceIndex == -1) continue;

                    int insertIndex = useInsertBefore ? referenceIndex : referenceIndex + 1;
                    menuItemCollection.Insert(insertIndex, menuItem);
                }
            }

            // Support custom menu configurer
            ICustomMenuConfigurer customMenuConfigurer = GetCustomMenuConfigurer();
            MenuContribution[] menuContributions = iIdToMenuMap.Values.ToArray();
            MenuContribution[] customMenuContributions = customMenuConfigurer != null
                ? customMenuConfigurer.Configure(menuContributions)
                : menuContributions;

            // Add each contributed menu to the main window
            Array.ForEach(customMenuContributions, mainWindow.AddMenu);

            // Disposing all opened streams
            using (LinkedList<Stream>.Enumerator enmtor = streamList.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    Stream stream = enmtor.Current;
                    if (stream != null)
                        stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns an instance of <see cref="ICustomMenuConfigurer"/> if any has been registered 
        /// using the associated extension point. If no type has been contributed, NULL is returned.
        /// </summary>
        /// <returns>An instance of <see cref="ICustomMenuConfigurer"/> or NULL</returns>
        private static ICustomMenuConfigurer GetCustomMenuConfigurer() {
            IConfigurationElement[] customMenuConfigurerElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointIdCustomMenuConfigurer);
            if (customMenuConfigurerElements == null || customMenuConfigurerElements.Length == 0) return null;

            IConfigurationElement customMenuConfigurationElement = customMenuConfigurerElements[0];
            string id = customMenuConfigurationElement["id"];
            string clsName = customMenuConfigurationElement["class"];
            if (string.IsNullOrEmpty(clsName)) return null;

            IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(customMenuConfigurationElement);
            try {
                Type instanceType = TypeLoader.TypeForName(providingBundle, clsName);
                return instanceType.NewInstance<ICustomMenuConfigurer>();
            } catch (Exception ex) {
                ILog logWriter = LogManager.GetLogger(typeof(MenuItemProvider));
                logWriter.Error(string.Format("Error on creating instance of custom menu configurer. " +
                                              "Id is '{0}'. Providing bundle is '{1}'", id, providingBundle), 
                                              ex);
                return null;
            }
        }
    }
}