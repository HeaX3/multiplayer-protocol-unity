
namespace MultiplayerProtocol
{
    public class TooManyRequestsException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.TooManyRequests;

        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}

