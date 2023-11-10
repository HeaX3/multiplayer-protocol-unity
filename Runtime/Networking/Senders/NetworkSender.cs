using System;
using JetBrains.Annotations;
using RSG;

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

        public IPromise SendRequest<T>([NotNull] T message, uint timeoutMs = 5000)
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
                    if (!response.isError) resolve();
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }

        public IPromise<TResponse> SendRequest<TMessage, TResponse>([NotNull] TMessage message, uint timeoutMs = 5000)
            where TMessage : INetworkMessage
            where TResponse : ISerializableValue, new()
        {
            return new Promise<TResponse>((resolve, reject) =>
            {
                if (!protocol.TryGetPartnerMessageId(message.GetType(), out var messageId))
                {
                    reject(new InvalidOperationException("Unknown message type " + message.GetType().Name));
                    return;
                }

                var requestMessage = new RequestMessage(messageId, message);
                protocol.AddResponseListener(requestMessage.requestId.value, timeoutMs, response =>
                {
                    if (!response.isError) resolve(response.value<TResponse>());
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }
    }
}