using UnityEngine;

namespace MultiplayerProtocol
{
    public class ErrorHandler : INetworkMessageHandler<ErrorMessage>
    {
        private NetworkConnection connection { get; }
        
        public ErrorHandler(NetworkConnection connection)
        {
            this.connection = connection;
        }
        
        public void Handle(ErrorMessage message)
        {
            Debug.LogError(message);
        }
    }
}