using UnityEngine;

namespace MultiplayerProtocol
{
    public interface INetworkMessageHandler : INetworkMessageListener
    {
        void Handle(INetworkMessage message, SerializedData serializedMessage) => Handle(message);
        void Handle(INetworkMessage message);
    }

    public interface INetworkMessageHandler<in T> : INetworkMessageHandler, INetworkMessageListener<T> where T : INetworkMessage, new()
    {
        void INetworkMessageHandler.Handle(INetworkMessage message, SerializedData serializedMessage)
        {
            if (message is not T t)
            {
                Debug.LogError(GetType().Name + " cannot handle message of type " + message.GetType().Name);
                return;
            }

            Handle(t, serializedMessage);
        }

        void INetworkMessageHandler.Handle(INetworkMessage message)
        {
            if (message is not T t)
            {
                Debug.LogError(GetType().Name + " cannot handle message of type " + message.GetType().Name);
                return;
            }

            Handle(t);
        }

        void Handle(T message, SerializedData serializedMessage) => Handle(message);
        void Handle(T message);
    }
}