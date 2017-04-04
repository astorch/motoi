using System;
using motoi.platform.ui.data;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines the properties of a tree viewer.
    /// </summary>
    public interface ITreeViewer : IWidget, IDataViewer<ITreeContentProvider, ITreeLabelProvider> {
        ContextMenuItemProvider ContextMenuItemProvider { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="ITreeViewer"/> that is used by data binding operations.
    /// </summary>
    public class PTreeViewer : PWidget<ITreeViewer> {
        
    }

    public delegate IContextMenuItem[] ContextMenuItemProvider(object item);

    public interface IContextMenuItem {
        string Name { get; }

        Action<object> Action { get; }

        Func<object, bool> IsEnabled { get; }
    }
}