namespace motoi.platform.ui.messaging {
    /// <summary>
    /// Defines a listener of dispatching events from the <see cref="UIMessageDispatcher"/>.
    /// </summary>
    public interface IUIMessageDispatchListener {
        /// <summary>
        /// Notifies the instance that the given <paramref name="message"/> has been dispatched synchronously.
        /// </summary>
        /// <param name="message">Dispatched message</param>
        void OnMessageDispatch(UIMessage message);

        /// <summary>
        /// Notifies the instance that the given <paramref name="message"/> has been dispatched asynchronously.
        /// </summary>
        /// <param name="message">Dispatched message</param>
        void OnAsyncMessageDispatch(UIMessage message);
    }
}