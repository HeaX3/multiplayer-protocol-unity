namespace MultiplayerProtocol
{
    public class ServiceUnavailableException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.ServiceUnavailable;

        public ServiceUnavailableException(string message) : base(message)
        {
        }
    }
}