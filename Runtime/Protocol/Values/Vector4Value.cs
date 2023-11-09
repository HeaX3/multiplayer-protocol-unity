using UnityEngine;

namespace MultiplayerProtocol
{
    public class Vector4Value : ISerializableValue<Vector4>
    {
        public Vector4 value { get; set; }
        
        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.x);
            message.Write(value.y);
            message.Write(value.z);
            message.Write(value.w);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new Vector4(message.ReadFloat(), message.ReadFloat(), message.ReadFloat(), message.ReadFloat());
        }
    }
}