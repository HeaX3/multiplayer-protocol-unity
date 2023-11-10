using System;
using JetBrains.Annotations;

namespace MultiplayerProtocol.Senders
{
    public class ResponseSender : MessageSender
    {
        public ResponseSender(NetworkConnection connection) : base(connection)
        {
        }

        public void SendResponse(Guid requestId, [NotNull] IRequestResponse response)
        {
            connection.Send(new ResponseMessage(requestId, response));
        }
    }
}