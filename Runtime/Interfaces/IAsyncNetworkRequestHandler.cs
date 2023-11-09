using RSG;

namespace MultiplayerProtocol
{
    public interface IAsyncNetworkRequestHandler : INetworkMessageListener
    {
        IPromise<IRequestResponse> Handle(INetworkMessage message, SerializedMessage serializedMessage) =>
            Handle(message);

        IPromise<IRequestResponse> Handle(INetworkMessage message);
    }

    public interface IAsyncNetworkRequestHandler<in TRequestBody> : IAsyncNetworkRequestHandler,
        INetworkMessageListener<TRequestBody>
        where TRequestBody : INetworkMessage, new()
    {
        IPromise<IRequestResponse> IAsyncNetworkRequestHandler.Handle(INetworkMessage message,
            SerializedMessage serializedMessage)
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

        IPromise<IRequestResponse> Handle(TRequestBody message, SerializedMessage serializedMessage) => Handle(message);
        IPromise<IRequestResponse> Handle(TRequestBody message);
    }
}