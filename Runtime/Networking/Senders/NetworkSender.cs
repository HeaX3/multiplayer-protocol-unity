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

        public RequestPromise SendRequest(
            [NotNull] INetworkMessage message,
            uint timeoutMs = 5000
        )
        {
            return new RequestPromise((successHandler, reject, resolve) =>
            {
                if (!protocol.TryGetPartnerMessageId(message.GetType(), out var messageId))
                {
                    reject(new InvalidOperationException("Unknown message type " + message.GetType().Name));
                    return;
                }

                var requestMessage = new RequestMessage(messageId, message);
                protocol.AddResponseListener(requestMessage.requestId, timeoutMs, response =>
                {
                    // Debug.Log("Response has pre response messages: " + (response.preResponse.value?.Length > 0 ? "yes" : "no"));
                    // Debug.Log("Response has post response messages: " + (response.postResponse.value?.Length > 0 ? "yes" : "no"));
                    if (response.preResponse?.value != null) protocol.Handle(response.preResponse);
                    if (!response.isError && successHandler != null) successHandler();
                    if (response.postResponse?.value != null) protocol.Handle(response.postResponse);
                    if (!response.isError) resolve();
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }

        public RequestPromise<TResponse> SendRequest<TResponse>(
            [NotNull] INetworkMessage message, uint timeoutMs = 5000
        )
            where TResponse : ISerializableValue, new()
        {
            return new RequestPromise<TResponse>((resultHandler, reject, resolve) =>
            {
                if (!protocol.TryGetPartnerMessageId(message.GetType(), out var messageId))
                {
                    reject(new InvalidOperationException("Unknown message type " + message.GetType().Name));
                    return;
                }

                var requestMessage = new RequestMessage(messageId, message);
                protocol.AddResponseListener(requestMessage.requestId, timeoutMs, response =>
                {
                    if (response.preResponse?.value != null) protocol.Handle(response.preResponse);
                    if (!response.isError) resultHandler(response.value<TResponse>());
                    if (response.postResponse?.value != null) protocol.Handle(response.postResponse);
                    if (!response.isError) resolve();
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }
    }
}