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
            if (!protocol.TryGetAndRemoveResponseListener(message.requestId, out var listener))
            {
                Debug.LogError("Protocol error: Received response for unknown request " + message.requestId);
                return;
            }

            listener.Receive(message);
        }
    }
}