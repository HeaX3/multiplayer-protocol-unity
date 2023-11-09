using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector2Value : ISerializableValue<Vector2>
    {
        public Vector2 value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.x);
            message.Write(value.y);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new Vector2(message.ReadFloat(), message.ReadFloat());
        }
    }
}