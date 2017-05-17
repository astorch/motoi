using System;
using motoi.platform.ui;
using motoi.platform.ui.factories;
using motoi.platform.ui.widgets;
using motoi.ui.windowsforms.controls;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Implements <see cref="IWidgetFactory"/> for the windows forms UI platform.
    /// </summary>
    public class WidgetFactory : IWidgetFactory {
        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TWidget">Type of the widget to create</typeparam>
        /// <param name="widgetCompound">Parent element that contains this widget</param>
        /// <returns>Newly created instance of the given widget type</returns>
        public TWidget CreateInstance<TWidget>(IWidgetCompound widgetCompound) 
            where TWidget : class, IWidget {
            Type type = typeof (TWidget);

            if (type.IsAssignableFrom(typeof(IGridPanel)))
                return new GridPanel() as TWidget;

            if (type.IsAssignableFrom(typeof(ITreeViewer)))
                return new TreeViewer() as TWidget;

            if (type.IsAssignableFrom(typeof(IListViewer)))
                return new ListViewer() as TWidget;

            if (type.IsAssignableFrom(typeof (ITextBox)))
                return new TextBox() as TWidget;

            if (type.IsAssignableFrom(typeof(IRichTextBox)))
                return new RichTextBox() as TWidget;

            if (type.IsAssignableFrom(typeof(IContentAssistTextBox)))
                return new ContentAssistTextBox() as TWidget;

            if (type.IsAssignableFrom(typeof(IComboBox)))
                return new ComboBox() as TWidget;

            if (type.IsAssignableFrom(typeof(ICheckBox)))
                return new CheckBox() as TWidget;

            if (type.IsAssignableFrom(typeof(ITextBlock)))
                return new TextBlock() as TWidget;

            return null;
        }
    }
}