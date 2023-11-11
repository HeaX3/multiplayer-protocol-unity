using UnityEngine;

namespace MultiplayerProtocol
{
    public class QuaternionValue : ISerializableValue<Quaternion>
    {
        public Quaternion value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadQuaternion();
        }
    }
}