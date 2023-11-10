namespace MultiplayerProtocol
{
    public class TimeoutException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.RequestTimeout;

        public TimeoutException(string message) : base(message)
        {
        }
    }
}