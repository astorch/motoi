﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using motoi.extensions;
using motoi.platform.nls;
using motoi.platform.ui.actions;
using motoi.platform.ui.shells;
using motoi.plugins;
using xcite.csharp;

namespace motoi.platform.ui.toolbars {
    /// <summary>
    /// Implements a provider which reads all registered toolbar items
    /// and tells the main window to handle it.
    /// </summary>
    public static class ToolbarItemProvider {
        /// <summary> Extension point id. </summary>
        private const string ExtensionPointId = "org.motoi.application.toolbar";

        private static readonly IDictionary<string, ToolbarGroupContribution> _idToMenuMap = new Dictionary<string, ToolbarGroupContribution>(10);

        /// <summary> Resolves all registered menus and items and tells the main window to handle it. </summary>
        /// <param name="mainWindow">Main window</param>
        public static void AddExtensionPointToolbarItems(IMainWindow mainWindow) {
            IConfigurationElement[] configurationElements = ExtensionService.Instance.GetConfigurationElements(ExtensionPointId);

            IConfigurationElement[] groupElements = configurationElements.Where(x => x.Prefix == "toolbarGroup").ToArray();
            IConfigurationElement[] groupItemElements = configurationElements.Where(x => x.Prefix == "toolbarItem").ToArray();

            for (int i = -1; ++i < groupElements.Length; ) {
                IConfigurationElement element = groupElements[i];
                string id = element["id"];
                ToolbarGroupContribution menu = new ToolbarGroupContribution(id);
                _idToMenuMap.Add(id, menu);
            }

            // Collection of all opened streams
            List<Stream> streamList = new List<Stream>(20);

            for (int i = -1; ++i < groupItemElements.Length; ) {
                IConfigurationElement element = groupItemElements[i];
                string id = element["id"];
                string group = element["group"];
                string handler = element["handler"];
                string image = element["image"];
                string label = element["label"];

                IBundle providingBundle = ExtensionService.Instance.GetProvidingBundle(element);

                IActionHandler actionHandler = null;
                if (!string.IsNullOrEmpty(handler)) {
                    Type handlerType = TypeLoader.TypeForName(providingBundle, handler);
                    actionHandler = handlerType.NewInstance<IActionHandler>();
                }

                Stream imageStream = null;
                if (!string.IsNullOrEmpty(image)) {
                    imageStream = providingBundle.GetAssemblyResourceAsStream(image);
                    streamList.Add(imageStream);
                }

                // NLS support
                label = NLS.Localize(label, actionHandler);

                ToolbarItemContribution menuItem = new ToolbarItemContribution(id, group, actionHandler, label, imageStream);
                ToolbarGroupContribution menuInstance;
                if (_idToMenuMap.TryGetValue(group, out menuInstance))
                    menuInstance.GroupItems.Add(menuItem);
            }

            using (IEnumerator<ToolbarGroupContribution> itr = _idToMenuMap.Values.GetEnumerator()) {
                while (itr.MoveNext()) {
                    ToolbarGroupContribution contribution = itr.Current;
                    mainWindow.AddToolbarGroup(contribution);
                }
            }

            // Disposing all opened streams
            using (IEnumerator<Stream> itr = streamList.GetEnumerator()) {
                while (itr.MoveNext()) {
                    Stream stream = itr.Current;
                    stream?.Dispose();
                }
            }
        }
    }
}