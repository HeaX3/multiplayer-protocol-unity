namespace MultiplayerProtocol
{
    public class UnprocessableEntityException : SocketErrorResponse
    {
        public override StatusCode statusCode => StatusCode.UnprocessableEntity;

        public UnprocessableEntityException(string message) : base(message)
        {
        }
    }
}