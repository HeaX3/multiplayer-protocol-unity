using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector3Value : ISerializableValue<Vector3>
    {
        public Vector3 value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.x);
            message.Write(value.y);
            message.Write(value.z);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new Vector3(message.ReadFloat(), message.ReadFloat(), message.ReadFloat());
        }
    }
}