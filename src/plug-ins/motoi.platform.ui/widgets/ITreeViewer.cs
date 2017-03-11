using System;
using motoi.platform.ui.data;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines the properties of a tree viewer.
    /// </summary>
    public interface ITreeViewer : IWidget, IDataViewer<ITreeContentProvider, ITreeLabelProvider> {
        ContextMenuItemProvider ContextMenuItemProvider { get; set; }
    }

    public delegate IContextMenuItem[] ContextMenuItemProvider(object item);

    public interface IContextMenuItem {
        string Name { get; }

        Action<object> Action { get; }

        Func<object, bool> IsEnabled { get; }
    }
}