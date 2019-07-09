namespace motoi.platform.ui {
    /// <summary> Describes a widget that is composite of other widgets. </summary>
    public interface IWidgetCompound : IWidget {
        
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IWidgetCompound"/> that is used by data binding operations.
    /// </summary>
    public class PWigdetCompoundControl<TControl> : PWidgetControl<TControl> where TControl : class, IWidgetCompound {
        
    }
}