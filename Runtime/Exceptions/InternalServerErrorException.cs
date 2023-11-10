namespace MultiplayerProtocol
{
    public class InternalServerErrorException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.InternalServerError;

        public InternalServerErrorException(string message) : base(message)
        {
        }
    }
}