namespace MultiplayerProtocol
{
    public class TimeoutException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.RequestTimeout;

        public TimeoutException(string message) : base(message)
        {
        }
    }
}