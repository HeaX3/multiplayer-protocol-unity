namespace MultiplayerProtocol
{
    public class ByteValue : ISerializableValue<byte>
    {
        public byte value { get; set; }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadByte();
        }
    }
}