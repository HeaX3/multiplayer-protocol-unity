using UnityEngine;

namespace MultiplayerProtocol
{
    public class ResponseMessageHandler : MessageHandler, INetworkMessageHandler<ResponseMessage>
    {
        internal ResponseMessageHandler(NetworkConnection connection) : base(connection)
        {
        }

        public void Handle(ResponseMessage message)
        {
            if (!protocol.TryGetAndRemoveResponseListener(message.requestId.value, out var listener))
            {
                Debug.LogError("Protocol error: Received response for unknown request " + message.requestId.value);
                return;
            }

            listener.Receive(new RequestResponse(message.status.value, new SerializedMessage(message.body.value)));
        }
    }
}