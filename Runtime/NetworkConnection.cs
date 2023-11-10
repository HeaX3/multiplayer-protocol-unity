using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MultiplayerProtocol.Senders;
using Newtonsoft.Json.Linq;
using RSG;
using UnityEngine;

namespace MultiplayerProtocol
{
    public abstract class NetworkConnection
    {
        private NetworkSender sender { get; }
        private ProtocolSender protocolSender { get; }
        internal ResponseSender responseSender { get; }
        private INetworkEndpoint endpoint { get; }

        private Protocol _protocol;

        public Protocol protocol
        {
            get
            {
                _protocol ??= new Protocol(handlers
                    .Prepend(new ErrorHandler(this))
                    .Prepend(new RequestMessageHandler(this))
                    .Prepend(new ResponseMessageHandler(this))
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
                protocolSender.SendProtocol().Then(protocol =>
                {
                    this.protocol.LoadData(protocol.value.value ?? new JObject());
                    resolve();
                }).Catch(reject);
            });
        }

        public void Send([NotNull] INetworkMessage message)
        {
            Send(protocol.Serialize(message));
        }

        public void Send([NotNull] SerializedMessage message)
        {
            endpoint.Send(message);
        }

        public IPromise SendRequest<T>([NotNull] T message, uint timeoutMs = 5000)
            where T : INetworkMessage, new()
        {
            return sender.SendRequest(message, timeoutMs);
        }

        public IPromise<TResponse> SendRequest<TMessage, TResponse>([NotNull] TMessage message, uint timeoutMs = 5000)
            where TMessage : INetworkMessage
            where TResponse : ISerializableValue, new()
        {
            return sender.SendRequest<TMessage, TResponse>(message, timeoutMs);
        }

        private void OnMessageReceived(SerializedMessage message)
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