using System;
using System.Collections.Generic;
using System.IO;
using motoi.platform.resources.model;
using xcite.csharp;

namespace motoi.platform.resources.runtime.filesystem {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IWorkspaceArtefactContainer"/>.
    /// </summary>
    abstract class AbstractFileSystemWorkspaceArtefactContainer : IWorkspaceArtefactContainer {
        /// <summary>
        /// Creates a new instance using the given <paramref name="directoryInfo"/>.
        /// </summary>
        /// <param name="directoryInfo">Directory info</param>
        /// <exception cref="ArgumentNullException">If the given directory reference is NULL</exception>
        protected AbstractFileSystemWorkspaceArtefactContainer(DirectoryInfo directoryInfo) {
            FileSystemDirectory = directoryInfo ?? throw new ArgumentNullException(nameof(directoryInfo));
            Name = directoryInfo.Name;
            Location = new Uri(directoryInfo.FullName);
            ContainerArtefacts = new List<IWorkspaceArtefact>();
            Refresh(); // Todo Think about if this is a good idea
        }

        /// <inheritdoc />
        public event EventHandler Refreshed;

        /// <summary>
        /// Returns the associated file system directory.
        /// </summary>
        protected virtual DirectoryInfo FileSystemDirectory { get; }

        /// <summary>
        /// Returns a collection containing the obtained container artefacts.
        /// </summary>
        protected virtual List<IWorkspaceArtefact> ContainerArtefacts { get; }

        /// <inheritdoc />
        public virtual string Name { get; }

        /// <inheritdoc />
        public abstract string Nature { get; }

        /// <inheritdoc />
        public abstract IWorkspaceArtefact Parent { get; }

        /// <inheritdoc />
        public virtual Uri Location { get; }

        /// <inheritdoc />
        public virtual IEnumerable<IWorkspaceArtefact> Artefacts 
            => ContainerArtefacts;

        /// <inheritdoc />
        public virtual IWorkspaceArtefact GetArtefact(string name) {
            if (string.IsNullOrEmpty(name)) return null;
            using (IEnumerator<IWorkspaceArtefact> artItr = Artefacts.GetEnumerator()) {
                while (artItr.MoveNext()) {
                    IWorkspaceArtefact artefact = artItr.Current;

                    if (artefact is IWorkspaceFile workspaceFile) {
                        if (workspaceFile.Name == name) return workspaceFile;
                        continue;
                    }

                    if (artefact is IWorkspaceArtefactContainer fileContainer) {
                        if (fileContainer.Name == name) return fileContainer;

                        IWorkspaceArtefact file = fileContainer.GetArtefact(name);
                        if (file != null) return file;
                    }
                }
            }

            return null;
        }

        /// <inheritdoc />
        public virtual IEnumerable<TElement> FlatHierarchy<TElement>() where TElement : class, IWorkspaceArtefact {
            using (IEnumerator<IWorkspaceArtefact> artItr = Artefacts.GetEnumerator()) {
                while (artItr.MoveNext()) {
                    IWorkspaceArtefact workspaceArtefact = artItr.Current;

                    if (workspaceArtefact is TElement targetElement) {
                        yield return targetElement;
                    }

                    if (workspaceArtefact is IWorkspaceArtefactContainer artefactContainer) {
                        using (IEnumerator<TElement> cntItr = artefactContainer.FlatHierarchy<TElement>().GetEnumerator()) {
                            while (cntItr.MoveNext()) {
                                yield return cntItr.Current;
                            }
                        }

                    }
                }
            }
        }

        /// <inheritdoc />
        public void Refresh(ERefreshBehavior refreshBehavior) {
            PerformRefresh(refreshBehavior, FileSystemDirectory, ContainerArtefacts);
            Refreshed.Dispatch(new object[] { this, EventArgs.Empty }, OnDispatchEventException);
        }

        /// <inheritdoc />
        public virtual void Refresh() {
            Refresh(ERefreshBehavior.Infinit);
        }

        /// <summary>
        /// Tells the instance to perform the refresh using the given <paramref name="refreshBehavior"/> and <paramref name="fileSystemDirectory"/>. 
        /// The obtained artefacts have to be stored in the given <paramref name="containerArtefacts"/> set.
        /// </summary>
        /// <param name="refreshBehavior">Refresh behavior</param>
        /// <param name="fileSystemDirectory">Associated folder directory to scan</param>
        /// <param name="containerArtefacts">Set of obtained artefacts</param>
        protected abstract void PerformRefresh(ERefreshBehavior refreshBehavior, DirectoryInfo fileSystemDirectory, List<IWorkspaceArtefact> containerArtefacts);

        /// <summary>
        /// Is invoked when an error during the event dispatching occured.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="delegate">Reference the exception happened to</param>
        private void OnDispatchEventException(Exception exception, Delegate @delegate) {
            ResourceService.Instance.ResourceServiceLog.Error(exception, $"Error on dispatching event to '{@delegate}'.");
        }

        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
        /// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;
            AbstractFileSystemWorkspaceArtefactContainer container = obj as AbstractFileSystemWorkspaceArtefactContainer;
            if (container == null) return false;

            if (!Equals(Nature, container.Nature)) return false;
            if (!Equals(Name, container.Name)) return false;
            if (!Equals(Location, container.Location)) return false;

            return true;
        }

        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
        public override int GetHashCode() {
            int result = 31;
            result = 17 * result + Nature.GetHashCode();
            result = 17 * result + Name.GetHashCode();
            result = 17 * result + Location.GetHashCode();
            return result;
        }

        /// <inheritdoc />
        public override string ToString() {
            string result = string.Format("{0} ({1})", Nature, Location.LocalPath);
            return result;
        }
    }
}