using System.Collections.Generic;

namespace motoi.workbench.problemsview {
    /// <summary>
    /// Implements a specialized <see cref="List{T}"/> of <see cref="ProblemsViewItem"/> that is 
    /// used by the <see cref="ProblemsDataView"/>.
    /// </summary>
    class ProblemsViewItemCollection : HashSet<ProblemsViewItem> {
        /// <summary>
        /// Creates a new instance with the given kind.
        /// </summary>
        /// <param name="kind"></param>
        public ProblemsViewItemCollection(EProblemsViewItemType kind) {
            Kind = kind;
        }
        
        /// <summary>
        /// Returns the kind of items the collection contains.
        /// </summary>
        public EProblemsViewItemType Kind { get; private set; }
    }
}