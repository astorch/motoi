using System;
using motoi.platform.ui.bindings;
using motoi.platform.ui.layouts;
using Xcite.Csharp.assertions;

namespace motoi.platform.ui.widgets {
    /// <summary>
    /// Defines a widget compound with a grid layout.
    /// </summary>
    public interface IGridPanel : IWidgetCompound {
        /// <summary>
        /// Returns the number of layout columns or does set it.
        /// </summary>
        int GridColumns { get; set; }

        /// <summary>
        /// Returns the number of layout rows or does set it.
        /// </summary>
        int GridRows { get; set; }

        /// <summary>
        /// Adds a new column. The column width is calculated based on the given <paramref name="value"/>.
        /// <list type="bullet">
        ///     <item>Auto - The width is calculated automatically.</item>
        ///     <item>* - The column grabs all available space.</item>
        ///     <item>125 - The column has a fixed width of 125.</item>
        /// </list>
        /// </summary>
        /// <param name="value">Value of column definition</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is NULL or empty</exception>
        /// <exception cref="FormatException">If <paramref name="value"/> cannot be parsed</exception>
        void AddColumnDefinition(string value);

        /// <summary>
        /// Adds the given widget to the next free cell of the composite.
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <param name="layoutBehaviour">Additional layout informations</param>
        void AddWidget(IWidget widget, GridLayoutBehaviour layoutBehaviour);

        /// <summary>
        /// Adds the given widget to the next free cell of the composite. 
        /// The widget gets a default layout behaviour.
        /// </summary>
        /// <param name="widget">Widget</param>
        void AddWidget(IWidget widget);
    }

    /// <summary>
    /// Provides the property meta data of <see cref="IGridPanel"/> that is used by data binding operations.
    /// </summary>
    public class PGridPanel : PWidgetControl<IGridPanel> {
        /// <summary> Grid columns property meta data </summary>
        public static readonly IBindableProperty<int> GridColumnsProperty = CreatePropertyInfo(_ => _.GridColumns, 1);

        /// <summary> Grid rows property meta data </summary>
        public static readonly IBindableProperty<int> GridRowsProperty = CreatePropertyInfo(_ => _.GridRows, 1);
    }

    /// <summary>
    /// Provides a column definition.
    /// </summary>
    public class ColumnDefinition {
        /// <summary>
        /// Creates a new instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is NULL or empty</exception>
        /// <exception cref="FormatException">If <paramref name="value"/> cannot be parsed</exception>
        public ColumnDefinition(string value) {
            string invariantValue = Assert.NotNullOrEmpty(() => value).ToLowerInvariant();

            if (invariantValue == "auto") {
                Width = 0;
                Mode = SizeBehavior.Auto;
            } else if (invariantValue == "*") {
                Width = 0;
                Mode = SizeBehavior.Fill;
            } else {
                Width = uint.Parse(invariantValue);
                Mode = SizeBehavior.Fixed;
            }
        }

        /// <summary>
        /// Returns the column width.
        /// </summary>
        public uint Width { get; private set; }

        /// <summary>
        /// Returns the column width mode.
        /// </summary>
        public SizeBehavior Mode { get; private set; }
    }

    /// <summary>
    /// Defines kindes of column width modes.
    /// </summary>
    public enum SizeBehavior : ushort {
        /// <summary>
        ///  Indicates that the column width is determined automatically.
        /// </summary>
        Auto,
        /// <summary>
        /// Indicates that the column width is as much as possible.
        /// </summary>
        Fill,
        /// <summary>
        /// Indicates that the column width is fixed.
        /// </summary>
        Fixed
    }
}