using System;
using GZipCompress;

namespace MultiplayerProtocol
{
    public class ArrayValue<T> : ISerializableValue<T[]> where T : ISerializableValue, new()
    {
        public T[] value { get; set; }

        public ArrayValue()
        {
        }

        public ArrayValue(T[] value)
        {
            this.value = value;
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadArray<T>();
        }
    }
}