using System;
using UnityEngine;

namespace MultiplayerProtocol
{
    public interface INetworkMessageHandler
    {
        string messageId { get; }
        Type messageType { get; }
        INetworkMessage CreateMessageInstance();
        void Handle(INetworkMessage message);
    }

    public interface INetworkMessageHandler<in T> : INetworkMessageHandler where T : INetworkMessage, new()
    {
        Type INetworkMessageHandler.messageType => typeof(T);

        INetworkMessage INetworkMessageHandler.CreateMessageInstance()
        {
            return new T();
        }

        void INetworkMessageHandler.Handle(INetworkMessage message)
        {
            if (message is not T)
            {
                Debug.LogError(GetType().Name + " cannot handle message of type " + message.GetType().Name);
                return;
            }

            Handle(message);
        }

        void Handle(T message);
    }
}