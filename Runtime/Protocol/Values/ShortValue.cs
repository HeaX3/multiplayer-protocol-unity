namespace MultiplayerProtocol
{
    public class ShortValue : ISerializableValue<short>
    {
        public short value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadShort();
        }
    }
}