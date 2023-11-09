using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class ProtocolMessageHandler : INetworkMessageListener<ProtocolMessage>
    {
        private Protocol protocol { get; }

        internal ProtocolMessageHandler(Protocol protocol)
        {
            this.protocol = protocol;
        }

        public void Handle(ProtocolMessage message)
        {
            protocol.LoadData(message.value.value ?? new JObject());
        }
    }
}