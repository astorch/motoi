using motoi.platform.ui.factories;
using motoi.workbench.model;
using xcite.csharp;

namespace motoi.workbench {
    /// <summary> Provides a factory for creating instances of <see cref="IPerspective"/>. </summary>
    public class PerspectiveFactory : GenericSingleton<PerspectiveFactory> {
        /// <summary>
        /// Returns an instance of a perspective referenced by the given id. If no instance 
        /// could be found NULL will returned.
        /// </summary>
        /// <param name="perspectiveId">Id of the perspective</param>
        /// <returns>Instance of perspective or null</returns>
        public IPerspective GetPerspective(string perspectiveId) {
            return GetPerspective<IMultiViewPerspective>();
        }

        /// <summary>
        /// Returns an instance of a perspective referenced by the given type. If no instance 
        /// could be found NULL is returned.
        /// </summary>
        /// <typeparam name="TPerspective">Type of perspective to create</typeparam>
        /// <returns>Instance of perspective or NULL</returns>
        public IPerspective GetPerspective<TPerspective>() where TPerspective : class, IPerspective {
            return UIFactory.NewService<TPerspective>();
        }
        
        /// <inheritdoc />
        protected override void OnInitialize() {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}