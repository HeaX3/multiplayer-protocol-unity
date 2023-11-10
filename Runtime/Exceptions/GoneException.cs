namespace MultiplayerProtocol
{
    public class GoneException : RequestErrorResponse
    {
        public override StatusCode status => StatusCode.Gone;

        public GoneException(string message) : base(message)
        {
        }
    }
}