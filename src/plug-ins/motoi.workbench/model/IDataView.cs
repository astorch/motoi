using motoi.platform.ui.images;

namespace motoi.workbench.model {
    /// <summary> Defines a data view. </summary>
    public interface IDataView : IWorkbenchPart {
        /// <summary> Tells the instance to initialize its state. </summary>
        void Init();

        /// <summary> Returns the name of the data view. </summary>
        string Name { get; }

        /// <summary> Returns the image of the data view. </summary>
        ImageDescriptor Image { get; }
    }
}