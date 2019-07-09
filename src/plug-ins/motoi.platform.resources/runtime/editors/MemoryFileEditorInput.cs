using System;
using System.IO;
using motoi.platform.resources.model;
using motoi.platform.resources.model.editors;

namespace motoi.platform.resources.runtime.editors {
    /// <summary> Provides an implementation of <see cref="IEditorInput"/> that operates in-memory. </summary>
    public class MemoryFileEditorInput : IEditorInput {
        private readonly byte[] iFileData;

        /// <summary> Creates a new instance with the given <paramref name="name"/>. </summary>
        /// <param name="name">Name of the editor input</param>
        public MemoryFileEditorInput(string name) : this(name, new byte[0]) {
            Name = name;
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="name"/>
        /// and <paramref name="fileData"/>.
        /// </summary>
        /// <param name="name">Name of the editor input</param>
        /// <param name="fileData">Data of the editor input</param>
        public MemoryFileEditorInput(string name, byte[] fileData) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            iFileData = fileData ?? new byte[0];
        }

        /// <inheritdoc />
        public virtual string Name { get; }

        /// <inheritdoc />
        public virtual string Nature 
            => "Memory.File";

        /// <inheritdoc />
        public virtual Uri Location 
            => null;

        /// <inheritdoc />
        public virtual IWorkspaceArtefact Parent 
            => null;

        /// <inheritdoc />
        public virtual Stream OpenRead() {
            return new MemoryStream(iFileData);
        }

        /// <inheritdoc />
        public virtual Stream OpenWrite() {
            throw new NotSupportedException();
        }
    }
}