namespace MultiplayerProtocol
{
    public interface IRateLimited : INetworkMessageListener
    {
        uint maxRequestsPerMinute => 60;
    }
}