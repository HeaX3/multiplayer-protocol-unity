namespace MultiplayerProtocol
{
    public class BoolValue : ISerializableValue<bool>
    {
        public bool value { get; set; }

        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadBool();
        }
    }
}