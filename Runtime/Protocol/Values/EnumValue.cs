using System;

namespace MultiplayerProtocol
{
    public class EnumValue<T> : ISerializableValue<T> where T : struct, Enum
    {
        public T value { get; set; }

        public EnumValue()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException(nameof(value) + " is not an Enum");
            }
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write((int)(object)value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = (T)(object)message.ReadInt();
        }
    }
}