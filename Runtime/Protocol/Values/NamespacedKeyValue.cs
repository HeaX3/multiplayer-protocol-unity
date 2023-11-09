using Essentials;

namespace MultiplayerProtocol
{
    public class NamespacedKeyValue : ISerializableValue<NamespacedKey>
    {
        public NamespacedKey value { get; set; }

        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value.nameSpace);
            message.Write(value.key);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            value = new NamespacedKey(message.ReadString(), message.ReadString());
        }
    }
}