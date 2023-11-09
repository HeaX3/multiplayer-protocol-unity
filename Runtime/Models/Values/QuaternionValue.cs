using UnityEngine;

namespace MultiplayerProtocol
{
    public class QuaternionValue : ISerializableValue<Quaternion>
    {
        public Quaternion value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.x);
            message.Write(value.y);
            message.Write(value.z);
            message.Write(value.w);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new Quaternion(message.ReadFloat(), message.ReadFloat(), message.ReadFloat(), message.ReadFloat());
        }
    }
}