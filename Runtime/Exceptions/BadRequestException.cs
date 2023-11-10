namespace MultiplayerProtocol
{
    public class BadRequestException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.BadRequest;

        public BadRequestException(string message) : base(message)
        {
        }
    }
}