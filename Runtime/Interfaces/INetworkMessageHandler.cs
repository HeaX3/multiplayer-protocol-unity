using UnityEngine;

namespace MultiplayerProtocol
{
    public interface INetworkMessageHandler : INetworkMessageListener
    {
        void Handle(INetworkMessage message);
    }

    public interface INetworkMessageHandler<in T> : INetworkMessageHandler, INetworkMessageListener<T> where T : INetworkMessage, new()
    {
        void INetworkMessageHandler.Handle(INetworkMessage message)
        {
            if (message is not T t)
            {
                Debug.LogError(GetType().Name + " cannot handle message of type " + message.GetType().Name);
                return;
            }

            Handle(t);
        }

        void Handle(T message);
    }
}