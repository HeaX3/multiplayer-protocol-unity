using System;
using System.Text;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class ResponseMessage : INetworkMessage
    {
        public Guid requestId { get; private set; }
        public StatusCode status { get; private set; }
        public SerializedMessages preResponse { get; private set; }
        public byte[] body { get; private set; }
        public SerializedMessages postResponse { get; private set; }

        public bool isError => status != StatusCode.Ok;

        public Exception error() => RequestErrorResponse.Of(status,
            body != null ? Encoding.UTF8.GetString(body) : status.ToString());

        public T value<T>() where T : ISerializableValue, new()
        {
            return ParseResponse<T>(new SerializedData(body ?? Array.Empty<byte>()));
        }

        public ResponseMessage()
        {
        }

        public ResponseMessage(Guid requestId, [NotNull] IRequestResponse response)
        {
            this.requestId = requestId;
            status = response.status;
            preResponse = response.preResponse ?? new SerializedMessages();
            postResponse = response.postResponse ?? new SerializedMessages();
            body = response.ToBytes();
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(requestId);
            message.WriteEnum(status);
            message.Write(preResponse != null);
            if (preResponse != null) message.Write(preResponse);
            message.Write(body?.Length ?? -1);
            if (body != null) message.Write(body);
            message.Write(postResponse != null);
            if (postResponse != null) message.Write(postResponse);
        }

        public void DeserializeFrom(SerializedData message)
        {
            requestId = message.ReadGuid();
            status = message.ReadEnum<StatusCode>();
            preResponse = message.ReadBool() ? message.Read<SerializedMessages>() : null;
            var bytesLength = message.ReadInt();
            body = bytesLength >= 0 ? message.ReadBytes(bytesLength) : null;
            postResponse = message.ReadBool() ? message.Read<SerializedMessages>() : null;
        }

        private T ParseResponse<T>(SerializedData message) where T : ISerializableValue, new()
        {
            var result = new T();
            result.DeserializeFrom(message);
            return result;
        }
    }
}