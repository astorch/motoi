namespace motoi.workbench.events {
    /// <summary> Describes a selection changed event. </summary>
    public class SelectionChangedEvent {
        /// <summary> Creates a new instance with the given arguments. </summary>
        /// <param name="newSelection">New selection</param>
        /// <param name="oldSelection">Old selection</param>
        public SelectionChangedEvent(object newSelection, object oldSelection) {
            NewSelection = newSelection;
            OldSelection = oldSelection;
        }

        /// <summary> Returns the previously selected object. </summary>
        public object OldSelection { get; }

        /// <summary> Returns the newly selected object. </summary>
        public object NewSelection { get; }
    }
}