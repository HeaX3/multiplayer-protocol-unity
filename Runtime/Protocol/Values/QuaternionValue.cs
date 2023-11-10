using UnityEngine;

namespace MultiplayerProtocol
{
    public class QuaternionValue : ISerializableValue<Quaternion>
    {
        public Quaternion value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadQuaternion();
        }
    }
}