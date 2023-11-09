namespace MultiplayerProtocol
{
    public class StringValue : ISerializableValue<string>
    {
        public string value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadString();
        }
    }
}