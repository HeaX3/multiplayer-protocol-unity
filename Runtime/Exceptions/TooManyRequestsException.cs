
namespace MultiplayerProtocol
{
    public class TooManyRequestsException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.TooManyRequests;

        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}

