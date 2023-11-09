using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector2IntValue : ISerializableValue<Vector2Int>
    {
        public Vector2Int value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.x);
            message.Write(value.y);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new Vector2Int(message.ReadInt(), message.ReadInt());
        }
    }
}