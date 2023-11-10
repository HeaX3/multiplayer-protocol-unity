namespace MultiplayerProtocol
{
    public class ForbiddenException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.Forbidden;

        public ForbiddenException(string message) : base(message)
        {
        }
    }
}