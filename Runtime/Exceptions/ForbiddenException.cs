namespace MultiplayerProtocol
{
    public class ForbiddenException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.Forbidden;

        public ForbiddenException(string message) : base(message)
        {
        }
    }
}