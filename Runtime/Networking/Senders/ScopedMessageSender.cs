using System.Collections.Generic;

namespace MultiplayerProtocol.Senders
{
    public abstract class ScopedMessageSender : IScopedMessageSender
    {
        public abstract IEnumerable<NetworkConnection> GetConnections();

        public void Send(INetworkMessage message) => ((IScopedMessageSender)this).Send(message);
    }
}