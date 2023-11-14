using System;

namespace MultiplayerProtocol
{
    public class ErrorMessage : INetworkMessage
    {
        public string error { get; private set; }
        public string message { get; private set; }

        public ErrorMessage()
        {
        }

        public ErrorMessage(Exception exception)
        {
            error = exception.GetType().Name;
            message = exception.Message;
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(error);
            message.Write(this.message);
        }

        public void DeserializeFrom(SerializedData message)
        {
            error = message.ReadString();
            this.message = message.ReadString();
        }
    }
}