namespace MultiplayerProtocol
{
    public class UnauthorizedException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}