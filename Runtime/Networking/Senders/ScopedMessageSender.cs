using System.Collections.Generic;

namespace MultiplayerProtocol.Senders
{
    /// <summary>
    /// Message sender that sends messages to multiple connections
    /// </summary>
    public interface ScopedMessageSender
    {
        IEnumerable<NetworkConnection> GetConnections();
        
        void Send(INetworkMessage message)
        {
            var type = message.GetType();
            var serialized = message.Serialize();
            foreach (var connection in GetConnections())
            {
                connection.Send(type, serialized);
            }
        }
    }
}