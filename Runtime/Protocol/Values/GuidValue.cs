using System;

namespace MultiplayerProtocol
{
    public class GuidValue : ISerializableValue<Guid>
    {
        public Guid value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.ToString());
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            var valueString = message.ReadString();
            value = valueString != null && Guid.TryParse(valueString, out var guid) ? guid : default;
        }
    }
}