using System;
using System.IO;
using motoi.platform.resources.model;
using motoi.platform.resources.model.editors;
using Xcite.Csharp.assertions;

namespace motoi.platform.resources.runtime.editors {
    /// <summary>
    /// Provides an implementation of <see cref="IEditorInput"/> that operates in-memory.
    /// </summary>
    public class MemoryFileEditorInput : IEditorInput {
        private readonly byte[] iFileData;

        /// <summary>
        /// Creates a new instance with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the editor input</param>
        public MemoryFileEditorInput(string name) : this(name, new byte[0]) {
            Name = name;
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="name"/> and <paramref name="fileData"/>.
        /// </summary>
        /// <param name="name">Name of the editor input</param>
        /// <param name="fileData">Data of the editor input</param>
        public MemoryFileEditorInput(string name, byte[] fileData) {
            Name = Assert.NotNull(() => name);
            iFileData = fileData ?? new byte[0];
        }

        /// <inheritdoc />
        public virtual string Name { get; private set; }

        /// <inheritdoc />
        public virtual string Nature { get { return "Memory.File"; } }

        /// <inheritdoc />
        public virtual Uri Location { get { return null; } }

        /// <inheritdoc />
        public virtual IWorkspaceArtefact Parent { get { return null; } }

        /// <inheritdoc />
        public virtual Stream OpenRead() {
            return new MemoryStream(iFileData);
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"/>
        public virtual Stream OpenWrite() {
            throw new NotSupportedException();
        }
    }
}