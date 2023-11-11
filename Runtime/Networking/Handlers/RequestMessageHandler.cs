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
            var payload = protocol.Deserialize(serializedMessage, message.messageId.value, out var handler);
            if (handler is INetworkMessageHandler simpleHandler)
            {
                simpleHandler.Handle(payload, serializedMessage);
            }
            else if (handler is INetworkRequestHandler requestHandler)
            {
                var response = requestHandler.Handle(payload, serializedMessage);
                connection.responseSender.SendResponse(message.requestId.value, response);
            }
            else if (handler is IAsyncNetworkRequestHandler asyncHandler)
            {
                var timeout = TaskScheduler.RunLater(() => connection.responseSender.SendResponse(
                    message.requestId.value,
                    RequestResponse.RequestTimeout()
                ), asyncHandler.maxTimeoutMs);

                asyncHandler.Handle(payload, serializedMessage).Then(response =>
                {
                    if (timeout.isCancelled) return;
                    timeout.Cancel();
                    connection.responseSender.SendResponse(message.requestId.value, response);
                }).Catch(e =>
                {
                    if (timeout.isCancelled)
                    {
                        Debug.LogError(handler.GetType().Name + " encountered error after time already ran out: " + e);
                        return;
                    }

                    timeout.Cancel();
                    connection.responseSender.SendResponse(
                        message.requestId.value,
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

            connection.Send(new ResponseMessage(
                request.requestId.value,
                e as RequestErrorResponse ?? (IRequestResponse)new RequestResponse(StatusCode.InternalServerError, e)
            ));
        }
    }
}