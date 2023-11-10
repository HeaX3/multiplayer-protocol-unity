using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector2IntValue : ISerializableValue<Vector2Int>
    {
        public Vector2Int value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadVector2Int();
        }
    }
}