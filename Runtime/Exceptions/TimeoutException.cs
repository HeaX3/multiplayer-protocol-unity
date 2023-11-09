namespace MultiplayerProtocol
{
    public class TimeoutException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.RequestTimeout;

        public TimeoutException(string message) : base(message)
        {
        }
    }
}