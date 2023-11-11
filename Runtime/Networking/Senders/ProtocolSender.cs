using RSG;

namespace MultiplayerProtocol.Senders
{
    public class ProtocolSender : MessageSender
    {
        public ProtocolSender(NetworkConnection connection) : base(connection)
        {
        }

        public IPromise SendProtocol()
        {
            return connection.SendRequest(connection.protocol.CreateProtocolMessage());
        }
    }
}