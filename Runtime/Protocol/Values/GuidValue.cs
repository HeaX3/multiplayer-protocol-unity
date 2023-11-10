using System;

namespace MultiplayerProtocol
{
    public class GuidValue : ISerializableValue<Guid>
    {
        public Guid value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadGuid();
        }
    }
}