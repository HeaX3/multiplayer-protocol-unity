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
                            connection.responseSender.SendResponse(
                                message.requestId,
                                e as IRequestResponse ?? new RequestResponse(StatusCode.InternalServerError, e)
                            );
                        });
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                if (e is not IRequestResponse)
                {
                    Debug.LogError("Error handling request of type " + payload.GetType().Name + ":");
                    Debug.LogError(e);
                    connection.responseSender.SendResponse(
                        message.requestId,
                        RequestResponse.InternalServerError()
                    );
                }

                connection.responseSender.SendResponse(
                    message.requestId,
                    e as IRequestResponse ?? new RequestResponse(StatusCode.InternalServerError, e)
                );
            }
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