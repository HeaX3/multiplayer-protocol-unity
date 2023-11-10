namespace MultiplayerProtocol
{
    public class InternalServerErrorException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.InternalServerError;

        public InternalServerErrorException(string message) : base(message)
        {
        }
    }
}