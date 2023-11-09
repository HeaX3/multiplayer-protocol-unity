using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector3IntValue : ISerializableValue<Vector3Int>
    {
        public Vector3Int value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.x);
            message.Write(value.y);
            message.Write(value.z);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new Vector3Int(message.ReadInt(), message.ReadInt(), message.ReadInt());
        }
    }
}