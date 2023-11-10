namespace MultiplayerProtocol
{
    public class NotFoundException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.NotFound;

        public NotFoundException(string message) : base(message)
        {
        }
    }
}