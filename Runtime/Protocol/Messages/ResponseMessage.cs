using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class ResponseMessage : INetworkMessage
    {
        public GuidValue requestId { get; } = new();
        public EnumValue<StatusCode> status { get; } = new();
        public ByteArrayValue body { get; } = new();

        public ResponseMessage()
        {
        }

        public ResponseMessage(Guid requestId, [NotNull] IRequestResponse response)
        {
            this.requestId.value = requestId;
            status.value = response.status;
            body.value = response.ToBytes();
        }

        public T ParseResponse<T>(SerializedMessage message) where T : ISerializableValue, new()
        {
            var result = new T();
            result.DeserializeFrom(message);
            return result;
        }

        public IEnumerable<ISerializableValue> values
        {
            get
            {
                yield return requestId;
                yield return status;
                yield return body;
            }
        }
    }
}