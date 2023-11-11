namespace MultiplayerProtocol
{
    public class LongValue : ISerializableValue<long>
    {
        public long value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadLong();
        }
    }
}