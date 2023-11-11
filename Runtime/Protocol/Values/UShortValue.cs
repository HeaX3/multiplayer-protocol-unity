namespace MultiplayerProtocol
{
    public class UShortValue : ISerializableValue<ushort>
    {
        public ushort value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadUShort();
        }
    }
}