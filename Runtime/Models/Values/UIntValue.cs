namespace MultiplayerProtocol
{
    public class UIntValue : ISerializableValue<uint>
    {
        public uint value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadUInt();
        }
    }
}