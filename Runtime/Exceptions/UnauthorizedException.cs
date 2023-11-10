namespace MultiplayerProtocol
{
    public class UnauthorizedException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}