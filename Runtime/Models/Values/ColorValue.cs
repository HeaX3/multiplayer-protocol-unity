using UnityEngine;

namespace MultiplayerProtocol
{
    public class ColorValue : ISerializableValue<Color>
    {
        public Color value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.r);
            message.Write(value.g);
            message.Write(value.b);
            message.Write(value.a);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new Color(message.ReadFloat(), message.ReadFloat(), message.ReadFloat(), message.ReadFloat());
        }
    }
}