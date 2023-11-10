using Essentials;

namespace MultiplayerProtocol
{
    public class NamespacedKeyValue : ISerializableValue<NamespacedKey>
    {
        public NamespacedKey value { get; set; }

        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = message.ReadNamespacedKey();
        }
    }
}