using System.ComponentModel;
using motoi.platform.ui;
using motoi.platform.ui.factories;
using motoi.platform.ui.toolbars;
using motoi.platform.ui.widgets;

namespace motoi.workbench.model {
    /// <summary> Defines a view of a business driven workbench UI part. </summary>
    public interface IWorkbenchPart : IViewPart, INotifyPropertyChanged {
        /// <summary> Returns the widget factory that can be used to create widgets or does set it. </summary>
        IWidgetFactory WidgetFactory { get; set; }

        /// <summary> Tells the instance to create its content using the given widget factory. </summary>
        /// <param name="gridComposite">Panel to place the content widgets of the editor</param>
        void CreateContents(IGridPanel gridComposite);

        /// <summary> Tells the editor to add additional tool bar controls if desired. </summary>
        /// <param name="toolBar">Tool bar to configure</param>
        void ConfigureToolBar(IToolBar toolBar);
    }
}