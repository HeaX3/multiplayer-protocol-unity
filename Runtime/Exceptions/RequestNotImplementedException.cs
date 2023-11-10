namespace MultiplayerProtocol
{
    public class RequestNotImplementedException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.NotImplemented;

        public RequestNotImplementedException(string message) : base(message)
        {
        }
    }
}