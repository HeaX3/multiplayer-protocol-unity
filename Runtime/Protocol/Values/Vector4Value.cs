using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector4Value : ISerializableValue<Vector4>
    {
        public Vector4 value { get; set; }
        
        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadVector4();
        }
    }
}