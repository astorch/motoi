﻿using motoi.platform.ui.bindings;

namespace motoi.platform.ui {
    /// <summary> Defines a widget. </summary>
    public interface IWidget : IDataBindingSupport {
        /// <summary> Returns TRUE, if the widget is enabled or does set it. </summary>
        bool Enabled { get; set; }

        /// <summary> Returns the visibility of the widget or does set. </summary>
        EVisibility Visibility { get; set; }
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IWidget"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PWidget : PWidgetControl<IWidget> {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IWidget"/>
    /// that is used by data binding operations.
    /// </summary>
    public class PWidgetControl<TControl> : BindableObject<TControl> where TControl : class, IWidget {
        /// <summary> Enabled property meta data </summary>
        public static readonly IBindableProperty<bool> EnabledProperty = CreatePropertyInfo(nameof(IWidget.Enabled), true, true);

        /// <summary> Visibility property meta data </summary>
        public static readonly IBindableProperty<EVisibility> VisibilityProperty = CreatePropertyInfo(nameof(IWidget.Visibility), EVisibility.Visible);
    }
}