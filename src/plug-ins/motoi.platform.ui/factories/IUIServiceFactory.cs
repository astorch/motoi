namespace motoi.platform.ui.factories {
    /// <summary> Defines a factory for UI services. </summary>
    /// <seealso cref="IUIService"/>
    public interface IUIServiceFactory {
        /// <summary>
        /// Returns a new instance of <typeparamref name="TService"/> if the underlying 
        /// UI platform supports this.
        /// Note, an UI platform must not provide each service type.
        /// </summary>
        /// <typeparam name="TService">Type of service instance to create</typeparam>
        /// <returns>Created service instance or NULL</returns>
        TService GetService<TService>() where TService : class, IUIService;

        /// <summary>
        /// Returns TRUE if a new instance of <typeparamref name="TService"/> has been 
        /// created. The new instance is provided by the out parameter <paramref name="service"/>. 
        /// Note, an UI platform must not provide each service type.
        /// </summary>
        /// <typeparam name="TService">Type of service instance to create</typeparam>
        /// <param name="service">Creates service instance or NULL</param>
        /// <returns>TRUE if the service type is supported</returns>
        bool TryGetService<TService>(out TService service) where TService : class, IUIService;
    }
}