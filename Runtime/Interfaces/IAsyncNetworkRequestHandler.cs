using RSG;

namespace MultiplayerProtocol
{
    public interface IAsyncNetworkRequestHandler : INetworkMessageListener
    {
        IPromise<IRequestResponse> Handle(INetworkMessage message, SerializedData serializedMessage) =>
            Handle(message);

        IPromise<IRequestResponse> Handle(INetworkMessage message);
        
        uint maxTimeoutMs => Protocol.DefaultTimeoutMs;
    }

    public interface IAsyncNetworkRequestHandler<in TRequestBody> : IAsyncNetworkRequestHandler,
        INetworkMessageListener<TRequestBody>
        where TRequestBody : INetworkMessage, new()
    {
        IPromise<IRequestResponse> IAsyncNetworkRequestHandler.Handle(INetworkMessage message,
            SerializedData serializedMessage)
        {
            if (message is not TRequestBody t)
            {
                return Promise<IRequestResponse>.Rejected(
                    new UnprocessableEntityException(GetType().Name + " cannot handle message of type " +
                                                     message.GetType().Name));
            }

            return Handle(t, serializedMessage);
        }

        IPromise<IRequestResponse> IAsyncNetworkRequestHandler.Handle(INetworkMessage message)
        {
            if (message is not TRequestBody t)
            {
                return Promise<IRequestResponse>.Rejected(
                    new UnprocessableEntityException(GetType().Name + " cannot handle message of type " +
                                                     message.GetType().Name));
            }

            return Handle(t);
        }

        IPromise<IRequestResponse> Handle(TRequestBody message, SerializedData serializedMessage) => Handle(message);
        IPromise<IRequestResponse> Handle(TRequestBody message);
    }
}