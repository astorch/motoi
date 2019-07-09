using System;
using System.Collections.Generic;

namespace motoi.platform.resources.model {
    /// <summary> Defines a workspace artefact container. </summary>
    public interface IWorkspaceArtefactContainer : IWorkspaceArtefact {
        /// <summary> Event that is raised when a refresh has been done. </summary>
        event EventHandler Refreshed;

        /// <summary> Returns all artefacts of the container. </summary>
        IEnumerable<IWorkspaceArtefact> Artefacts { get; }

        /// <summary> Performs a refresh of the artefacts using the given <paramref name="refreshBehavior"/>. </summary>
        /// <param name="refreshBehavior">Refresh behavor</param>
        void Refresh(ERefreshBehavior refreshBehavior);

        /// <summary> Performs a refresh of the artefacts using <see cref="ERefreshBehavior.Infinit"/> as refresh behavior. </summary>
        /// <seealso cref="Refresh(ERefreshBehavior)"/>
        void Refresh();

        /// <summary>
        /// Returns the artefact with the given <paramref name="name"/> that is contained by the container. If 
        /// the artefact cannot be found, NULL is returned.
        /// </summary>
        /// <param name="name">Name of the artefact to look up</param>
        /// <returns>Instance of <see cref="IWorkspaceArtefact"/> with the requested <paramref name="name"/> or NULL</returns>
        IWorkspaceArtefact GetArtefact(string name);

        /// <summary>
        /// Returns an enumerable collection of all elements of the given type <typeparamref name="TElement"/> that is contained 
        /// by the container and all of its subcontainer.
        /// </summary>
        /// <typeparam name="TElement">Type of element to select</typeparam>
        /// <returns>Collection that contains all elements of the desired type <typeparamref name="TElement"/></returns>
        IEnumerable<TElement> FlatHierarchy<TElement>() where TElement : class, IWorkspaceArtefact;
    }
}