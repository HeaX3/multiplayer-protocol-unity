using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector3IntValue : ISerializableValue<Vector3Int>
    {
        public Vector3Int value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadVector3Int();
        }
    }
}