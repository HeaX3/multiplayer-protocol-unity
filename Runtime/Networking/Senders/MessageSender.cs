namespace MultiplayerProtocol.Senders
{
    public abstract class MessageSender
    {
        protected NetworkConnection connection { get; }

        public MessageSender(NetworkConnection connection)
        {
            this.connection = connection;
        }
    }
}