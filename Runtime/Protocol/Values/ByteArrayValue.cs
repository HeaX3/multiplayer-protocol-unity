using GZipCompress;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class ByteArrayValue : ISerializableValue<byte[]>
    {
        [CanBeNull] public byte[] value { get; set; }
        public bool useCompression { get; }
        
        public ByteArrayValue(bool useCompression = false)
        {
            this.useCompression = useCompression;
        }

        public void SerializeInto(SerializedData message)
        {
            if (this.value == null)
            {
                message.Write(-1);
                return;
            }

            var value = useCompression ? GZipCompressor.Compress(this.value) : this.value;
            message.Write(value.Length);
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            var length = message.ReadInt();
            if (length < 0)
            {
                this.value = null;
                return;
            }

            var value = message.ReadBytes(length);
            if (useCompression) value = GZipCompressor.Decompress(value);
            this.value = value;
        }
    }
}