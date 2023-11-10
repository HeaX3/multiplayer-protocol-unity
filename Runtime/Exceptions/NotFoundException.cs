namespace MultiplayerProtocol
{
    public class NotFoundException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.NotFound;

        public NotFoundException(string message) : base(message)
        {
        }
    }
}