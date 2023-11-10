using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector4Value : ISerializableValue<Vector4>
    {
        public Vector4 value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadVector4();
        }
    }
}