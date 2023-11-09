namespace MultiplayerProtocol
{
    public class ShortValue : ISerializableValue<short>
    {
        public short value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadShort();
        }
    }
}