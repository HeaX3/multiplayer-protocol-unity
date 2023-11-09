using System;
using System.Collections.Generic;

namespace MultiplayerProtocol
{
    public class ResponseMessage : INetworkMessage
    {
        public GuidValue id { get; } = new();
        public EnumValue<StatusCode> status { get; } = new();
        
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
                yield return id;
                yield return status;
            }
        }
    }
}