using System;
using JetBrains.Annotations;
using RSG;
using UnityEngine;

namespace MultiplayerProtocol.Senders
{
    public sealed class NetworkSender
    {
        public NetworkConnection connection { get; }
        public Protocol protocol => connection.protocol;

        internal NetworkSender(NetworkConnection connection)
        {
            this.connection = connection;
        }

        public IPromise SendRequest<T>(
            [NotNull] T message,
            uint timeoutMs = 5000
        )
            where T : INetworkMessage
        {
            return SendRequest(message, null, null, timeoutMs);
        }

        public IPromise SendRequest<T>(
            [NotNull] T message,
            Action responseHandler,
            uint timeoutMs = 5000
        )
            where T : INetworkMessage
        {
            return SendRequest(message, responseHandler, null, timeoutMs);
        }

        public IPromise SendRequest<T>(
            [NotNull] T message,
            Action responseHandler,
            Action<Exception> errorHandler,
            uint timeoutMs = 5000
        )
            where T : INetworkMessage
        {
            return new Promise((resolve, reject) =>
            {
                if (!protocol.TryGetPartnerMessageId(message.GetType(), out var messageId))
                {
                    reject(new InvalidOperationException("Unknown message type " + message.GetType().Name));
                    return;
                }

                var requestMessage = new RequestMessage(messageId, message);
                protocol.AddResponseListener(requestMessage.requestId.value, timeoutMs, response =>
                {
                    Debug.Log("Response has pre response messages: " + (response.preResponse.value?.Length > 0 ? "yes" : "no"));
                    Debug.Log("Response has post response messages: " + (response.postResponse.value?.Length > 0 ? "yes" : "no"));
                    if (response.preResponse?.value != null) protocol.Handle(response.preResponse);
                    if (!response.isError && responseHandler != null) responseHandler();
                    else if (response.isError && errorHandler != null) errorHandler(response.error());
                    if (response.postResponse?.value != null) protocol.Handle(response.postResponse);
                    if (!response.isError) resolve();
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }

        public IPromise SendRequest<TMessage, TResponse>(
            [NotNull] TMessage message,
            Action<TResponse> responseHandler,
            uint timeoutMs = 5000
        )
            where TMessage : INetworkMessage
            where TResponse : ISerializableValue, new()
        {
            return SendRequest(message, responseHandler, null, timeoutMs);
        }

        public IPromise SendRequest<TMessage, TResponse>(
            [NotNull] TMessage message,
            Action<TResponse> responseHandler,
            Action<Exception> errorHandler,
            uint timeoutMs = 5000
        )
            where TMessage : INetworkMessage
            where TResponse : ISerializableValue, new()
        {
            return new Promise((resolve, reject) =>
            {
                if (!protocol.TryGetPartnerMessageId(message.GetType(), out var messageId))
                {
                    reject(new InvalidOperationException("Unknown message type " + message.GetType().Name));
                    return;
                }

                var requestMessage = new RequestMessage(messageId, message);
                protocol.AddResponseListener(requestMessage.requestId.value, timeoutMs, response =>
                {
                    if (response.preResponse?.value != null) protocol.Handle(response.preResponse);
                    if (!response.isError) responseHandler(response.value<TResponse>());
                    else if (errorHandler != null) errorHandler(response.error());
                    if (response.postResponse?.value != null) protocol.Handle(response.postResponse);
                    if (!response.isError) resolve();
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }
    }
}