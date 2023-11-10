using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class ProtocolMessageHandler : MessageHandler, INetworkRequestHandler<ProtocolMessage>
    {
        public ProtocolMessageHandler(NetworkConnection connection) : base(connection)
        {
        }
        
        public IRequestResponse Handle(ProtocolMessage message)
        {
            protocol.LoadData(message.value.value ?? new JObject());
            return protocol.CreateProtocolMessage();
        }

    }
}