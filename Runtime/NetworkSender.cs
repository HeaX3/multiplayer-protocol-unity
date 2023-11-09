using System;
using RSG;

namespace MultiplayerProtocol
{
    public sealed class NetworkSender
    {
        public NetworkConnection connection { get; }
        public Protocol protocol { get; }

        internal NetworkSender(NetworkConnection connection)
        {
            this.connection = connection;
            protocol = connection.protocol;
        }

        public IPromise SendRequest<T>(T message, uint timeoutMs = 5000)
            where T : INetworkMessage, new()
        {
            return new Promise((resolve, reject) =>
            {
                if (!protocol.TryGetMessageId(message.GetType(), out var messageId))
                {
                    reject(new InvalidOperationException("Unknown message type " + message.GetType().Name));
                    return;
                }

                var requestMessage = new RequestMessage(messageId, message);
                protocol.AddResponseListener(requestMessage.id.value, timeoutMs, response =>
                {
                    if (!response.isError) resolve();
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }

        public IPromise<TResponse> SendRequest<TMessage, TResponse>(TMessage message, uint timeoutMs = 5000)
            where TMessage : INetworkMessage
            where TResponse : ISerializableValue, new()
        {
            return new Promise<TResponse>((resolve, reject) =>
            {
                if (!protocol.TryGetMessageId(message.GetType(), out var messageId))
                {
                    reject(new InvalidOperationException("Unknown message type " + message.GetType().Name));
                    return;
                }

                var requestMessage = new RequestMessage(messageId, message);
                protocol.AddResponseListener(requestMessage.id.value, timeoutMs, response =>
                {
                    if (!response.isError) resolve(response.value<TResponse>());
                    else reject(response.error());
                });
                connection.Send(requestMessage);
            });
        }
    }
}