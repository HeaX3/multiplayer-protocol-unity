using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public class ProtocolResponseMessage : INetworkMessage
    {
        internal JsonValue value { get; } = new();

        public ProtocolResponseMessage()
        {
        }

        internal ProtocolResponseMessage(Protocol protocol)
        {
            value.value = protocol.ToJson();
        }

        public IEnumerable<ISerializableValue> values
        {
            get { yield return value; }
        }
    }
}