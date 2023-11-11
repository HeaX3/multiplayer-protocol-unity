using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector3Value : ISerializableValue<Vector3>
    {
        public Vector3 value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadVector3();
        }
    }
}