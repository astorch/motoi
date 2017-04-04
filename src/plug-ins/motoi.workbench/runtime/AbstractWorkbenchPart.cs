using motoi.platform.ui;
using motoi.platform.ui.factories;
using motoi.platform.ui.toolbars;
using motoi.platform.ui.widgets;
using motoi.workbench.bindings;
using motoi.workbench.model;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IWorkbenchPart"/>.
    /// </summary>
    public abstract class AbstractWorkbenchPart : WorkbenchPropertyChangedDispatcher, IWorkbenchPart {
        /// <summary>
        /// Returns the widget factory that can be used to create widgets or does set it.
        /// </summary>
        public abstract IWidgetFactory WidgetFactory { get; set; }

        /// <summary>
        /// Tells the instance to create its content using the given widget factory.
        /// </summary>
        /// <param name="gridComposite">Panel to place the content widgets of the editor</param>
        public abstract void CreateContents(IGridComposite gridComposite);

        /// <inheritdoc />
        /// <remarks>This method is intended to be overriden by clients</remarks>
        public virtual void ConfigureToolBar(IToolBar toolBar) {
            // Clients may override
        }
    }
}