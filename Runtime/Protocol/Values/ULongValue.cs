namespace MultiplayerProtocol
{
    public class ULongValue : ISerializableValue<ulong>
    {
        public ulong value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadULong();
        }
    }
}