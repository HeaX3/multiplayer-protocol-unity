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
            var length = this.value?.Length ?? 0;
            message.Write(length);
            if (this.value == null || length == 0) return;

            var raw = new SerializedData();
            foreach (var value in this.value)
            {
                value.SerializeInto(raw);
            }

            var compressed = GZipCompressor.Compress(raw.ToArray());
            message.Write(compressed.Length);
            message.Write(compressed);
        }

        public void DeserializeFrom(SerializedData message)
        {
            var length = message.ReadInt();
            if (length == 0)
            {
                this.value = Array.Empty<T>();
                return;
            }

            var rawLength = message.ReadInt();
            var compressed = message.ReadBytes(rawLength);
            var raw = new SerializedData(GZipCompressor.Decompress(compressed));
            var value = new T[length];
            for (var i = 0; i < length; i++)
            {
                var entry = new T();
                entry.DeserializeFrom(raw);
                value[i] = entry;
            }

            this.value = value;
        }
    }
}