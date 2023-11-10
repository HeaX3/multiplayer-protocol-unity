namespace MultiplayerProtocol
{
    public class ServiceUnavailableException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.ServiceUnavailable;

        public ServiceUnavailableException(string message) : base(message)
        {
        }
    }
}