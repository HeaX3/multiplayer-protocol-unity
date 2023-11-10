namespace MultiplayerProtocol
{
    public class UnauthorizedException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}