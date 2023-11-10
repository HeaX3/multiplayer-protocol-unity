namespace MultiplayerProtocol
{
    public class GoneException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.Gone;

        public GoneException(string message) : base(message)
        {
        }
    }
}