using System;
using motoi.platform.ui;
using motoi.platform.ui.factories;
using motoi.ui.windowsforms.jobs;
using motoi.ui.windowsforms.shells;
using motoi.workbench.model;
using motoi.workbench.model.jobs;

namespace motoi.ui.windowsforms {
    /// <summary>
    /// Implements <see cref="IUIServiceFactory"/> for the windows forms UI platform.
    /// </summary>
    public class ServiceFactory : IUIServiceFactory {
        /// <inheritdoc />
        public TService GetService<TService>() where TService : class, IUIService {
            Type type = typeof(TService);

            if (type.IsAssignableFrom(typeof(IProgressMonitor)))
                return ProgressMonitor.GetInstance() as TService;

            if (type.IsAssignableFrom(typeof(ISingleViewPerspective)))
                return new SingleViewPerspective() as TService;

            if (type.IsAssignableFrom(typeof(IMultiViewPerspective)))
                return new MultiViewPerspective() as TService;

            return null;
        }

        /// <inheritdoc />
        public bool TryGetService<TService>(out TService service) where TService : class, IUIService {
            service = GetService<TService>();
            return service != null;
        }
    }
}