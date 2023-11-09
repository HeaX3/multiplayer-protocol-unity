namespace MultiplayerProtocol
{
    public interface INetworkEndpoint
    {
        public delegate void MessageEvent(SerializedMessage message);

        public event MessageEvent received;

        public void Send(SerializedMessage message);
    }
}