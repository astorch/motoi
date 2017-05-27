using motoi.platform.resources.model;

namespace motoi.workbench.problemsview {
    /// <summary> Defines an item that can be displayed by the <see cref="ProblemsDataView"/>. </summary>
    public class ProblemsViewItem {
        /// <summary>
        /// Creates a new instance with the given arguments and 1 as line and column value.
        /// </summary>
        /// <param name="itemType">Item type</param>
        /// <param name="description">Item description</param>
        /// <param name="workspaceArtefact">Affected workspace artefact</param>
        public ProblemsViewItem(EProblemsViewItemType itemType, string description, IWorkspaceArtefact workspaceArtefact)
            : this(itemType, description, workspaceArtefact, 1, 1) {
            // Nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given arguments.
        /// </summary>
        /// <param name="itemType">Item type</param>
        /// <param name="description">Item description</param>
        /// <param name="workspaceArtefact">Affected workspace artefact</param>
        /// <param name="line">Line number that is associated with the item</param>
        /// <param name="column">Column number that is assocated with the item</param>
        public ProblemsViewItem(EProblemsViewItemType itemType, string description, IWorkspaceArtefact workspaceArtefact, uint line, uint column) {
            ItemType = itemType;
            Description = description;
            WorkspaceArtefact = workspaceArtefact;
            Line = line;
            Column = column;
        }

        /// <summary> Returns the type of the item. </summary>
        public EProblemsViewItemType ItemType { get; private set; }

        /// <summary> Returns the description of the item. </summary>
        public string Description { get; private set; }

        /// <summary> Returns the affected workspace artefact. </summary>
        public IWorkspaceArtefact WorkspaceArtefact { get; private set; }

        /// <summary> Returns the line number that is associated with the item. </summary>
        public uint Line { get; private set; }

        /// <summary>  Returns the column number that is associated with the item. </summary>
        public uint Column { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;
            ProblemsViewItem item = obj as ProblemsViewItem;
            if (item == null) return false;

            if (!Equals(ItemType, item.ItemType)) return false;
            if (!Equals(WorkspaceArtefact, item.WorkspaceArtefact)) return false;
            if (!Equals(Line, item.Line)) return false;
            if (!Equals(Column, item.Column)) return false;
            if (!Equals(Description, item.Description)) return false;

            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            int result = 31;
            result = 17*result + ItemType.GetHashCode();
            result = 17*result + WorkspaceArtefact.GetHashCode();
            result = 17*result + Line.GetHashCode();
            result = 17*result + Column.GetHashCode();
            result = 17*result + Description.GetHashCode();
            return result;
        }
    }
}