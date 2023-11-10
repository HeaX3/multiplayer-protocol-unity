namespace MultiplayerProtocol
{
    public class RequestNotImplementedException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.NotImplemented;

        public RequestNotImplementedException(string message) : base(message)
        {
        }
    }
}