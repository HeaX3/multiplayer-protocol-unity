using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class ProtocolResponseMessageHandler : MessageHandler, INetworkMessageHandler<ProtocolResponseMessage>
    {
        public ProtocolResponseMessageHandler(NetworkConnection connection) : base(connection)
        {
        }

        public void Handle(ProtocolResponseMessage message)
        {
            protocol.LoadData(message.value ?? new JObject());
        }
    }
}