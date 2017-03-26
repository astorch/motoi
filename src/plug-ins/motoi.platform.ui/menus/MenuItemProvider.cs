using System;
using System.Collections.Generic;
using System.IO;
using motoi.extensions;
using motoi.extensions.core;
using motoi.platform.ui.actions;
using motoi.platform.ui.shells;
using motoi.plugins.model;
using Xcite.Collections;
using Xcite.Csharp.generics;

namespace motoi.platform.ui.menus {
    /// <summary>
    /// Implements a provider which reads all registered menus and items and tells the main window 
    /// to handle it.
    /// </summary>
    public static class MenuItemProvider {
        /// <summary>
        /// Extension point id.
        /// </summary>
        private const string ExtensionPointId = "org.motoi.application.menu";

        private static readonly OrderedDictionary<string, MenuContribution> iIdToMenuMap = new OrderedDictionary<string, MenuContribution>(10);

        /// <summary>
        /// Resolves all registered menus and items and tells the main window to handle it.
        /// </summary>
        /// <param name="mainWindow">Main window</param>
        public static void AddExtensionPointMenuItems(IMainWindow mainWindow) {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointId);
            
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
                    MenuContribution menu = new MenuContribution(id, label);
                    iIdToMenuMap.InsertLast(id, menu);
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

            using (IEnumerator<MenuContribution> enmtor = iIdToMenuMap.Values.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    mainWindow.AddMenu(enmtor.Current);
                }
            }

            // Disposing all opened streams
            using (LinkedList<Stream>.Enumerator enmtor = streamList.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    Stream stream = enmtor.Current;
                    if (stream != null)
                        stream.Dispose();
                }
            }
        }
    }
}