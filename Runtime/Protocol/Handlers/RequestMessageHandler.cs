namespace MultiplayerProtocol
{
    public class RequestMessageHandler : INetworkMessageHandler<RequestMessage>
    {
        private Protocol protocol { get; }

        internal RequestMessageHandler(Protocol protocol)
        {
            this.protocol = protocol;
        }
        
        public void Handle(RequestMessage message, SerializedMessage serializedMessage)
        {
            var payload = protocol.Deserialize(serializedMessage, message.messageId.value, out var handler);
            if (handler is INetworkMessageHandler simpleHandler)
            {
                simpleHandler.Handle(payload, serializedMessage);
            } else if (handler is IAsyncNetworkRequestHandler asyncHandler)
            {
                asyncHandler.Handle(payload, serializedMessage);
            }
        }

        public void Handle(RequestMessage message)
        {
            // Will not run due to the override above
            throw new System.InvalidOperationException();
        }
    }
}