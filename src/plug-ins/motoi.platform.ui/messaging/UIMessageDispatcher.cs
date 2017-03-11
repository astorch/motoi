using System;
using log4net;
using Xcite.Csharp.generics;
using Xcite.Csharp.oop;

namespace motoi.platform.ui.messaging {
    /// <summary>
    /// Implements a dispatcher of messages to the UI elements.
    /// </summary>
    public class UIMessageDispatcher : GenericSingleton<UIMessageDispatcher> {
        private static readonly ILog iLog = LogManager.GetLogger(typeof(UIMessageDispatcher));

        private readonly AuxiliaryAudible<IUIMessageDispatchListener> iRegisteredListeners = new AuxiliaryAudible<IUIMessageDispatchListener>();

        /// <summary>
        /// Subscribes the given <paramref name="listener"/> to dispatching events. 
        /// </summary>
        /// <param name="listener">Listener to subscribe</param>
        public void AddMessageDispatchListener(IUIMessageDispatchListener listener) {
            iRegisteredListeners.AddListener(listener);
        }

        /// <summary>
        /// Unsubscribes the given <paramref name="listener"/> from dispatching events.
        /// </summary>
        /// <param name="listener">Listener to unsubscribe</param>
        public void RemoveMessageDispatchListener(IUIMessageDispatchListener listener) {
            iRegisteredListeners.RemoveListener(listener);
        }

        /// <summary>
        /// Dispatches a message to UI elements using the given parameters.
        /// </summary>
        /// <param name="uiElement">UI element to notify</param>
        /// <param name="action">Action to perform</param>
        /// <param name="arguments">Action arguments</param>
        public void DispatchMessage(object uiElement, ushort action, object[] arguments) {
            UIMessage message = new UIMessage {UIElement = uiElement, Action = action, Arguments = arguments};
            iRegisteredListeners.Dispatch(lstnr => lstnr.OnMessageDispatch(message), OnDispatchingException);
        }

        /// <summary>
        /// Dispatches a message asynchronously to UI elements using the given parameters.
        /// </summary>
        /// <param name="uiElement">UI element to notify</param>
        /// <param name="action">Action to perform</param>
        /// <param name="arguments">Action arguments</param>
        public void DispatchMessageAsync(object uiElement, ushort action, object[] arguments) {
            UIMessage message = new UIMessage { UIElement = uiElement, Action = action, Arguments = arguments };
            iRegisteredListeners.Dispatch(lstnr => lstnr.OnAsyncMessageDispatch(message), OnDispatchingException);
        }

        /// <summary>
        /// Is invoked when an exception during the dispatching to the <paramref name="uiMessageDispatchListener"/> occurred.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="uiMessageDispatchListener">Listener the exception happened to</param>
        private void OnDispatchingException(Exception exception, IUIMessageDispatchListener uiMessageDispatchListener) {
            iLog.ErrorFormat("Error on dispatching event to '{0}'. Reason: {1}", uiMessageDispatchListener, exception);
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            // Currently nothing to do here
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}