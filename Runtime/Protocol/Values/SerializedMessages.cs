using System;
using System.Collections.Generic;
using System.Linq;
using GZipCompress;

namespace MultiplayerProtocol
{
    public class SerializedMessages : ISerializableValue<SerializedData[]>
    {
        public SerializedData[] value { get; set; }

        public SerializedMessages()
        {
        }

        public SerializedMessages(params SerializedData[] messages)
        {
            value = messages;
        }

        public SerializedMessages(IEnumerable<SerializedData> messages)
        {
            value = messages.ToArray();
        }

        public void SerializeInto(SerializedData message)
        {
            var raw = new SerializedData();
            var value = this.value ?? Array.Empty<SerializedData>();
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

        public void DeserializeFrom(SerializedData message)
        {
            var compressedLength = message.ReadInt();
            var compressed = message.ReadBytes(compressedLength);
            var raw = new SerializedData(GZipCompressor.Decompress(compressed));
            var count = raw.ReadInt();
            value = new SerializedData[count];
            for (var i = 0; i < count; i++)
            {
                var length = raw.ReadInt();
                value[i] = new SerializedData(raw.ReadBytes(length));
            }
        }
    }
}