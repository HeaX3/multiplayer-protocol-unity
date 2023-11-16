namespace MultiplayerProtocol
{
    public class RejectedException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.Rejected;

        public RejectedException(string message) : base(message)
        {
        }
    }
}