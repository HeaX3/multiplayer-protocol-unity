namespace MultiplayerProtocol
{
    public class BadRequestException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.BadRequest;

        public BadRequestException(string message) : base(message)
        {
        }
    }
}