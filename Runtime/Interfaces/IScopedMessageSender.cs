using System.Collections.Generic;

namespace MultiplayerProtocol
{
    /// <summary>
    /// Message sender that sends messages to multiple connections
    /// </summary>
    public interface IScopedMessageSender
    {
        IEnumerable<NetworkConnection> GetConnections();

        void Send(INetworkMessage message)
        {
            var type = message.GetType();
            var serialized = message.Serialize().ToArray();
            var instance = new SerializedData();
            foreach (var connection in GetConnections())
            {
                instance.Write(serialized);
                connection.Send(type, instance);
                instance.Reset();
            }
        }
    }
}