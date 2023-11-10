﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class RequestMessage : INetworkMessage
    {
        public GuidValue requestId { get; } = new();
        public UShortValue messageId { get; } = new();
        private ISerializableValue value { get; }

        public RequestMessage()
        {
        }

        public RequestMessage(ushort messageId, [NotNull] ISerializableValue value)
        {
            requestId.value = Guid.NewGuid();
            this.messageId.value = messageId;
            this.value = value;
        }

        public IEnumerable<ISerializableValue> values
        {
            get
            {
                yield return requestId;
                yield return messageId;
                if (value != default) yield return value;
            }
        }
    }
}