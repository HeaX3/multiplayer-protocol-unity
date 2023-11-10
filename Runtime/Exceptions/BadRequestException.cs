namespace MultiplayerProtocol
{
    public class BadRequestException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.BadRequest;

        public BadRequestException(string message) : base(message)
        {
        }
    }
}