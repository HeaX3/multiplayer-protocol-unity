using RSG;

namespace MultiplayerProtocol.Senders
{
    public class ProtocolSender : MessageSender
    {
        public ProtocolSender(NetworkConnection connection) : base(connection)
        {
        }

        public IPromise<ProtocolMessage> SendProtocol()
        {
            return connection.SendRequest<ProtocolMessage, ProtocolMessage>(
                connection.protocol.CreateProtocolMessage()
            );
        }
    }
}