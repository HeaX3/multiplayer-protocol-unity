using System;
using GZipCompress;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class RequestMessage : INetworkMessage
    {
        public Guid requestId { get; private set; }
        public ushort messageId { get; private set; }
        public SerializedData message { get; private set; }

        public RequestMessage()
        {
        }

        public RequestMessage(ushort messageId, [NotNull] INetworkMessage message)
        {
            requestId = Guid.NewGuid();
            this.messageId = messageId;
            this.message = message.Serialize();
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(requestId);
            message.Write(messageId);
            var data = this.message.ToArray();
            if (data.Length == 0)
            {
                message.Write(0);
                return;
            }

            var compressed = GZipCompressor.Compress(data);
            message.Write(compressed.Length);
            message.Write(compressed);
        }

        public void DeserializeFrom(SerializedData message)
        {
            requestId = message.ReadGuid();
            messageId = message.ReadUShort();
            var compressedLength = message.ReadInt();
            if (compressedLength == 0)
            {
                this.message = new SerializedData(Array.Empty<byte>());
                return;
            }

            var compressed = message.ReadBytes(compressedLength);
            var data = GZipCompressor.Decompress(compressed);
            this.message = new SerializedData(data);
        }
    }
}