using System;
using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public interface INetworkMessage : ISerializableValue, IRequestResponse
    {
        StatusCode IRequestResponse.status => StatusCode.Ok;
        bool IRequestResponse.isError => false;
        Exception IRequestResponse.error() => null;
        T IRequestResponse.value<T>() => this is T ? (T)this : default;

        byte[] IRequestResponse.ToBytes()
        {
            var serialized = new SerializedMessage();
            SerializeInto(serialized);
            return serialized.ToArray();
        }

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