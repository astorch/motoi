using System.Collections;
using motoi.platform.ui.bindings;
using Xcite.Csharp.lang;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines a widget that displays several data objects.
    /// </summary>
    public interface IItemsHost : IWidget {
        /// <summary>
        /// Returns the items source of the control or does set it.
        /// </summary>
        ICollection ItemsSource { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IItemsHost"/> that is used by data binding operations.
    /// </summary>
    public class PItemsHost : PItemsHostControl<IItemsHost> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IItemsHost"/> that is used by data binding operations.
    /// </summary>
    public class PItemsHostControl<TControl> : PWidgetControl<TControl> where TControl : class, IWidget {
        /// <summary>Items Source property meta data</summary>
        public static readonly IBindableProperty<ICollection> ItemsSourceProperty = CreatePropertyInfo(Name.Of<IItemsHost,ICollection>(_=>_.ItemsSource), (ICollection)null);
    }
}