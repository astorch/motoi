using System;

namespace motoi.platform.ui.data {
    /// <summary> Defines the interface of a data viewer. </summary>
    /// <typeparam name="TContentProvider">Type of content provider</typeparam>
    /// <typeparam name="TLabelProvider">Type of label provider</typeparam>
    public interface IDataViewer<TContentProvider, TLabelProvider> 
        where TContentProvider : IContentProvider 
        where TLabelProvider : ILabelProvider {

        /// <summary> The handler will be notified when a new selection has been made. </summary>
        event EventHandler<SelectionEventArgs> SelectionChanged;

        /// <summary> The handler will be notified when a selection has been made using a double click. </summary>
        event EventHandler<SelectionEventArgs> SelectionDoubleClicked;

        /// <summary> Returns the input of the data viewer or does set it. </summary>
        object Input { get; set; }
        
        /// <summary> Returns the currently used content provider or does set it. </summary>
        TContentProvider ContentProvider { get; set; }

        /// <summary> Returns the currently used label provider or does set it. </summary>
        TLabelProvider LabelProvider { get; set; }

        /// <summary> Refreshs the viewer. Only visual properties will be updated. </summary>
        void Refresh();

        /// <summary> Fully re-creates the tree. Structural and visual properties will be updated. </summary>
        void Update();
    }
}