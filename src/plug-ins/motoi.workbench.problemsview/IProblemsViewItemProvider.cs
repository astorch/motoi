using System;
using motoi.workbench.model;

namespace motoi.workbench.problemsview {
    /// <summary>
    /// A workbench part that implements this interface offers support to provide items that can 
    /// be displayed by the <see cref="ProblemsDataView"/>.
    /// </summary>
    public interface IProblemsViewItemProvider : IWorkbenchPart {
        /// <summary>
        /// Event that is raised when items to display has been found.
        /// </summary>
        event EventHandler<ItemsFoundEventArgs> ItemsFound;
    }

    /// <summary>
    /// Defines the arguments of the <see cref="IProblemsViewItemProvider.ItemsFound"/> event.
    /// </summary>
    public class ItemsFoundEventArgs : EventArgs {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="items">Items that has been found</param>
        public ItemsFoundEventArgs(ProblemsViewItem[] items) {
            Items = items;
        }

        /// <summary>
        /// Returns the items that shall be displayed by the Problems Data View.
        /// </summary>
        public ProblemsViewItem[] Items { get; private set; }
    }
}