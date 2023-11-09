namespace MultiplayerProtocol
{
    public class UShortValue : ISerializableValue<ushort>
    {
        public ushort value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadUShort();
        }
    }
}