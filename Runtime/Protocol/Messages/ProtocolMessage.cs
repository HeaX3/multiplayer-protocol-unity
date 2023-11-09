using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public class ProtocolMessage : INetworkMessage
    {
        internal JsonValue value { get; } = new();

        public ProtocolMessage()
        {
        }

        internal ProtocolMessage(Protocol protocol)
        {
            value.value = protocol.ToJson();
        }

        public IEnumerable<ISerializableValue> values
        {
            get { yield return value; }
        }
    }
}