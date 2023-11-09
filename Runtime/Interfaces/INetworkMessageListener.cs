using System;

namespace MultiplayerProtocol
{
    public interface INetworkMessageListener
    {
        string messageId { get; }
        Type messageType { get; }
        INetworkMessage CreateMessageInstance();
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