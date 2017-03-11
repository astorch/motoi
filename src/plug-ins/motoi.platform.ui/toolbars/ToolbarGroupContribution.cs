using System.Collections.Generic;
using Xcite.Collections;

namespace motoi.platform.ui.toolbars {
    /// <summary>
    /// Defines a toolbar group contribution.
    /// </summary>
    public class ToolbarGroupContribution {
        /// <summary>
        /// Creates a new instance with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Unique id of the group</param>
        public ToolbarGroupContribution(string id) {
            Id = id;
            GroupItems = new LinearList<ToolbarItemContribution>();
        }

        /// <summary>
        /// Returns the unique id of the group.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Returns all associated group items.
        /// </summary>
        public ICollection<ToolbarItemContribution> GroupItems { get; private set; }
    }
}