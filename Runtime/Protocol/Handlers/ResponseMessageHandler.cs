using System;
using UnityEngine;

namespace MultiplayerProtocol
{
    public class ResponseMessageHandler : INetworkMessageListener<ResponseMessage>
    {
        private Protocol protocol { get; }

        internal ResponseMessageHandler(Protocol protocol)
        {
            this.protocol = protocol;
        }

        public void Handle(ResponseMessage message, SerializedMessage serializedMessage)
        {
            if (!protocol.TryGetAndRemoveResponseListener(message.id.value, out var listener))
            {
                Debug.LogError("Protocol error: Received response for unknown request " + message.id.value);
                return;
            }
            
            listener.Receive(new RequestResponse(message.status.value, serializedMessage));
        }

        public void Handle(ResponseMessage message)
        {
            // Will not run due to the override above
            throw new InvalidOperationException();
        }
    }
}