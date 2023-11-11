using GZipCompress;

namespace MultiplayerProtocol
{
    public class StringValue : ISerializableValue<string>
    {
        public string value { get; set; }
        public bool useCompression { get; }

        public StringValue(bool useCompression = false)
        {
            this.useCompression = useCompression;
        }

        public void SerializeInto(SerializedData message)
        {
            var value = this.value;
            if (useCompression && value != null) value = GZipCompressor.CompressString(value);
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            var value = message.ReadString();
            if (useCompression && value != null) value = GZipCompressor.DecompressString(value);
            this.value = value;
        }
    }
}