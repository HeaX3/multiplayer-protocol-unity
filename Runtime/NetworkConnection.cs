using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MultiplayerProtocol.Senders;
using RSG;
using UnityEngine;

namespace MultiplayerProtocol
{
    public abstract class NetworkConnection
    {
        public delegate void CloseEvent();

        public event CloseEvent closed = delegate { };

        private NetworkSender sender { get; }
        private ProtocolSender protocolSender { get; }
        internal ResponseSender responseSender { get; }
        private INetworkEndpoint endpoint { get; }

        private Protocol _protocol;

        public bool isOpen => endpoint.isOpen;

        public Protocol protocol
        {
            get
            {
                _protocol ??= new Protocol(GetType().Name, handlers
                    .Prepend(new ErrorHandler(this))
                    .Prepend(new RequestMessageHandler(this))
                    .Prepend(new ResponseMessageHandler(this))
                    .Prepend(new ProtocolResponseMessageHandler(this))
                    .Prepend(new ProtocolMessageHandler(this))
                );
                return _protocol;
            }
        }

        protected abstract IEnumerable<INetworkMessageListener> handlers { get; }

        public NetworkConnection(INetworkEndpoint endpoint)
        {
            this.endpoint = endpoint;
            sender = new NetworkSender(this);
            protocolSender = new ProtocolSender(this);
            responseSender = new ResponseSender(this);
            endpoint.closed += () => closed();
            endpoint.received += OnMessageReceived;
        }

        /// <summary>
        /// Send the expected protocol to the other side, and receive their expected protocol.
        /// Only trigger this handshake either in the server or in the client, ideally right after the connection is established.
        /// </summary>
        public IPromise PerformProtocolHandshake()
        {
            return new Promise((resolve, reject) =>
            {
                protocolSender.SendProtocol()
                    .ThenAccept(response => protocol.Handle(response))
                    .Then(resolve)
                    .Catch(reject);
            });
        }

        /// <summary>
        /// Serialize the network message and then send it
        /// </summary>
        /// <param name="message">Unserialized message</param>
        /// <param name="expiration">Time after which this message will no longer be sent</param>
        public void Send([NotNull] INetworkMessage message, DateTime expiration = default)
        {
            Send(protocol.Serialize(message), expiration);
        }

        /// <summary>
        /// Send an already serialized message, but first mark it with the appropriate message id. In order for this
        /// to work, the message must not yet contain a message id.
        ///
        /// This is useful if a message is sent to multiple recipients since the message only needs to be serialized once,
        /// but it can still be marked with a variable message id depending on the agreed upon protocol between the
        /// server and the client for each connection.
        /// </summary>
        /// <param name="type">Message type</param>
        /// <param name="message">Serialized message</param>
        /// <param name="expiration">Time after which this message will no longer be sent</param>
        /// <exception cref="InvalidOperationException">Thrown if the recipient cannot handle the provided message type</exception>
        public void Send([NotNull] Type type, [NotNull] SerializedData message, DateTime expiration = default)
        {
            if (!protocol.TryGetPartnerMessageId(type, out var messageId))
            {
                throw new InvalidOperationException("Recipient cannot handle message of type " + type.Name);
            }

            message.InsertUShort(messageId);
            Send(message, expiration);
        }

        /// <summary>
        /// Send an already serialized message. In order for this to work, the message must be marked with a
        /// message type id that the recipient understands.
        /// </summary>
        /// <param name="message">Serialized message preceded by the message id</param>
        /// <param name="expiration">Cancel sending the message if it takes longer than this time to process previous messages</param>
        public void Send([NotNull] SerializedData message, DateTime expiration = default)
        {
            endpoint.Send(message, expiration);
        }

        public RequestPromise SendRequest(
            [NotNull] INetworkMessage message,
            uint timeoutMs = 5000
        ) => sender.SendRequest(message, timeoutMs);

        public RequestPromise<TResponse> SendRequest<TResponse>(
            [NotNull] INetworkMessage message,
            uint timeoutMs = 5000
        )
            where TResponse : ISerializableValue, new()
        {
            return sender.SendRequest<TResponse>(message, timeoutMs);
        }

        private void OnMessageReceived(SerializedData message)
        {
            try
            {
                protocol.Handle(message);
            }
            catch (Exception e)
            {
                if (e is RequestErrorResponse requestError)
                {
                    Send(new ErrorMessage(requestError));
                }
                else
                {
                    Debug.LogError(e);
                }
            }
        }
    }
}