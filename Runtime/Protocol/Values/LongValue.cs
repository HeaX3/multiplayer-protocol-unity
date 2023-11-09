namespace MultiplayerProtocol
{
    public class LongValue : ISerializableValue<long>
    {
        public long value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadLong();
        }
    }
}