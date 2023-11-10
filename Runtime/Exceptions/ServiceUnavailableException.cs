namespace MultiplayerProtocol
{
    public class ServiceUnavailableException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.ServiceUnavailable;

        public ServiceUnavailableException(string message) : base(message)
        {
        }
    }
}