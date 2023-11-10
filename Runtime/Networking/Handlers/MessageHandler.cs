namespace MultiplayerProtocol
{
    public abstract class MessageHandler
    {
        protected NetworkConnection connection { get; }
        protected Protocol protocol => connection.protocol;

        protected MessageHandler(NetworkConnection connection)
        {
            this.connection = connection;
        }
    }
}