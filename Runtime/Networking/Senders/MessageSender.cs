namespace MultiplayerProtocol.Senders
{
    public abstract class MessageSender
    {
        protected NetworkConnection connection { get; }

        protected MessageSender(NetworkConnection connection)
        {
            this.connection = connection;
        }
    }

    public abstract class MessageSender<T> : MessageSender where T : NetworkConnection
    {
        protected new T connection { get; }
        
        protected MessageSender(T connection) : base(connection)
        {
            this.connection = connection;
        }
    }
}