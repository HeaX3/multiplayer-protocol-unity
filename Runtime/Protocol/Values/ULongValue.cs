namespace MultiplayerProtocol
{
    public class ULongValue : ISerializableValue<ulong>
    {
        public ulong value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadULong();
        }
    }
}