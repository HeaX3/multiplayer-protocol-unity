using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector2Value : ISerializableValue<Vector2>
    {
        public Vector2 value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadVector2();
        }
    }
}