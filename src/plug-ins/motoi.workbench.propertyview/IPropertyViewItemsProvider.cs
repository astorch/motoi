using System.Collections.Generic;
using motoi.platform.resources.model;

namespace motoi.workbench.propertyview {
    /// <summary>
    /// Defines a provider of items that are displayed by the <see cref="PropertyDataView"/>.
    /// </summary>
    public interface IPropertyViewItemsProvider {
        /// <summary>
        /// Returns all properties that are associated with the given <paramref name="workspaceArtefact"/>.
        /// </summary>
        /// <param name="workspaceArtefact">Workspace artefact the properties are requested for</param>
        /// <returns>Set of properties or NULL if there aren't some</returns>
        IDictionary<string, object> GetProperties(IWorkspaceArtefact workspaceArtefact);
    }
}