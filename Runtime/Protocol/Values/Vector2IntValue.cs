using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector2IntValue : ISerializableValue<Vector2Int>
    {
        public Vector2Int value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadVector2Int();
        }
    }
}