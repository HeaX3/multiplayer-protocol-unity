using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector3Value : ISerializableValue<Vector3>
    {
        public Vector3 value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadVector3();
        }
    }
}