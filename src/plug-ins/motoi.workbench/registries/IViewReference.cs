using motoi.platform.ui;

namespace motoi.workbench.registries {
    /// <summary> Describes a reference of a view. </summary>
    public interface IViewReference {
        /// <summary> Returns the id of the view. </summary>
        string Id { get; }

        /// <summary> Returns the title of the view. </summary>
        string Title { get; }

        /// <summary>
        /// Returns the instance referenced by this object. Returns NULL if 
        /// the view hasn't been opened yet or failed to be restored.
        /// </summary>
        /// <param name="restore">TRUE if the view shall be tried to restored</param>
        /// <returns></returns>
        IViewPart GetInstance(bool restore);
    }
}