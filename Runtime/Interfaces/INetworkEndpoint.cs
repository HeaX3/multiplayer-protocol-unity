using System;

namespace MultiplayerProtocol
{
    public interface INetworkEndpoint
    {
        public delegate void CloseEvent();
        public delegate void MessageEvent(SerializedData message);

        public event MessageEvent received;
        public event CloseEvent closed;
        
        public bool isOpen { get; }

        public void Send(SerializedData message) => Send(message, default);
        public void Send(SerializedData message, DateTime expiration);
    }
}