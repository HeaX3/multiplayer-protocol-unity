using System;
using System.Collections.Generic;
using System.Linq;
using GZipCompress;

namespace MultiplayerProtocol
{
    public class MessageArrayValue : ISerializableValue<SerializedMessage[]>
    {
        public SerializedMessage[] value { get; set; }

        public MessageArrayValue()
        {
        }

        public MessageArrayValue(params SerializedMessage[] messages)
        {
            value = messages;
        }

        public MessageArrayValue(IEnumerable<SerializedMessage> messages)
        {
            value = messages.ToArray();
        }

        public void SerializeInto(SerializedMessage message)
        {
            var raw = new SerializedMessage();
            var value = this.value ?? Array.Empty<SerializedMessage>();
            raw.Write(value.Length);
            foreach (var m in value)
            {
                var bytes = m.ToArray();
                raw.Write(bytes.Length);
                raw.Write(bytes);
            }

            var compressed = GZipCompressor.Compress(raw.ToArray());
            message.Write(compressed.Length);
            message.Write(compressed);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            var compressedLength = message.ReadInt();
            var compressed = message.ReadBytes(compressedLength);
            var raw = new SerializedMessage(GZipCompressor.Decompress(compressed));
            var count = raw.ReadInt();
            value = new SerializedMessage[count];
            for (var i = 0; i < count; i++)
            {
                var length = raw.ReadInt();
                value[i] = new SerializedMessage(raw.ReadBytes(length));
            }
        }

        public static MessageArrayValue Create(Protocol protocol, params INetworkMessage[] messages) =>
            Create(protocol, (IEnumerable<INetworkMessage>)messages);

        public static MessageArrayValue Create(Protocol protocol, IEnumerable<INetworkMessage> messages)
        {
            return new MessageArrayValue(messages.Select(protocol.Serialize));
        }
    }
}