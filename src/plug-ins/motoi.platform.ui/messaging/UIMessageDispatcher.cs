using System;
using xcite.csharp;
using xcite.csharp.oop;
using xcite.logging;

namespace motoi.platform.ui.messaging {
    /// <summary> Implements a dispatcher of messages to the UI elements. </summary>
    public class UIMessageDispatcher : GenericSingleton<UIMessageDispatcher> {
        private static readonly ILog _log = LogManager.GetLog(typeof(UIMessageDispatcher));

        private readonly AuxiliaryAudible<IUIMessageDispatchListener> _registeredListeners = new AuxiliaryAudible<IUIMessageDispatchListener>();

        /// <summary> Subscribes the given <paramref name="listener"/> to dispatching events. </summary>
        /// <param name="listener">Listener to subscribe</param>
        public void AddMessageDispatchListener(IUIMessageDispatchListener listener) {
            _registeredListeners.AddListener(listener);
        }

        /// <summary> Unsubscribes the given <paramref name="listener"/> from dispatching events. </summary>
        /// <param name="listener">Listener to unsubscribe</param>
        public void RemoveMessageDispatchListener(IUIMessageDispatchListener listener) {
            _registeredListeners.RemoveListener(listener);
        }

        /// <summary> Dispatches a message to UI elements using the given parameters. </summary>
        /// <param name="uiElement">UI element to notify</param>
        /// <param name="action">Action to perform</param>
        /// <param name="arguments">Action arguments</param>
        public void DispatchMessage(object uiElement, ushort action, object[] arguments) {
            UIMessage message = new UIMessage {UIElement = uiElement, Action = action, Arguments = arguments};
            _registeredListeners.Dispatch(lstnr => lstnr.OnMessageDispatch(message), OnDispatchingException);
        }

        /// <summary> Dispatches a message asynchronously to UI elements using the given parameters. </summary>
        /// <param name="uiElement">UI element to notify</param>
        /// <param name="action">Action to perform</param>
        /// <param name="arguments">Action arguments</param>
        public void DispatchMessageAsync(object uiElement, ushort action, object[] arguments) {
            UIMessage message = new UIMessage { UIElement = uiElement, Action = action, Arguments = arguments };
            _registeredListeners.Dispatch(lstnr => lstnr.OnAsyncMessageDispatch(message), OnDispatchingException);
        }

        /// <summary>
        /// Is invoked when an exception during the dispatching
        /// to the <paramref name="uiMessageDispatchListener"/> occurred.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="uiMessageDispatchListener">Listener the exception happened to</param>
        private void OnDispatchingException(Exception exception, IUIMessageDispatchListener uiMessageDispatchListener) {
            _log.Error($"Error on dispatching event to '{uiMessageDispatchListener}'.", exception);
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