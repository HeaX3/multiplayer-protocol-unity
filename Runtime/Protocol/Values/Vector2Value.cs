using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector2Value : ISerializableValue<Vector2>
    {
        public Vector2 value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadVector2();
        }
    }
}