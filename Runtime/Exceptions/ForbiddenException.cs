namespace MultiplayerProtocol
{
    public class ForbiddenException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.Forbidden;

        public ForbiddenException(string message) : base(message)
        {
        }
    }
}