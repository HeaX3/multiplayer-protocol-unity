namespace MultiplayerProtocol
{
    public class IntValue : ISerializableValue<int>
    {
        public int value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadInt();
        }
    }
}