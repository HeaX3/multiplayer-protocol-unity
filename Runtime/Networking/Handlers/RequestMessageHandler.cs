using System;
using Essentials;
using UnityEngine;

namespace MultiplayerProtocol
{
    public class RequestMessageHandler : MessageHandler, INetworkMessageHandler<RequestMessage>
    {
        internal RequestMessageHandler(NetworkConnection connection) : base(connection)
        {
        }

        public void Handle(RequestMessage message, SerializedData serializedMessage)
        {
            var payload = protocol.Deserialize(serializedMessage, message.messageId, out var handler);
            if (handler is INetworkMessageHandler simpleHandler)
            {
                simpleHandler.Handle(payload, serializedMessage);
            }
            else if (handler is INetworkRequestHandler requestHandler)
            {
                var response = requestHandler.Handle(payload, serializedMessage);
                connection.responseSender.SendResponse(message.requestId, response);
            }
            else if (handler is IAsyncNetworkRequestHandler asyncHandler)
            {
                var timeout = TaskScheduler.RunLater(() => connection.responseSender.SendResponse(
                    message.requestId,
                    RequestResponse.RequestTimeout()
                ), asyncHandler.maxTimeoutMs);

                asyncHandler.Handle(payload, serializedMessage).Then(response =>
                {
                    if (timeout.isCancelled) return;
                    timeout.Cancel();
                    connection.responseSender.SendResponse(message.requestId, response);
                }).Catch(e =>
                {
                    if (timeout.isCancelled)
                    {
                        Debug.LogError(handler.GetType().Name + " encountered error after time already ran out: " + e);
                        return;
                    }

                    timeout.Cancel();
                    connection.responseSender.SendResponse(
                        message.requestId,
                        e as IRequestResponse ?? new RequestResponse(StatusCode.InternalServerError, e)
                    );
                });
            }
        }

        public void Handle(RequestMessage message)
        {
            // Will not run due to the override above
            throw new System.InvalidOperationException();
        }

        void INetworkMessageListener.Reject(INetworkMessage message, Exception e)
        {
            if (message is not RequestMessage request)
            {
                throw e;
            }

            if (e is not RequestErrorResponse || e is InternalServerErrorException)
            {
                Debug.LogError(e);
            }

            connection.Send(new ResponseMessage(
                request.requestId,
                e as RequestErrorResponse ?? (IRequestResponse)new RequestResponse(StatusCode.InternalServerError, e)
            ));
        }
    }
}