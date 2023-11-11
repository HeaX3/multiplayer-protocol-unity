namespace MultiplayerProtocol
{
    public class UIntValue : ISerializableValue<uint>
    {
        public uint value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadUInt();
        }
    }
}