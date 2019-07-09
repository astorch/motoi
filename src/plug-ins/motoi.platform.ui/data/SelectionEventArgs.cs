using System;

namespace motoi.platform.ui.data {
    /// <summary> Defines properties of selection event args. </summary>
    public class SelectionEventArgs : EventArgs {
        /// <summary> Returns the current selection </summary>
        public object Selection { get; set; } 
    }
}