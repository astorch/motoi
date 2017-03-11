using System;

namespace Xcite.Csharp.generics {
    /// <summary>
    /// Provides a default stub of a singleton class.
    /// </summary>
    /// <typeparam name="TClass">Class type of singleton</typeparam>
    public abstract class GenericSingleton<TClass> 
        where TClass : GenericSingleton<TClass> {
        private static TClass iInstance;

        /// <summary>
        /// Returns the instance.
        /// </summary>
        public static TClass Instance {
            get {
                lock (typeof (TClass)) {
                    if (iInstance != null) return iInstance;
                    iInstance = typeof (TClass).NewInstance<TClass>(true);
                    iInstance.OnInitialize();
                    return iInstance;
                }
            }
        }

        private bool iDestroyed;

        /// <summary>
        /// Disposes this instance. Afterwards a new instance can be created.
        /// </summary>
        public void Destroy() {
            lock (typeof(TClass)) {
                OnDestroy();
                iInstance = null;
                iDestroyed = true;
            }
        }

        /// <summary>
        /// (Convenience method) This method does in general nothing. It can be used as 
        /// utility method to create the singleton without explicity method invocation. 
        /// <code>
        /// MySingleton.Instance.Handshake();
        /// </code>
        /// Hint: Derived classes may implement this.
        /// </summary>
        public virtual void Handshake() {
            // Can be overriden
        }

        /// <summary>
        /// Throws an exception if this instance has been destroyed.
        /// </summary>
        protected void AssertNotDestroyed() {
            if (iDestroyed) throw new ObjectDisposedException(typeof (TClass).Name, "Instance has been destroyed");
        }

        #region Abstract methods

        /// <summary>
        /// Will be called when <see cref="Destroy"/> has been called for this instance.
        /// </summary>
        protected abstract void OnDestroy();

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected abstract void OnInitialize();

        #endregion

    }
}