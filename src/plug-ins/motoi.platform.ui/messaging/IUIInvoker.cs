using System;

namespace motoi.platform.ui.messaging {
    /// <summary>
    /// Defines methods to perform actions on the UI thread.
    /// </summary>
    public interface IUIInvoker {
        /// <summary>
        /// Returns TRUE if current thread is not the UI thread and therefore an <see cref="Invoke"/> is required 
        /// to execute an action by it.
        /// </summary>
        /// <returns>TRUE if the current thread is not the UI thread</returns>
        bool RequiresInvoke();

        /// <summary>
        /// Invokes the given <paramref name="action"/> on the UI thread. It blocks the current thread until 
        /// the action has been executed.
        /// </summary>
        /// <param name="action">Action to be executed</param>
        void Invoke(Action action);

        /// <summary>
        /// Invokes the given <paramref name="action"/> on the UI thread using the given <paramref name="value"/>. 
        /// It blocks the current thread until the action has been executed.
        /// </summary>
        /// <typeparam name="TValue">Type of the action parameter</typeparam>
        /// <param name="action">Action to be executed</param>
        /// <param name="value">Action argument</param>
        void Invoke<TValue>(Action<TValue> action, TValue value);

        /// <summary>
        /// Invokes the given <paramref name="action"/> asynchronously on the UI thread. It doesn't block the current thread.
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result of the operation</returns>
        IAsyncResult InvokeAsync(Action action);

        /// <summary>
        /// Invokes the given <paramref name="action"/> asynchronously on the UI thread. It doesn't block the current thread.
        /// </summary>
        /// <typeparam name="TValue">Type of the action parameter</typeparam>
        /// <param name="action">Action to be executed</param>
        /// <param name="value">Action argument</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result of the operation</returns>
        IAsyncResult InvokeAsync<TValue>(Action<TValue> action, TValue value);
    }
}