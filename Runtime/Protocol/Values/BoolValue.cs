namespace MultiplayerProtocol
{
    public class BoolValue : ISerializableValue<bool>
    {
        public bool value { get; set; }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadBool();
        }
    }
}