using System;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public interface IRequestResponse
    {
        StatusCode status { get; }
        bool isError { get; }

        [CanBeNull]
        Exception error();

        T value<T>() where T : ISerializableValue, new();
        byte[] ToBytes();
    }
}