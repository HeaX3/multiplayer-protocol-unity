using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class ResponseMessage : INetworkMessage
    {
        public GuidValue requestId { get; } = new();
        public EnumValue<StatusCode> status { get; } = new();
        public SerializedMessages preResponse { get; }
        public ByteArrayValue body { get; } = new();
        public SerializedMessages postResponse { get; }

        public ResponseMessage()
        {
        }

        public ResponseMessage(Guid requestId, [NotNull] IRequestResponse response)
        {
            this.requestId.value = requestId;
            status.value = response.status;
            preResponse = response.preResponse;
            postResponse = response.postResponse;
            body.value = response.ToBytes();
        }

        public T ParseResponse<T>(SerializedData message) where T : ISerializableValue, new()
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
                yield return preResponse ?? new SerializedMessages();
                yield return body;
                yield return postResponse ?? new SerializedMessages();
            }
        }
    }
}