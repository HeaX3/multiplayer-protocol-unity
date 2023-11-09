namespace MultiplayerProtocol
{
    public class GoneException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.Gone;

        public GoneException(string message) : base(message)
        {
        }
    }
}