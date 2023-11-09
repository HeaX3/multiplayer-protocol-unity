namespace MultiplayerProtocol
{
    public class NotFoundException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.NotFound;

        public NotFoundException(string message) : base(message)
        {
        }
    }
}