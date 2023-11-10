namespace MultiplayerProtocol
{
    public class UnprocessableEntityException : SocketErrorResponse
    {
        public override StatusCode status => StatusCode.UnprocessableEntity;

        public UnprocessableEntityException(string message) : base(message)
        {
        }
    }
}