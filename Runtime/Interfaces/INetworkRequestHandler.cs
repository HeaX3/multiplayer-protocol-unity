﻿namespace MultiplayerProtocol
{
    public interface INetworkRequestHandler : INetworkMessageListener
    {
        IRequestResponse Handle(INetworkMessage message);
    }

    public interface INetworkRequestHandler<in T> : INetworkRequestHandler, INetworkMessageListener<T>
        where T : INetworkMessage, new()
    {
        IRequestResponse INetworkRequestHandler.Handle(INetworkMessage message)
        {
            if (message is not T t)
            {
                return RequestResponse.UnprocessableEntity(GetType().Name + " cannot handle message of type " +
                                                           message.GetType().Name);
            }

            return Handle(t);
        }

        IRequestResponse Handle(T message);
    }
}