using System.Collections.Generic;
using System.Linq;
using GZipCompress;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class SerializedMessages : ISerializableValue<SerializedData[]>
    {
        [CanBeNull] public SerializedData[] value { get; set; }

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
            if (this.value == null || this.value.Length < 1)
            {
                message.Write(0);
                return;
            }

            var raw = new SerializedData();
            var value = this.value;
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
            if (compressedLength == 0)
            {
                this.value = null;
                return;
            }

            var compressed = message.ReadBytes(compressedLength);
            var raw = new SerializedData(GZipCompressor.Decompress(compressed));
            var count = raw.ReadInt();
            var value = new SerializedData[count];
            for (var i = 0; i < count; i++)
            {
                var length = raw.ReadInt();
                value[i] = new SerializedData(raw.ReadBytes(length));
            }

            this.value = value;
        }
    }
}