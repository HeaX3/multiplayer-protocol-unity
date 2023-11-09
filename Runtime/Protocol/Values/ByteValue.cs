namespace MultiplayerProtocol
{
    public class ByteValue : ISerializableValue<byte>
    {
        public byte value { get; set; }

        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadByte();
        }
    }
}