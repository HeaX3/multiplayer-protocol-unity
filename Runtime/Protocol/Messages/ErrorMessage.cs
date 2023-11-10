using System;
using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public class ErrorMessage : INetworkMessage
    {
        public StringValue error { get; } = new();
        public StringValue message { get; } = new();

        public ErrorMessage()
        {
        }

        public ErrorMessage(Exception exception)
        {
            error.value = exception.GetType().Name;
            message.value = exception.Message;
        }

        public IEnumerable<ISerializableValue> values
        {
            get
            {
                yield return error;
                yield return message;
            }
        }
    }
}