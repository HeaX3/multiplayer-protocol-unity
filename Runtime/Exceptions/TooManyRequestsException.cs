
namespace MultiplayerProtocol
{
    public class TooManyRequestsException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.TooManyRequests;

        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}

