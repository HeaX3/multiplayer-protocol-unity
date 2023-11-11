using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector3IntValue : ISerializableValue<Vector3Int>
    {
        public Vector3Int value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadVector3Int();
        }
    }
}