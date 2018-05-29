using xcite.csharp;

namespace motoi.plugins {
    /// <summary>
    /// Defines the common stub of a factory.
    /// </summary>
    /// <typeparam name="T">Type of the factory</typeparam>
    class AbstractFactory<T> : GenericSingleton<T> where T : GenericSingleton<T> {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected AbstractFactory() { }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            // Currently nothing to do here
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            // Currently nothing to do here
        }
    }
}