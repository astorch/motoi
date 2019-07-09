using System;
using System.IO;
using motoi.platform.resources.model;

namespace motoi.platform.resources.runtime.filesystem {
    /// <summary> Provides an abstract implementation of <see cref="IWorkspaceFile"/>. </summary>
    public abstract class AbstractFileSystemWorkspaceFile : IWorkspaceFile { // TODO remove public modifier
        /// <summary> Creates a new instance using the associated <paramref name="file"/>. </summary>
        /// <param name="file">File system file</param>
        /// <exception cref="ArgumentNullException">If <paramref name="file"/> is NULL</exception>
        protected AbstractFileSystemWorkspaceFile(FileInfo file) {
            FileSystemFile = file ?? throw new ArgumentNullException(nameof(file));
            Name = file.Name;
            Location = new Uri(file.FullName);
        }

        /// <summary> Returns the associated file system file. </summary>
        protected virtual FileInfo FileSystemFile { get; }

        /// <inheritdoc />
        public virtual string Name { get; }

        /// <inheritdoc />
        public virtual Uri Location { get; }

        /// <inheritdoc />
        public abstract IWorkspaceArtefact Parent { get; }

        /// <inheritdoc />
        public abstract string Nature { get; }

        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
        /// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;
            AbstractFileSystemWorkspaceFile file = obj as AbstractFileSystemWorkspaceFile;
            if (file == null) return false;

            if (!Equals(Nature, file.Nature)) return false;
            if (!Equals(Name, file.Name)) return false;
            if (!Equals(Location, file.Location)) return false;

            return true;
        }

        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
        public override int GetHashCode() {
            int result = 31;
            result = 17*result + Nature.GetHashCode();
            result = 17*result + Name.GetHashCode();
            result = 17*result + Location.GetHashCode();
            return result;
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Nature} ({Location.LocalPath})";
        }
    }
}