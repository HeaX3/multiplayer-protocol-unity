namespace MultiplayerProtocol
{
    public class IntValue : ISerializableValue<int>
    {
        public int value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadInt();
        }
    }
}