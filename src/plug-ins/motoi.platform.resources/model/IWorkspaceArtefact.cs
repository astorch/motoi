using System;

namespace motoi.platform.resources.model {
    /// <summary> Defines an artefact of a workspace. </summary>
    public interface IWorkspaceArtefact {
        /// <summary> Returns the name of the artefact. </summary>
        string Name { get; }

        /// <summary> Returns the nature of the artefact. </summary>
        string Nature { get; }

        /// <summary> Returns an URI to the location of the artefact. </summary>
        Uri Location { get; }

        /// <summary> Returns the parent of the artefact or NULL if there is none. </summary>
        IWorkspaceArtefact Parent { get; }
    }
}