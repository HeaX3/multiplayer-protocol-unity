namespace MultiplayerProtocol
{
    public class RequestNotImplementedException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.NotImplemented;

        public RequestNotImplementedException(string message) : base(message)
        {
        }
    }
}