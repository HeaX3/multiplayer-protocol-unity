namespace MultiplayerProtocol
{
    public interface INetworkEndpoint
    {
        public delegate void CloseEvent();
        public delegate void MessageEvent(SerializedMessage message);

        public event MessageEvent received;
        public event CloseEvent closed;

        public void Send(SerializedMessage message);
    }
}