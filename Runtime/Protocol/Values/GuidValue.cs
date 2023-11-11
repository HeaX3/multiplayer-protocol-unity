using System;

namespace MultiplayerProtocol
{
    public class GuidValue : ISerializableValue<Guid>
    {
        public Guid value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadGuid();
        }
    }
}