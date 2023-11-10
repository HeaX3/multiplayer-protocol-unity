namespace MultiplayerProtocol
{
    public class UnprocessableEntityException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.UnprocessableEntity;

        public UnprocessableEntityException(string message) : base(message)
        {
        }
    }
}