using Essentials;

namespace MultiplayerProtocol
{
    public class NamespacedKeyValue : ISerializableValue<NamespacedKey>
    {
        public NamespacedKey value { get; set; }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadNamespacedKey();
        }
    }
}