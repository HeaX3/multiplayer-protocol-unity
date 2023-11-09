using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public interface INetworkMessage : ISerializableValue
    {
        IEnumerable<ISerializableValue> values { get; }
        
        void ISerializableValue.SerializeInto(SerializedMessage message)
        {
            foreach (var value in values) value.SerializeInto(message);
        }

        void ISerializableValue.DeserializeFrom(SerializedMessage message)
        {
            foreach (var value in values) value.DeserializeFrom(message);
        }
    }
}