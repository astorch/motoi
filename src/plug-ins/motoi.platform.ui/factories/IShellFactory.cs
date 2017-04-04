namespace motoi.platform.ui.factories {
    /// <summary>
    /// Provides methods to create instances of <see cref="IShell"/>.
    /// </summary>
    public interface IShellFactory {
        /// <summary>
        /// Return a new instance of the given type.
        /// </summary>
        /// <typeparam name="TShell">Type of shell to create</typeparam>
        /// <returns>Newly created instance of the given type</returns>
        TShell CreateInstance<TShell>() where TShell : class, IShell;
    }
}