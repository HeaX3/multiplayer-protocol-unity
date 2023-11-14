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
            var serialized = new SerializedData();
            SerializeInto(serialized);
            return serialized.ToArray();
        }
    }
}