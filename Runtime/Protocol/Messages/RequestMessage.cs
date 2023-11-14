using System;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class RequestMessage : INetworkMessage
    {
        public Guid requestId { get; private set; }
        public ushort messageId { get; private set; }
        private INetworkMessage message { get; }

        public RequestMessage()
        {
        }

        public RequestMessage(ushort messageId, [NotNull] INetworkMessage message)
        {
            requestId = Guid.NewGuid();
            this.messageId = messageId;
            this.message = message;
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(requestId);
            message.Write(messageId);
            message.Write(this.message); // Note: message is deserialized separately on the receiving end
        }

        public void DeserializeFrom(SerializedData message)
        {
            requestId = message.ReadGuid();
            messageId = message.ReadUShort();
        }
    }
}