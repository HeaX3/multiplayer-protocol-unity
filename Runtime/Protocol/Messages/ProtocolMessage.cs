using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class ProtocolMessage : INetworkMessage
    {
        internal JObject value { get; private set; }

        public ProtocolMessage()
        {
        }

        internal ProtocolMessage(Protocol protocol)
        {
            value = protocol.ToJson();
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value, compress: true);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadJson(decompress: true);
        }
    }
}