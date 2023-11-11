namespace MultiplayerProtocol
{
    public interface INetworkEndpoint
    {
        public delegate void CloseEvent();
        public delegate void MessageEvent(SerializedData message);

        public event MessageEvent received;
        public event CloseEvent closed;

        public void Send(SerializedData message);
    }
}