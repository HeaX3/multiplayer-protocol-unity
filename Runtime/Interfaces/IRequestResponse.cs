using System;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public interface IRequestResponse
    {
        StatusCode status { get; }
        bool isError { get; }

        SerializedMessages extra => null;
        
        [CanBeNull]
        Exception error();

        T value<T>() where T : ISerializableValue, new();
        byte[] ToBytes();
    }
}