using System;

namespace MultiplayerProtocol
{
    public interface INetworkMessageListener
    {
        string messageId { get; }
        Type messageType { get; }
        INetworkMessage CreateMessageInstance();

        internal void Reject(INetworkMessage message, Exception error)
        {
            throw error;
        }
    }

    public interface INetworkMessageListener<in T> : INetworkMessageListener where T : INetworkMessage, new()
    {
        string INetworkMessageListener.messageId => typeof(T).FullName;
        Type INetworkMessageListener.messageType => typeof(T);

        INetworkMessage INetworkMessageListener.CreateMessageInstance()
        {
            return new T();
        }
    }
}