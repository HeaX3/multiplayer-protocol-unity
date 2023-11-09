using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public class NetworkConnection
    {
        public Protocol protocol { get; private set; }
        public NetworkSender sender { get; }

        internal INetworkEndpoint endpoint;

        protected virtual IEnumerable<INetworkMessageHandler> handlers { get; }

        public NetworkConnection(INetworkEndpoint endpoint, params INetworkMessageHandler[] handlers)
        {
            sender = new NetworkSender(this);
            endpoint.received += OnMessageReceived;
            this.handlers = handlers;
            Initialize();
        }

        public NetworkConnection(INetworkEndpoint endpoint)
        {
            sender = new NetworkSender(this);
            endpoint.received += OnMessageReceived;
        }

        public void Initialize()
        {
            protocol = new Protocol(handlers);
        }

        public void Send(INetworkMessage message) => Send(protocol.Serialize(message));

        public void Send(SerializedMessage message) => endpoint.Send(message);

        private void OnMessageReceived(SerializedMessage message)
        {
            protocol.Handle(message);
        }
    }
}