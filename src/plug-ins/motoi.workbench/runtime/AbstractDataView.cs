using motoi.platform.ui.images;
using motoi.workbench.model;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IDataView"/>
    /// based on <see cref="AbstractWorkbenchPart"/>.
    /// </summary>
    public abstract class AbstractDataView : AbstractWorkbenchPart, IDataView {
        /// <summary> Tells the instance to initialize its state. </summary>
        public abstract void Init();

        /// <summary> Returns the name of the data view. </summary>
        public abstract string Name { get; }

        /// <summary> Returns the image of the data view. </summary>
        public abstract ImageDescriptor Image { get; }
    }
}