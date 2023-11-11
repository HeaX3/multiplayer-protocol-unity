using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class JsonValue : ISerializableValue<JObject>
    {
        public JObject value { get; set; }
        public bool useCompression { get; }

        public JsonValue(bool useCompression = false)
        {
            this.useCompression = useCompression;
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value, useCompression);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadJson(decompress: useCompression);
        }
    }
}