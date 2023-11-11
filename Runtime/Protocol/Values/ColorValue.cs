using UnityEngine;

namespace MultiplayerProtocol
{
    public class ColorValue : ISerializableValue<Color>
    {
        public Color value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value.r);
            message.Write(value.g);
            message.Write(value.b);
            message.Write(value.a);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = new Color(message.ReadFloat(), message.ReadFloat(), message.ReadFloat(), message.ReadFloat());
        }
    }
}