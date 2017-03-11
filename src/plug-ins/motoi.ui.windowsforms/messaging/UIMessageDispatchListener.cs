using System;
using System.Collections.Generic;
using System.Windows.Forms;
using motoi.platform.ui.messaging;
using Xcite.Csharp.generics;

namespace motoi.ui.windowsforms.messaging {
    /// <summary>
    /// Implements a <see cref="IUIMessageDispatchListener"/> as singleton. The main purpose of this class 
    /// is to delegate the events to the UI thread of the application.
    /// </summary>
    public class UIMessageDispatchListener : GenericSingleton<UIMessageDispatchListener>, IUIMessageDispatchListener {
        private readonly Dictionary<Control, EventHandler<UIMessage>> iMappedMessageReceivedHandlers = new Dictionary<Control, EventHandler<UIMessage>>();

        /// <summary>
        /// Adds the given <paramref name="eventHandler"/> for the <see cref="MessageReceived"/> events that are specifically 
        /// for the given <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control which shall received events</param>
        /// <param name="eventHandler">Event handler for the control</param>
        public void AddMessageReceiverHandler(Control control, EventHandler<UIMessage> eventHandler) {
            if (control == null) return;
            if (eventHandler == null) return;

            lock (iMappedMessageReceivedHandlers) {
                iMappedMessageReceivedHandlers.Add(control, eventHandler);
            }
        }

        /// <summary>
        /// Removes all specific event handlers for the given <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control which had registered a custom event handler</param>
        public void RemoveMessageReceivedHandler(Control control) {
            if (control == null) return;

            lock (iMappedMessageReceivedHandlers) {
                iMappedMessageReceivedHandlers.Remove(control);
            }
        }

        /// <summary>
        /// Event that is raised when a message has been received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived; 

        /// <summary>
        /// Returns the currently used UI thread message dispatcher or does set it.
        /// </summary>
        public Control Dispatcher { get; set; }

        /// <summary>
        /// Notifies the instance that the given <paramref name="message"/> has been dispatched synchronously.
        /// </summary>
        /// <param name="message">Dispatched message</param>
        public void OnMessageDispatch(UIMessage message) {
            if (Dispatcher == null) return;

            // Dispatch global event
            EventHandler<MessageReceivedEventArgs> messageReceivedHandler = MessageReceived;
            if (messageReceivedHandler != null) {
                Dispatcher.Invoke(MessageReceived, this, message);
            }
            
            // Dispatch to specific control
            EventHandler<UIMessage> mappedEventHandler;
            lock (iMappedMessageReceivedHandlers) {
                if (!iMappedMessageReceivedHandlers.TryGetValue((Control) message.UIElement, out mappedEventHandler))
                    return;
            }
            Dispatcher.Invoke(mappedEventHandler, this, message);
        }

        /// <summary>
        /// Notifies the instance that the given <paramref name="message"/> has been dispatched asynchronously.
        /// </summary>
        /// <param name="message">Dispatched message</param>
        public void OnAsyncMessageDispatch(UIMessage message) {
            if (Dispatcher == null) return;

            // Dispatch global event
            EventHandler<MessageReceivedEventArgs> messageReceivedHandler = MessageReceived;
            if (messageReceivedHandler != null) {
                Dispatcher.BeginInvoke(MessageReceived, this, message);
            }

            // Dispatch to specific control
            EventHandler<UIMessage> mappedEventHandler;
            lock (iMappedMessageReceivedHandlers) {
                if (!iMappedMessageReceivedHandlers.TryGetValue((Control)message.UIElement, out mappedEventHandler))
                    return;
            }
            Dispatcher.BeginInvoke(mappedEventHandler, this, message);
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
//            UIMessageDispatcher.Instance.AddMessageDispatchListener(this);
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            // Currently nothing to do here
        }
    }
}