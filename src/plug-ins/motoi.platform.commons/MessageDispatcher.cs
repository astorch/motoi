using System;
using System.Linq;
using System.Threading;
using log4net;
using Xcite.Collections;
using Xcite.Csharp.generics;
using Xcite.Csharp.lang;

namespace motoi.platform.commons {
    /// <summary>
    /// Provides a common message dispatcher.
    /// </summary>
    public class MessageDispatcher : GenericSingleton<MessageDispatcher> {
        private readonly ILog iLog = LogManager.GetLogger(typeof(MessageDispatcher));
        
        private readonly AutoLockStruct<bool> iIsAlive = new AutoLockStruct<bool>();
        private readonly AutoLockObject<LinearList<ConsumerEntry>> iRegisteredConsumers = new AutoLockObject<LinearList<ConsumerEntry>>(new LinearList<ConsumerEntry>());
        private readonly AutoLockObject<LinearList<EnqueuedEvent>> iEventQueue = new AutoLockObject<LinearList<EnqueuedEvent>>(new LinearList<EnqueuedEvent>());
        private readonly ManualResetEventSlim iProcessQueueEvent = new ManualResetEventSlim(false);

        /// <summary>
        /// Subscribes the given <paramref name="messageConsumer"/> to the dispatcher.
        /// </summary>
        /// <param name="messageConsumer">Message consumer</param>
        /// <param name="dataObject">Additional data object that is passed to the consumer each time</param>
        public void Subscribe(IMessageConsumer messageConsumer, object dataObject = null) {
            if (messageConsumer == null) return;

            iRegisteredConsumers.Access(_ => _.Add(new ConsumerEntry(messageConsumer, dataObject)));
        }

        /// <summary>
        /// Unsubscribes the given <paramref name="messageConsumer"/> from the dispatcher.
        /// </summary>
        /// <param name="messageConsumer">Message consumer</param>
        public void Unsubscribe(IMessageConsumer messageConsumer) {
            if (messageConsumer == null) return;

            ConsumerEntry[] registeredConsumer = iRegisteredConsumers.Access(_ => _.ToArray());
            ConsumerEntry entry = registeredConsumer.FirstOrDefault(_ => _.MessageConsumer == messageConsumer);
            if (entry == null) return;

            iRegisteredConsumers.Access(_ => _.Remove(entry));
        }

        /// <summary>
        /// Enqueues a new message with the given arguments. The message is being dispatched the next time 
        /// the dispatcher gains control.
        /// </summary>
        /// <param name="sender">Message sender</param>
        /// <param name="eventArguments">Message arguments</param>
        public void Enqueue(object sender, object eventArguments) {
            iEventQueue.Access(_ => _.Add(new EnqueuedEvent(sender, eventArguments)));
            iProcessQueueEvent.Set();
        }

        /// <summary>
        /// Is invoked when the message dispatcher thread has been started.
        /// </summary>
        /// <param name="o">Reference to the message dispatcher</param>
        private void ReadAndDispatch(object o) {
            MessageDispatcher messageDispatcher = (MessageDispatcher) o;
            
            while (iIsAlive.Get()) {
                // Wait until a signal is received
                messageDispatcher.iProcessQueueEvent.Wait();

                // Reset
                messageDispatcher.iProcessQueueEvent.Reset();

                // Collect consumer and events
                EnqueuedEvent[] queuedEvents = messageDispatcher.iEventQueue.Access(list => { var buffer = list.ToArray(); list.Clear(); return buffer; });
                ConsumerEntry[] registeredConsumers = messageDispatcher.iRegisteredConsumers.Access(_ => _.ToArray());

                // Dispatch
                for (int i = -1; ++i != queuedEvents.Length;) {
                    EnqueuedEvent queuedEvent = queuedEvents[i];

                    for (int j = -1; ++j != registeredConsumers.Length;) {
                        ConsumerEntry consumerEntry = registeredConsumers[j];

                        try {
                            consumerEntry.MessageConsumer.OnMessageReceived(queuedEvent.Sender, queuedEvent.EventArguments, consumerEntry.DataObject);
                        } catch (Exception ex) {
                            iLog.ErrorFormat("Error on dispatch event to '{0}'. Reason: {1}", consumerEntry.MessageConsumer.GetType(), ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Will be called directly after this instance has been created.
        /// </summary>
        protected override void OnInitialize() {
            iIsAlive.Set(true);
            Thread thread = new Thread(ReadAndDispatch) {Name = "Motoi Message Dispatcher", IsBackground = true};
            thread.Start(this);
        }

        /// <summary>
        /// Will be called when <see cref="GenericSingleton{TClass}.Destroy"/> has been called for this instance.
        /// </summary>
        protected override void OnDestroy() {
            iIsAlive.Set(false);
        }

        /// <summary>
        /// Describes an enqueued event that waits for dispatching.
        /// </summary>
        class EnqueuedEvent {
            /// <summary>
            /// Creates a new instance with the given arguments.
            /// </summary>
            /// <param name="sender">Message sender</param>
            /// <param name="eventArguments">Message arguments</param>
            public EnqueuedEvent(object sender, object eventArguments) {
                Sender = sender;
                EventArguments = eventArguments;
            }

            /// <summary>
            /// Returns the message sender.
            /// </summary>
            public object Sender { get; private set; }

            /// <summary>
            /// Returns the message arguments.
            /// </summary>
            public object EventArguments { get; private set; }
        }

        /// <summary>
        /// Describes an registered message consumer.
        /// </summary>
        class ConsumerEntry {
            /// <summary>
            /// Creates a new instance with the given arguments.
            /// </summary>
            /// <param name="messageConsumer">Message consumer</param>
            /// <param name="dataObject">Additional data object</param>
            public ConsumerEntry(IMessageConsumer messageConsumer, object dataObject) {
                MessageConsumer = messageConsumer;
                DataObject = dataObject;
            }

            /// <summary>
            /// Returns the message consumer.
            /// </summary>
            public IMessageConsumer MessageConsumer { get; private set; }

            /// <summary>
            /// Returns the additional data object.
            /// </summary>
            public object DataObject { get; private set; }
        }
    }
}