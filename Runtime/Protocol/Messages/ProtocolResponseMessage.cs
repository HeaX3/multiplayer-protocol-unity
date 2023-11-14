using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class ProtocolResponseMessage : INetworkMessage
    {
        internal JObject value { get; private set; }

        public ProtocolResponseMessage()
        {
        }

        internal ProtocolResponseMessage(Protocol protocol)
        {
            value = protocol.ToJson();
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadJson();
        }
    }
}