using RSG;
using UnityEngine;

namespace MultiplayerProtocol.Senders
{
    public class ProtocolSender : MessageSender
    {
        public ProtocolSender(NetworkConnection connection) : base(connection)
        {
        }

        public IPromise SendProtocol()
        {
            return connection.SendRequest(
                connection.protocol.CreateProtocolMessage(),
                (ProtocolResponseMessage response) =>
                {
                    connection.protocol.Handle(response);
                });
        }
    }
}