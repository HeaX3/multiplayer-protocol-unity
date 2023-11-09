namespace MultiplayerProtocol
{
    public class InternalServerErrorException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.InternalServerError;

        public InternalServerErrorException(string message) : base(message)
        {
        }
    }
}