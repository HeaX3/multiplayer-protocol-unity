using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public interface INetworkMessage
    {
        IEnumerable<ISerializableValue> values { get; }
    }
}