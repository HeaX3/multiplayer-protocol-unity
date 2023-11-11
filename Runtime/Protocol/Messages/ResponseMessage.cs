using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class ResponseMessage : INetworkMessage
    {
        public GuidValue requestId { get; } = new();
        public EnumValue<StatusCode> status { get; } = new();
        [NotNull] public SerializedMessages preResponse { get; }
        public ByteArrayValue body { get; } = new();
        [NotNull] public SerializedMessages postResponse { get; }

        public bool isError => status.value != StatusCode.Ok;

        public Exception error() => RequestErrorResponse.Of(status.value,
            body.value != null ? Encoding.UTF8.GetString(body.value) : status.value.ToString());

        public T value<T>() where T : ISerializableValue, new()
        {
            return ParseResponse<T>(new SerializedData(body.value ?? Array.Empty<byte>()));
        }

        public ResponseMessage()
        {
            preResponse = new SerializedMessages();
            postResponse = new SerializedMessages();
        }

        public ResponseMessage(Guid requestId, [NotNull] IRequestResponse response)
        {
            this.requestId.value = requestId;
            status.value = response.status;
            preResponse = response.preResponse ?? new SerializedMessages();
            postResponse = response.postResponse ?? new SerializedMessages();
            body.value = response.ToBytes();
        }

        private T ParseResponse<T>(SerializedData message) where T : ISerializableValue, new()
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
                yield return preResponse;
                yield return body;
                yield return postResponse;
            }
        }
    }
}