using RSG;

namespace MultiplayerProtocol.Senders
{
    public class ProtocolSender : MessageSender
    {
        public ProtocolSender(NetworkConnection connection) : base(connection)
        {
        }

        public RequestPromise<ProtocolResponseMessage> SendProtocol()
        {
            return connection.SendRequest<ProtocolResponseMessage>(connection.protocol.CreateProtocolMessage());
        }
    }
}