using System;

namespace MultiplayerProtocol
{
    public class ArrayValue<T> : ISerializableValue<T[]> where T : ISerializableValue, new()
    {
        public T[] value { get; set; }

        public void SerializeInto(SerializedData message)
        {
            var length = this.value?.Length ?? 0;
            message.Write(length);
            if (this.value == null || length == 0) return;
            foreach (var value in this.value)
            {
                value.SerializeInto(message);
            }
        }

        public void DeserializeFrom(SerializedData message)
        {
            var length = message.ReadInt();
            if (length == 0)
            {
                this.value = Array.Empty<T>();
                return;
            }

            var value = new T[length];
            for (var i = 0; i < length; i++)
            {
                var entry = new T();
                entry.DeserializeFrom(message);
                value[i] = entry;
            }

            this.value = value;
        }
    }
}