using System;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public interface IRequestResponse
    {
        StatusCode status { get; }
        bool isError { get; }

        SerializedMessages preResponse => null;
        SerializedMessages postResponse => null;
        
        [CanBeNull]
        Exception error();

        T value<T>() where T : ISerializableValue, new();
        byte[] ToBytes();
    }
}