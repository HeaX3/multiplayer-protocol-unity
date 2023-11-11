using System;

namespace MultiplayerProtocol
{
    public class DateTimeValue : ISerializableValue<DateTime>
    {
        public DateTime value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadDateTime();
        }
    }
}