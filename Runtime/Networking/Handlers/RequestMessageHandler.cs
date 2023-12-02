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

        public void Handle(RequestMessage message)
        {
            // Debug.Log(connection.protocol.name + ": Going to deserialize the actual request message of type " +
            //           (connection.protocol.TryGetMessageHandler(message.messageId, out var h)
            //               ? h.messageType.Name
            //               : "Unknown type"));
            var payload = protocol.Deserialize(message.message, message.messageId, out var handler);
            // Debug.Log(connection.protocol.name + ": Deserialized message is " + payload.GetType().Name);
            try
            {
                switch (handler)
                {
                    case INetworkMessageHandler simpleHandler:
                        simpleHandler.Handle(payload);
                        break;
                    case INetworkRequestHandler requestHandler:
                    {
                        var response = requestHandler.Handle(payload);
                        connection.responseSender.SendResponse(message.requestId, response);
                        break;
                    }
                    case IAsyncNetworkRequestHandler asyncHandler:
                    {
                        var timeout = TaskScheduler.RunLater(() => connection.responseSender.SendResponse(
                            message.requestId,
                            RequestResponse.RequestTimeout()
                        ), asyncHandler.maxTimeoutMs);

                        asyncHandler.Handle(payload).Then(response =>
                        {
                            if (timeout.isCancelled) return;
                            timeout.Cancel();
                            connection.responseSender.SendResponse(message.requestId, response);
                        }).Catch(e =>
                        {
                            if (timeout.isCancelled)
                            {
                                Debug.LogError(handler.GetType().Name +
                                               " encountered error after time already ran out: " +
                                               e);
                                return;
                            }

                            timeout.Cancel();
                            HandleError(message.requestId, payload.GetType(), e);
                        });
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                HandleError(message.requestId, payload.GetType(), e);
            }
        }

        void INetworkMessageListener.Reject(INetworkMessage message, Exception e)
        {
            if (message is not RequestMessage request)
            {
                throw e;
            }

            HandleError(request.requestId, request.GetType(), e);
        }

        private void HandleError(Guid requestId, Type messageType, Exception e)
        {
            if (e is IRequestResponse response and not InternalServerErrorException)
            {
                connection.responseSender.SendResponse(
                    requestId,
                    response
                );
                return;
            }

            Debug.LogError("Error handling request of type " + messageType.Name + ":");
            Debug.LogError(e);
            connection.responseSender.SendResponse(
                requestId,
                RequestResponse.InternalServerError()
            );
        }
    }
}